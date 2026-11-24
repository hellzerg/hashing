using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class MainForm : Form
    {
        // Always on top syscall... used TopMost property instead

        //static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        //const UInt32 SWP_NOSIZE = 0x0001;
        //const UInt32 SWP_NOMOVE = 0x0002;
        //const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        InputBox _pathInput;
        List<string> _fileList;
        List<FileSummary> _fileSummaries;
        List<string> _analyzedFiles;

        bool _allowExit;
        bool _closeAfterHashing;
        bool _settingsSaveWarningShown;

        // used when auto-enabling SHA256, for VirusTotal
        int _tempIndex = -1;

        // used for checking if all files are JSON files
        bool _allFilesAreJson = false;

        // used for counting total size of calculated sum
        long _totalSize;
        Stopwatch _timer;
        int _fileCounter;

        readonly bool _calculateFilesOnOpen;
        readonly bool _enableShellRequestServer;
        readonly Queue<string[]> _pendingShellRequests = new Queue<string[]>();
        bool _processingInput;
        ShellRequestServer _shellRequestServer;

        // used for filtering sum table
        MoonTreeNode[] _allSums = null;
        MoonTreeNode[] _identicalSums;
        MoonTreeNode[] _groupedSums;
        readonly Dictionary<string, Control> _controlsByName = new Dictionary<string, Control>(StringComparer.Ordinal);
        readonly int[] _modernButtonWidths = { 82, 100, 94, 100 };
        Button[] _modernButtons;

        // used for cancelling hashing procedure
        CancellationTokenSource tokenSource;

        readonly string _latestVersionLink = "https://raw.githubusercontent.com/hellzerg/hashing/master/version.txt";

        string _noNewVersionMessage = "You already have the latest version!";
        string _betaVersionMessage = "You are using an experimental version!";

        private string NewVersionMessage(string latestVersion)
        {
            return Options.TranslationList["newVersion"].ToString().Replace("{LATEST}", latestVersion).Replace("{CURRENT}", Program.GetCurrentVersionToString());
        }

        public MainForm(string[] files) : this(files, false, false)
        {
        }

        internal MainForm(string[] files, bool calculateFilesOnOpen, bool enableShellRequestServer)
        {
            _calculateFilesOnOpen = calculateFilesOnOpen;
            _enableShellRequestServer = enableShellRequestServer;
            InitializeComponent();
            _btnTools = new ModernButton
            {
                Name = "btnTools",
                Text = ToolText.Get("btnTools", "Tools"),
                Font = Fluent.Ui(9f),
                Variant = ButtonVariant.Subtle,
                Glyph = Fluent.Glyphs.Settings,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            topPanel.Controls.Add(_btnTools);
            _btnTools.Click += btnTools_Click;
            _modernButtons = new[] { _btnTools, btnSaveJson, btnUpdate, btnOptions };
            InitializeSecurityToolsMenu();
            BuildModernLayout();
            lblversion.Tag = "secondary";
            WrapPathInput();
            CacheNamedControls();
            topPanel.Resize += topPanel_Resize;

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            this.DoubleBuffered = true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Options.ApplyTheme(this);

            _allowExit = !Options.CurrentOptions.TrayIcon;
            _fileList = new List<string>();

            ConfigureMenuGlyphs();
            helperMenu.Opening += HelperMenu_Opening;
            Fluent.StyleMenu(helperMenu);
            Fluent.StyleMenu(trayMenu);

            _timer = new Stopwatch();

            ProcessPriorityClass requestedPriority = Options.CurrentOptions.HighPriority
                ? ProcessPriorityClass.High
                : ProcessPriorityClass.Normal;
            string priorityError;
            if (!Utilities.TrySetProcessPriority(requestedPriority, out priorityError))
            {
                Options.CurrentOptions.HighPriority = false;
                Options.StartupWarnings.Add(Options.TranslationList["highPriorityFailed"].ToString().Replace("{ERROR}", priorityError));
            }

            // Hash options handling from right-click menu
            CheckMenuItemsByHashes();
            EnableHashMenuItemsEvent();

            // Translation-related
            if (Options.CurrentOptions.LanguageCode == LanguageCode.EN)
            {
                Translate(true);
            }
            else
            {
                Translate();
            }

            Shown += MainForm_Shown;

            if (files != null && files.Length > 0)
            {
                string[] startupFiles = files.ToArray();
                Shown += (sender, args) =>
                {
                    DetectFiles(startupFiles, _calculateFilesOnOpen);
                    StartShellRequestServer();
                };
            }
            else
            {
                Shown += (sender, args) => StartShellRequestServer();
            }

            RestoreWindowState();
            if (_calculateFilesOnOpen && WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            if (Options.CurrentOptions.StayOnTop)
            {
                // SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }

            lblversion.Text = Options.TranslationList["lblversion"].ToString().Replace("{VN}", Program.GetCurrentVersionToString());
            trayIcon.Visible = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Options.StartupWarnings.Count == 0)
                return;

            string[] warnings = Options.StartupWarnings.ToArray();
            Options.StartupWarnings.Clear();
            foreach (string warning in warnings)
                FluentMessageBox.Show(this, warning, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void StartShellRequestServer()
        {
            if (!_enableShellRequestServer || _shellRequestServer != null || IsDisposed)
                return;

            _shellRequestServer = new ShellRequestServer(ReceiveShellRequest);
            FormClosed += MainForm_FormClosed;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_shellRequestServer == null)
                return;

            _shellRequestServer.Dispose();
            _shellRequestServer = null;
        }

        private void ReceiveShellRequest(string[] files)
        {
            if (files == null || files.Length == 0 || IsDisposed || !IsHandleCreated)
                return;

            try
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (IsDisposed)
                        return;

                    if (!Visible)
                        Show();
                    if (WindowState == FormWindowState.Minimized)
                        WindowState = FormWindowState.Normal;
                    trayIcon.Visible = false;
                    Activate();
                    BringToFront();

                    _pendingShellRequests.Enqueue(files);
                    ProcessPendingShellRequests();
                });
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void ProcessPendingShellRequests()
        {
            if (IsDisposed || _processingInput || tokenSource != null || _pendingShellRequests.Count == 0)
                return;

            string[] files = _pendingShellRequests.Dequeue();
            DetectFiles(files, true);
        }

        private void CacheNamedControls()
        {
            foreach (Control control in Options.GetSelfAndChildrenRecursive(this))
            {
                if (!string.IsNullOrEmpty(control.Name) && !_controlsByName.ContainsKey(control.Name))
                    _controlsByName.Add(control.Name, control);
            }
        }

        private void ConfigureMenuGlyphs()
        {
            int dpi = DeviceDpi;
            Fluent.SetMenuGlyph(copyToolStripMenuItem, Fluent.Glyphs.Copy, dpi);
            Fluent.SetMenuGlyph(toolStripMenuItem4, Fluent.Glyphs.View, dpi);
            Fluent.SetMenuGlyph(removeToolStripMenuItem, Fluent.Glyphs.Delete, dpi);
            Fluent.SetMenuGlyph(toolStripMenuItem1, Fluent.Glyphs.Shield, dpi);
            Fluent.SetMenuGlyph(toolStripMenuItem2, Fluent.Glyphs.Search, dpi);
            Fluent.SetMenuGlyph(toolStripMenuItem3, Fluent.Glyphs.Search, dpi);
            Fluent.SetMenuGlyph(toolStripMenuItem5, Fluent.Glyphs.Tag, dpi);
            Fluent.SetMenuGlyph(clearToolStripMenuItem, Fluent.Glyphs.Clear, dpi);
        }

        private void EnableHashMenuItemsEvent()
        {
            itemMD5.Click += HashClickFromMenu;
            itemSHA1.Click += HashClickFromMenu;
            itemSHA256.Click += HashClickFromMenu;
            itemSHA384.Click += HashClickFromMenu;
            itemSHA512.Click += HashClickFromMenu;
            itemCRC32.Click += HashClickFromMenu;
            itemRIPEMD160.Click += HashClickFromMenu;
            itemSHA32.Click += HashClickFromMenu;
            itemSHA33.Click += HashClickFromMenu;
            itemSHA35.Click += HashClickFromMenu;
        }

        private void HashClickFromMenu(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null)
                return;

            HashOptions hashes = Options.CurrentOptions.HashOptions;
            bool enabled;
            switch (Convert.ToString(item.Tag))
            {
                case "md5": enabled = hashes.MD5 = !hashes.MD5; break;
                case "sha1": enabled = hashes.SHA1 = !hashes.SHA1; break;
                case "sha256": enabled = hashes.SHA256 = !hashes.SHA256; break;
                case "sha384": enabled = hashes.SHA384 = !hashes.SHA384; break;
                case "sha512": enabled = hashes.SHA512 = !hashes.SHA512; break;
                case "crc32": enabled = hashes.CRC32 = !hashes.CRC32; break;
                case "ripemd160": enabled = hashes.RIPEMD160 = !hashes.RIPEMD160; break;
                case "sha32": enabled = hashes.SHA3_256 = !hashes.SHA3_256; break;
                case "sha33": enabled = hashes.SHA3_384 = !hashes.SHA3_384; break;
                case "sha35": enabled = hashes.SHA3_512 = !hashes.SHA3_512; break;
                default: return;
            }

            item.ForeColor = enabled ? Fluent.AccentText : Fluent.Text;
            SaveSettingsSafely();

            if (SumResult.Sums.Count > 0)
                ReCalculateSums();
        }

        private void CheckMenuItemsByHashes()
        {
            HashOptions hashes = Options.CurrentOptions.HashOptions;
            itemMD5.ForeColor = hashes.MD5 ? Fluent.AccentText : Fluent.Text;
            itemSHA1.ForeColor = hashes.SHA1 ? Fluent.AccentText : Fluent.Text;
            itemSHA256.ForeColor = hashes.SHA256 ? Fluent.AccentText : Fluent.Text;
            itemSHA384.ForeColor = hashes.SHA384 ? Fluent.AccentText : Fluent.Text;
            itemSHA512.ForeColor = hashes.SHA512 ? Fluent.AccentText : Fluent.Text;
            itemCRC32.ForeColor = hashes.CRC32 ? Fluent.AccentText : Fluent.Text;
            itemRIPEMD160.ForeColor = hashes.RIPEMD160 ? Fluent.AccentText : Fluent.Text;
            itemSHA32.ForeColor = hashes.SHA3_256 ? Fluent.AccentText : Fluent.Text;
            itemSHA33.ForeColor = hashes.SHA3_384 ? Fluent.AccentText : Fluent.Text;
            itemSHA35.ForeColor = hashes.SHA3_512 ? Fluent.AccentText : Fluent.Text;
        }

        internal void Translate(bool skipFull = false)
        {
            if (!skipFull)
            {
                Dictionary<string, string> translationList = Options.TranslationList.ToObject<Dictionary<string, string>>();
                _noNewVersionMessage = Options.TranslationList["noNewVersion"];
                _betaVersionMessage = Options.TranslationList["betaVersion"];

                restoreToolStripMenuItem.Text = Options.TranslationList["restoreToolStripMenuItem"];
                exitToolStripMenuItem.Text = Options.TranslationList["exitToolStripMenuItem"];
                copyToolStripMenuItem.Text = Options.TranslationList["copyToolStripMenuItem"];
                toolStripMenuItem4.Text = Options.TranslationList["toolStripMenuItem4"];
                removeToolStripMenuItem.Text = Options.TranslationList["removeToolStripMenuItem"];
                toolStripMenuItem1.Text = Options.TranslationList["toolStripMenuItem1"];
                toolStripMenuItem2.Text = Options.TranslationList["toolStripMenuItem2"];
                toolStripMenuItem3.Text = Options.TranslationList["toolStripMenuItem3"];
                clearToolStripMenuItem.Text = Options.TranslationList["clearToolStripMenuItem"];

                Control element;

                foreach (var x in translationList)
                {
                    if (string.IsNullOrEmpty(x.Key)) continue;
                    if (_controlsByName.TryGetValue(x.Key, out element))
                        element.Text = x.Value;
                }
            }

            lblCalculating.HintText = Options.TranslationList["lblCalculating"].ToString();
            lblCalculating.Subtitle = Options.TranslationList["dropHintSubtitle"].ToString();
            lblversion.Text = Options.TranslationList["lblversion"].ToString().Replace("{VN}", Program.GetCurrentVersionToString());
            TranslateSecurityTools();
            SetSearchWatermark();
        }

        private void EnableSHA256()
        {
            Options.CurrentOptions.HashOptions.SHA256 = true;
            SaveSettingsSafely();

            OptionsForm.HashesChanged = true;

            ReCalculateSums();
        }

        private string NewDownloadLink(string latestVersion)
        {
            return string.Format("https://github.com/hellzerg/hashing/releases/download/{0}/Hashing-{0}.exe", latestVersion);
        }

        private void CheckForUpdate()
        {
            string latestVersion = string.Empty;
            try
            {
                using (WebClient client = new WebClient { Encoding = Encoding.UTF8 })
                    latestVersion = client.DownloadString(_latestVersionLink).Trim();
            }
            catch (Exception ex)
            {
                FluentMessageBox.Show(this, ex.Message, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!string.IsNullOrEmpty(latestVersion))
            {
                if (float.Parse(latestVersion) > Program.GetCurrentVersion())
                {
                    if (FluentMessageBox.Show(this, NewVersionMessage(latestVersion), "Hashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // PATCHING PROCESS
                        try
                        {
                            Assembly currentAssembly = Assembly.GetEntryAssembly();

                            if (currentAssembly == null)
                            {
                                currentAssembly = Assembly.GetCallingAssembly();
                            }

                            string appFolder = Path.GetDirectoryName(currentAssembly.Location);
                            string appName = Path.GetFileNameWithoutExtension(currentAssembly.Location);
                            string appExtension = Path.GetExtension(currentAssembly.Location);

                            string archiveFile = Path.Combine(appFolder, "Hashing_old" + appExtension);
                            string appFile = Path.Combine(appFolder, appName + appExtension);
                            string tempFile = Path.Combine(appFolder, "Hashing_tmp" + appExtension);

                            // DOWNLOAD NEW VERSION
                            using (WebClient downloadClient = new WebClient())
                            {
                                downloadClient.DownloadFile(NewDownloadLink(latestVersion), tempFile);
                            }

                            // DELETE PREVIOUS BACK-UP
                            if (File.Exists(archiveFile))
                            {
                                File.Delete(archiveFile);
                            }

                            // MAKE BACK-UP
                            File.Move(appFile, archiveFile);

                            // PATCH
                            File.Move(tempFile, appFile);

                            // BYPASS SINGLE-INSTANCE MECHANISM
                            if (Program.MUTEX != null)
                            {
                                Program.MUTEX.ReleaseMutex();
                                Program.MUTEX.Dispose();
                                Program.MUTEX = null;
                            }

                            Application.Restart();
                        }
                        catch (Exception ex)
                        {
                            FluentMessageBox.Show(this, ex.Message);
                        }
                    }
                }
                else if (float.Parse(latestVersion) == Program.GetCurrentVersion())
                {
                    FluentMessageBox.Show(this, _noNewVersionMessage, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    FluentMessageBox.Show(this, _betaVersionMessage, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Copy()
        {
            MoonTreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null)
                return;

            if (selectedNode.Nodes.Count == 0)
            {
                Utilities.CopyToClipboard(selectedNode.Text);
                return;
            }

            StringBuilder text = new StringBuilder(selectedNode.Text.Length + selectedNode.Nodes.Count * 96);
            text.Append(selectedNode.Text).Append(Environment.NewLine).Append(Environment.NewLine);

            foreach (MoonTreeNode node in selectedNode.Nodes)
                text.Append(node.Text).Append(Environment.NewLine);

            Utilities.CopyToClipboard(text.ToString(), true);
        }

        private void Remove()
        {
            MoonTreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null || selectedNode.Tag is DuplicateGroupInfo || selectedNode.Tag is DuplicateHashMatch)
                return;

            SumResult selectedResult = GetSelectedSumResult();
            if (selectedResult == null)
                return;

            int index = SumResult.Sums.IndexOf(selectedResult);
            if (index < 0)
                index = SumResult.Sums.FindIndex(item =>
                    string.Equals(item.File, selectedResult.File, StringComparison.OrdinalIgnoreCase));
            if (index >= 0)
                SumResult.Sums.RemoveAt(index);

            if (_allSums != null)
            {
                bool removedRoot = false;
                _allSums = _allSums.Where(node =>
                {
                    if (!removedRoot && ReferenceEquals(GetNodeSumResult(node), selectedResult))
                    {
                        removedRoot = true;
                        return false;
                    }
                    return true;
                }).ToArray();
                _identicalSums = null;
                _groupedSums = null;
                FilterTable();
            }
            else if (SumView.ContainsNode(selectedNode))
            {
                SumView.Nodes.Remove(selectedNode.Parent ?? selectedNode);
            }

            if (SumView.Nodes.Count == 0)
            {
                lblCalculating.Text = Options.TranslationList["lblCalculating"].ToString();
                lblCalculating.Visible = true;
                btnCancelHashing.Visible = false;
            }
        }

        private void Clear()
        {
            this.Text = "Hashing";
            this.AllowDrop = false;

            _allSums = null;
            _identicalSums = null;
            _groupedSums = null;
            txtPath.Clear();
            SumView.Nodes.Clear();
            SumResult.Sums.Clear();

            _timer.Reset();
            lblCalculating.Text = Options.TranslationList["lblCalculating"].ToString();
            lblCalculating.Visible = true;
            btnCancelHashing.Visible = false;

            this.AllowDrop = true;
        }

        private void SearchVirusTotal()
        {
            SumResult selectedResult = GetSelectedSumResult();
            int index = selectedResult == null ? -1 : SumResult.Sums.IndexOf(selectedResult);
            if (index < 0 && selectedResult != null)
                index = SumResult.Sums.FindIndex(x => string.Equals(x.File, selectedResult.File, StringComparison.OrdinalIgnoreCase));

            if (index < 0)
                return;

            if (!string.IsNullOrEmpty(SumResult.Sums[index].SHA256))
            {
                Utilities.SearchVirusTotal(SumResult.Sums[index].SHA256);
                return;
            }

            if (FluentMessageBox.Show(this, Options.TranslationList["enableSHA256Message"].ToString(), "Hashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                EnableSHA256();
                _tempIndex = index;
            }
        }

        private void RefreshSumList()
        {
            if (SumResult.Sums.Count == 0)
                return;

            List<MoonTreeNode> nodes = new List<MoonTreeNode>(SumResult.Sums.Count);
            foreach (SumResult sum in SumResult.Sums)
            {
                MoonTreeNode rootNode = new MoonTreeNode(sum.File)
                {
                    ForeColor = Fluent.AccentText,
                    Tag = new FileNodeInfo(sum, Options.ThemeFlag)
                };

                if (Options.CurrentOptions.HashOptions.MD5) rootNode.Nodes.Add("MD5: " + sum.MD5);
                if (Options.CurrentOptions.HashOptions.SHA1) rootNode.Nodes.Add("SHA1: " + sum.SHA1);
                if (Options.CurrentOptions.HashOptions.SHA256) rootNode.Nodes.Add("SHA256: " + sum.SHA256);
                if (Options.CurrentOptions.HashOptions.SHA384) rootNode.Nodes.Add("SHA384: " + sum.SHA384);
                if (Options.CurrentOptions.HashOptions.SHA512) rootNode.Nodes.Add("SHA512: " + sum.SHA512);
                if (Options.CurrentOptions.HashOptions.CRC32) rootNode.Nodes.Add("CRC32: " + sum.CRC32);
                if (Options.CurrentOptions.HashOptions.RIPEMD160) rootNode.Nodes.Add("RIPEMD160: " + sum.RIPEMD160);
                if (Options.CurrentOptions.HashOptions.SHA3_256) rootNode.Nodes.Add("SHA3-256: " + sum.SHA3_256);
                if (Options.CurrentOptions.HashOptions.SHA3_384) rootNode.Nodes.Add("SHA3-384: " + sum.SHA3_384);
                if (Options.CurrentOptions.HashOptions.SHA3_512) rootNode.Nodes.Add("SHA3-512: " + sum.SHA3_512);

                nodes.Add(rootNode);
            }

            SumView.BeginUpdate();
            try
            {
                SumView.Nodes.Clear();
                SumView.Nodes.AddRange(nodes.ToArray());
                SumView.ExpandAll();
                _allSums = SumView.Nodes.Cast<MoonTreeNode>().ToArray();
                _identicalSums = null;
                _groupedSums = null;
            }
            finally
            {
                SumView.EndUpdate();
            }

            FilterTable();
        }

        private static HashOptions CloneHashOptions(HashOptions source)
        {
            return new HashOptions
            {
                MD5 = source.MD5,
                SHA1 = source.SHA1,
                SHA256 = source.SHA256,
                SHA384 = source.SHA384,
                SHA512 = source.SHA512,
                CRC32 = source.CRC32,
                RIPEMD160 = source.RIPEMD160,
                SHA3_256 = source.SHA3_256,
                SHA3_384 = source.SHA3_384,
                SHA3_512 = source.SHA3_512
            };
        }

        private static string NormalizeVerificationPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            try
            {
                return Path.GetFullPath(path);
            }
            catch
            {
                return path;
            }
        }

        private static Dictionary<string, List<FileSummary>> BuildExpectedSummariesByFile(IEnumerable<FileSummary> summaries)
        {
            Dictionary<string, List<FileSummary>> result = new Dictionary<string, List<FileSummary>>(StringComparer.OrdinalIgnoreCase);
            if (summaries == null)
                return result;

            foreach (FileSummary summary in summaries)
            {
                if (summary == null || string.IsNullOrWhiteSpace(summary.File))
                    continue;

                string key = NormalizeVerificationPath(summary.File);
                List<FileSummary> fileSummaries;
                if (!result.TryGetValue(key, out fileSummaries))
                {
                    fileSummaries = new List<FileSummary>();
                    result.Add(key, fileSummaries);
                }
                fileSummaries.Add(summary);
            }

            return result;
        }

        private static Dictionary<string, HashOptions> BuildVerificationHashOptions(
            Dictionary<string, List<FileSummary>> expectedSummariesByFile, HashOptions activeOptions)
        {
            Dictionary<string, HashOptions> result = new Dictionary<string, HashOptions>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, List<FileSummary>> pair in expectedSummariesByFile)
            {
                HashOptions options = CloneHashOptions(activeOptions);
                foreach (FileSummary summary in pair.Value)
                {
                    options.MD5 |= !string.IsNullOrWhiteSpace(summary.MD5);
                    options.SHA1 |= !string.IsNullOrWhiteSpace(summary.SHA1);
                    options.SHA256 |= !string.IsNullOrWhiteSpace(summary.SHA256);
                    options.SHA384 |= !string.IsNullOrWhiteSpace(summary.SHA384);
                    options.SHA512 |= !string.IsNullOrWhiteSpace(summary.SHA512);
                    options.CRC32 |= !string.IsNullOrWhiteSpace(summary.CRC32);
                    options.RIPEMD160 |= !string.IsNullOrWhiteSpace(summary.RIPEMD160);
                    options.SHA3_256 |= !string.IsNullOrWhiteSpace(summary.SHA3_256);
                    options.SHA3_384 |= !string.IsNullOrWhiteSpace(summary.SHA3_384);
                    options.SHA3_512 |= !string.IsNullOrWhiteSpace(summary.SHA3_512);
                }
                result.Add(pair.Key, options);
            }
            return result;
        }

        private static bool VerifyExpectedSummaries(SumResult actual, IEnumerable<FileSummary> expectedSummaries)
        {
            bool hasExpectedHashes = false;
            foreach (FileSummary expected in expectedSummaries)
            {
                int compared = 0;
                bool matches =
                    CompareExpectedHash(actual.MD5, expected.MD5, ref compared) &&
                    CompareExpectedHash(actual.SHA1, expected.SHA1, ref compared) &&
                    CompareExpectedHash(actual.SHA256, expected.SHA256, ref compared) &&
                    CompareExpectedHash(actual.SHA384, expected.SHA384, ref compared) &&
                    CompareExpectedHash(actual.SHA512, expected.SHA512, ref compared) &&
                    CompareExpectedHash(actual.CRC32, expected.CRC32, ref compared) &&
                    CompareExpectedHash(actual.RIPEMD160, expected.RIPEMD160, ref compared) &&
                    CompareExpectedHash(actual.SHA3_256, expected.SHA3_256, ref compared) &&
                    CompareExpectedHash(actual.SHA3_384, expected.SHA3_384, ref compared) &&
                    CompareExpectedHash(actual.SHA3_512, expected.SHA3_512, ref compared);

                if (compared == 0)
                    continue;

                hasExpectedHashes = true;
                if (!matches)
                    return false;
            }

            return hasExpectedHashes;
        }

        private static bool CompareExpectedHash(string actual, string expected, ref int compared)
        {
            if (string.IsNullOrWhiteSpace(expected))
                return true;

            compared++;
            if (string.IsNullOrWhiteSpace(actual))
                return false;

            return string.Equals(actual.Trim(), expected.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static void ClearUnselectedHashes(SumResult result, HashOptions activeOptions)
        {
            if (!activeOptions.MD5) result.MD5 = null;
            if (!activeOptions.SHA1) result.SHA1 = null;
            if (!activeOptions.SHA256) result.SHA256 = null;
            if (!activeOptions.SHA384) result.SHA384 = null;
            if (!activeOptions.SHA512) result.SHA512 = null;
            if (!activeOptions.CRC32) result.CRC32 = null;
            if (!activeOptions.RIPEMD160) result.RIPEMD160 = null;
            if (!activeOptions.SHA3_256) result.SHA3_256 = null;
            if (!activeOptions.SHA3_384) result.SHA3_384 = null;
            if (!activeOptions.SHA3_512) result.SHA3_512 = null;
        }

        private MoonTreeNode[] BuildIdenticalNodes()
        {
            List<SumResult> fileResults = new List<SumResult>(_allSums == null ? 0 : _allSums.Length);
            if (_allSums != null)
            {
                foreach (MoonTreeNode node in _allSums)
                {
                    SumResult item = GetNodeSumResult(node);
                    if (item != null && !string.IsNullOrWhiteSpace(item.File))
                        fileResults.Add(item);
                }
            }

            return BuildIdenticalNodes(fileResults, CancellationToken.None);
        }

        private MoonTreeNode[] BuildIdenticalNodes(IEnumerable<SumResult> sourceItems, CancellationToken cancellationToken, List<string> operationErrors = null)
        {
            List<SumResult> fileResults = sourceItems == null
                ? new List<SumResult>()
                : sourceItems.Where(item => item != null && !string.IsNullOrWhiteSpace(item.File)).ToList();

            if (fileResults.Count < 2)
                return new MoonTreeNode[0];

            List<HashSelectorInfo> selectors = GetAvailableHashSelectors(fileResults);
            Dictionary<SumResult, int> itemIndexes = new Dictionary<SumResult, int>();
            for (int i = 0; i < fileResults.Count; i++)
                itemIndexes[fileResults[i]] = i;

            Dictionary<long, bool> contentComparisonCache = new Dictionary<long, bool>();
            List<DuplicateGroupInfo> orderedGroups = new List<DuplicateGroupInfo>();

            foreach (HashSelectorInfo selector in selectors)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Dictionary<string, List<SumResult>> filesByHash = new Dictionary<string, List<SumResult>>(StringComparer.OrdinalIgnoreCase);
                List<List<SumResult>> orderedHashGroups = new List<List<SumResult>>();

                foreach (SumResult item in fileResults)
                {
                    string hash = selector.Selector(item);
                    if (string.IsNullOrWhiteSpace(hash))
                        continue;

                    List<SumResult> group;
                    if (!filesByHash.TryGetValue(hash, out group))
                    {
                        group = new List<SumResult>(2);
                        filesByHash.Add(hash, group);
                        orderedHashGroups.Add(group);
                    }
                    group.Add(item);
                }

                foreach (List<SumResult> candidateGroup in orderedHashGroups)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (candidateGroup.Count < 2)
                        continue;

                    List<List<SumResult>> verifiedGroups = new List<List<SumResult>>();
                    foreach (SumResult item in candidateGroup)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        bool assigned = false;
                        int itemIndex = itemIndexes[item];

                        foreach (List<SumResult> verifiedGroup in verifiedGroups)
                        {
                            int groupIndex = itemIndexes[verifiedGroup[0]];
                            int first = Math.Min(itemIndex, groupIndex);
                            int second = Math.Max(itemIndex, groupIndex);
                            long comparisonKey = ((long)first << 32) | (uint)second;
                            bool identical;

                            if (!contentComparisonCache.TryGetValue(comparisonKey, out identical))
                            {
                                string comparisonError;
                                bool comparisonSucceeded = Utilities.TryAreFilesIdentical(verifiedGroup[0], item, cancellationToken, out identical, out comparisonError);
                                contentComparisonCache.Add(comparisonKey, identical);
                                if (!comparisonSucceeded && operationErrors != null && !string.IsNullOrWhiteSpace(comparisonError))
                                {
                                    string formatted = FormatFileOperationError(Options.TranslationList["operationCompareDuplicates"].ToString(), item.File, new IOException(comparisonError));
                                    if (!operationErrors.Contains(formatted))
                                        operationErrors.Add(formatted);
                                }
                            }

                            if (identical)
                            {
                                verifiedGroup.Add(item);
                                assigned = true;
                                break;
                            }
                        }

                        if (!assigned)
                            verifiedGroups.Add(new List<SumResult> { item });
                    }

                    foreach (List<SumResult> verifiedGroup in verifiedGroups)
                    {
                        if (verifiedGroup.Count < 2)
                            continue;

                        string hash = selector.Selector(verifiedGroup[0]);
                        HashSet<SumResult> verifiedItems = new HashSet<SumResult>(verifiedGroup);
                        List<DuplicateGroupInfo> overlappingGroups = new List<DuplicateGroupInfo>();
                        int insertIndex = orderedGroups.Count;

                        for (int groupIndex = 0; groupIndex < orderedGroups.Count; groupIndex++)
                        {
                            DuplicateGroupInfo existing = orderedGroups[groupIndex];
                            bool overlaps = existing.Items.Any(verifiedItems.Contains);
                            if (!overlaps)
                                continue;

                            if (overlappingGroups.Count == 0)
                                insertIndex = groupIndex;
                            overlappingGroups.Add(existing);
                        }

                        if (overlappingGroups.Count == 0)
                        {
                            orderedGroups.Add(new DuplicateGroupInfo(
                                selector.Algorithm, selector.Name, hash, verifiedGroup.ToArray()));
                            continue;
                        }

                        List<SumResult> mergedItems = new List<SumResult>();
                        HashSet<SumResult> mergedItemSet = new HashSet<SumResult>();
                        foreach (DuplicateGroupInfo existing in overlappingGroups)
                        {
                            foreach (SumResult item in existing.Items)
                            {
                                if (mergedItemSet.Add(item))
                                    mergedItems.Add(item);
                            }
                        }

                        foreach (SumResult item in verifiedGroup)
                        {
                            if (mergedItemSet.Add(item))
                                mergedItems.Add(item);
                        }

                        DuplicateGroupInfo merged = new DuplicateGroupInfo(
                            overlappingGroups[0].Algorithm,
                            overlappingGroups[0].AlgorithmName,
                            overlappingGroups[0].Hash,
                            mergedItems.ToArray());

                        foreach (DuplicateGroupInfo existing in overlappingGroups)
                        {
                            foreach (DuplicateHashMatch match in existing.HashMatches)
                                merged.AddHashMatch(match.Algorithm, match.AlgorithmName, match.Hash);
                            orderedGroups.Remove(existing);
                        }

                        merged.AddHashMatch(selector.Algorithm, selector.Name, hash);
                        orderedGroups.Insert(insertIndex, merged);
                    }
                }
            }

            List<MoonTreeNode> nodes = new List<MoonTreeNode>(orderedGroups.Count);
            string filesWord = Convert.ToString(Options.TranslationList["filesWord"]);
            string groupTitle = Convert.ToString(Options.TranslationList["identicalGroupTitle"]);

            foreach (DuplicateGroupInfo info in orderedGroups)
            {
                cancellationToken.ThrowIfCancellationRequested();
                MoonTreeNode groupNode = new MoonTreeNode(string.Format("{0} ({1} {2})",
                    groupTitle, info.Items.Length, filesWord))
                {
                    Tag = info,
                    ForeColor = Fluent.AccentText
                };

                foreach (DuplicateHashMatch match in info.HashMatches)
                {
                    groupNode.Nodes.Add(new MoonTreeNode(string.Format("{0}: {1}", match.AlgorithmName, match.Hash))
                    {
                        Tag = match,
                        ForeColor = Fluent.TextSecondary
                    });
                }

                foreach (SumResult item in info.Items)
                    groupNode.Nodes.Add(new MoonTreeNode(item.File) { Tag = item });

                groupNode.Expand();
                nodes.Add(groupNode);
            }

            return nodes.ToArray();
        }

        private static List<HashSelectorInfo> GetAvailableHashSelectors(IEnumerable<SumResult> items)
        {
            List<HashSelectorInfo> candidates = new List<HashSelectorInfo>(10)
            {
                new HashSelectorInfo(HashCode.MD5, "MD5", item => item.MD5),
                new HashSelectorInfo(HashCode.SHA1, "SHA1", item => item.SHA1),
                new HashSelectorInfo(HashCode.SHA256, "SHA256", item => item.SHA256),
                new HashSelectorInfo(HashCode.SHA384, "SHA384", item => item.SHA384),
                new HashSelectorInfo(HashCode.SHA512, "SHA512", item => item.SHA512),
                new HashSelectorInfo(HashCode.RIPEMD160, "RIPEMD160", item => item.RIPEMD160),
                new HashSelectorInfo(HashCode.CRC32, "CRC32", item => item.CRC32),
                new HashSelectorInfo(HashCode.SHA3_256, "SHA3-256", item => item.SHA3_256),
                new HashSelectorInfo(HashCode.SHA3_384, "SHA3-384", item => item.SHA3_384),
                new HashSelectorInfo(HashCode.SHA3_512, "SHA3-512", item => item.SHA3_512)
            };

            List<SumResult> results = items as List<SumResult> ?? new List<SumResult>(items);
            candidates.RemoveAll(selector => !results.Any(item =>
                item != null && !string.IsNullOrWhiteSpace(selector.Selector(item))));
            return candidates;
        }

        private static SumResult GetNodeSumResult(MoonTreeNode node)
        {
            if (node == null)
                return null;

            FileNodeInfo fileInfo = node.Tag as FileNodeInfo;
            return fileInfo == null ? node.Tag as SumResult : fileInfo.Result;
        }

        private void HelperMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MoonTreeNode selectedNode = SumView.SelectedNode;
            removeToolStripMenuItem.Enabled = selectedNode != null && !(selectedNode.Tag is DuplicateGroupInfo) && !(selectedNode.Tag is DuplicateHashMatch);
        }

        private sealed class HashSelectorInfo
        {
            internal readonly HashCode Algorithm;
            internal readonly string Name;
            internal readonly Func<SumResult, string> Selector;

            internal HashSelectorInfo(HashCode algorithm, string name, Func<SumResult, string> selector)
            {
                Algorithm = algorithm;
                Name = name;
                Selector = selector;
            }
        }

        private sealed class FileNodeInfo
        {
            internal readonly SumResult Result;
            internal readonly string ThemeFlag;

            internal FileNodeInfo(SumResult result, string themeFlag)
            {
                Result = result;
                ThemeFlag = themeFlag;
            }
        }

        private sealed class FileEnumerationResult
        {
            internal readonly List<string> Files = new List<string>();
            internal readonly List<string> Errors = new List<string>();
            internal int ErrorCount;
            internal int SkippedReparseDirectories;

            internal void AddError(string path, Exception exception)
            {
                ErrorCount++;
                if (Errors.Count < 10)
                    Errors.Add(MainForm.FormatFileOperationError(Options.TranslationList["operationScanAccess"].ToString(), path, exception));
            }
        }

        private void ReCalculateSums()
        {
            if (!Options.HasActiveHash(Options.CurrentOptions.HashOptions))
            {
                Clear();
                return;
            }

            if (_fileList != null)
            {
                if (_fileList.Count > 0)
                {
                    string[] currentList = SumResult.Sums.Select(x => x.File).ToArray();
                    SumResult.Sums.Clear();

                    DetectFiles(currentList);
                }
            }
        }

        private static string FormatFileOperationError(string operation, string path, Exception exception)
        {
            string error = exception == null ? string.Empty : exception.Message;
            return Options.TranslationList["fileOperationFailed"].ToString()
                .Replace("{OPERATION}", operation ?? string.Empty)
                .Replace("{PATH}", path ?? string.Empty)
                .Replace("{ERROR}", error);
        }

        private void ShowFileOperationErrors(IList<string> errors)
        {
            if (errors == null || errors.Count == 0)
                return;

            StringBuilder details = new StringBuilder();
            int count = Math.Min(errors.Count, 10);
            for (int i = 0; i < count; i++)
                details.Append(errors[i]).Append(Environment.NewLine).Append(Environment.NewLine);
            if (errors.Count > count)
                details.Append(Options.TranslationList["fileOperationErrorsMore"].ToString()
                    .Replace("{COUNT}", (errors.Count - count).ToString(CultureInfo.InvariantCulture)));

            FluentMessageBox.Show(this, details.ToString().Trim(), Options.TranslationList["fileOperationErrorsTitle"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string ListIncompatibles(List<string> files)
        {
            StringBuilder result = new StringBuilder();

            foreach (string s in files)
            {
                result.Append(s).Append(Environment.NewLine);
            }

            return result.ToString();
        }

        private static List<T> DeserializeJsonList<T>(string file)
        {
            using (StreamReader textReader = File.OpenText(file))
            using (JsonTextReader jsonReader = new JsonTextReader(textReader))
                return JsonSerializer.CreateDefault().Deserialize<List<T>>(jsonReader);
        }

        private List<FileSummary> AnalyzeJsonFiles()
        {
            List<FileSummary> fileSummaries = new List<FileSummary>();
            List<string> incompatibles = new List<string>();

            foreach (string s in _fileList)
            {
                try
                {
                    fileSummaries.AddRange(DeserializeJsonList<FileSummary>(s));
                }
                catch (Exception ex)
                {
                    incompatibles.Add(FormatFileOperationError(Options.TranslationList["operationReadJson"].ToString(), s, ex));
                    continue;
                }
            }

            _fileList.RemoveAll(x => string.Equals(Path.GetExtension(x), ".json", StringComparison.OrdinalIgnoreCase));

            if (incompatibles.Count > 0)
            {
                FluentMessageBox.ShowTopMost(this, Options.TranslationList["unsupportedJSON1"].ToString() + ListIncompatibles(incompatibles) + Options.TranslationList["unsupportedJSON2"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (FileSummary json in fileSummaries)
            {
                if (json != null && !string.IsNullOrWhiteSpace(json.File))
                    _fileList.Add(json.File);
            }

            return fileSummaries;
        }

        private async void DetectFiles(string[] files, bool calculateImmediately = false)
        {
            if (files == null || files.Length == 0 || tokenSource != null || _processingInput)
                return;

            _processingInput = true;
            CancellationTokenSource scanSource = new CancellationTokenSource();
            tokenSource = scanSource;

            bool sourceReleased = false;
            try
            {
                SetEnumerationUi(true);
                FileEnumerationResult enumeration = await Task.Run(
                    () => EnumerateInputFiles(files, scanSource.Token), scanSource.Token);
                scanSource.Token.ThrowIfCancellationRequested();

                _fileList = enumeration.Files;
                tokenSource = null;
                scanSource.Dispose();
                sourceReleased = true;
                SetEnumerationUi(false);

                if (enumeration.ErrorCount > 0 || enumeration.SkippedReparseDirectories > 0)
                {
                    StringBuilder details = new StringBuilder();
                    details.Append(Options.TranslationList["scanWarnings"].ToString());
                    foreach (string error in enumeration.Errors)
                        details.Append(error).Append(Environment.NewLine);
                    if (enumeration.ErrorCount > enumeration.Errors.Count)
                        details.Append(Options.TranslationList["fileOperationErrorsMore"].ToString()
                            .Replace("{COUNT}", (enumeration.ErrorCount - enumeration.Errors.Count).ToString(CultureInfo.InvariantCulture)));
                    if (enumeration.SkippedReparseDirectories > 0)
                    {
                        if (details.Length > 0 && details[details.Length - 1] != '\n')
                            details.AppendLine();
                        details.AppendFormat(Options.TranslationList["scanSkippedReparseDirectories"].ToString(), enumeration.SkippedReparseDirectories);
                    }
                    FluentMessageBox.Show(this, details.ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (_fileList.Count == 0)
                    return;

                _allFilesAreJson = true;
                foreach (string file in _fileList)
                {
                    if (!string.Equals(Path.GetExtension(file), ".json", StringComparison.OrdinalIgnoreCase))
                    {
                        _allFilesAreJson = false;
                        break;
                    }
                }

                if (_allFilesAreJson && !calculateImmediately)
                {
                    using (FeatureBox featureBox = new FeatureBox())
                        featureBox.ShowDialog(this);

                    if (FeatureBox.SelectedAction == FeatureAction.Calculate)
                    {
                        CalculateSums();
                    }
                    else if (FeatureBox.SelectedAction == FeatureAction.AnalyzeJSON)
                    {
                        Clear();
                        _fileSummaries = AnalyzeJsonFiles();
                        _fileSummaries.RemoveAll(item => item == null || string.IsNullOrWhiteSpace(item.File));

                        if (_fileSummaries.Count > 0)
                            CalculateSums(true);
                        else
                            FluentMessageBox.ShowTopMost(this, Options.TranslationList["invalidPath"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (FeatureBox.SelectedAction == FeatureAction.ListJSON)
                    {
                        List<SumResult> listing = new List<SumResult>();
                        List<string> listingErrors = new List<string>();
                        foreach (string file in _fileList)
                        {
                            try
                            {
                                List<SumResult> loaded = DeserializeJsonList<SumResult>(file);
                                if (loaded != null)
                                    listing.AddRange(loaded);
                            }
                            catch (Exception ex)
                            {
                                listingErrors.Add(FormatFileOperationError(Options.TranslationList["operationReadJsonResults"].ToString(), file, ex));
                            }
                        }

                        listing.RemoveAll(item => item == null || string.IsNullOrWhiteSpace(item.File));
                        if (listing.Count > 0)
                        {
                            List<MoonTreeNode> roots = new List<MoonTreeNode>(listing.Count);
                            for (int i = 0; i < listing.Count; i++)
                            {
                                SumResult item = listing[i];
                                MoonTreeNode rootNode = new MoonTreeNode(item.File)
                                {
                                    ForeColor = Fluent.AccentText,
                                    Tag = new FileNodeInfo(item, Options.ThemeFlag)
                                };

                                if (!string.IsNullOrEmpty(item.MD5)) rootNode.Nodes.Add("MD5: " + item.MD5);
                                if (!string.IsNullOrEmpty(item.SHA1)) rootNode.Nodes.Add("SHA1: " + item.SHA1);
                                if (!string.IsNullOrEmpty(item.SHA256)) rootNode.Nodes.Add("SHA256: " + item.SHA256);
                                if (!string.IsNullOrEmpty(item.SHA384)) rootNode.Nodes.Add("SHA384: " + item.SHA384);
                                if (!string.IsNullOrEmpty(item.SHA512)) rootNode.Nodes.Add("SHA512: " + item.SHA512);
                                if (!string.IsNullOrEmpty(item.CRC32)) rootNode.Nodes.Add("CRC32: " + item.CRC32);
                                if (!string.IsNullOrEmpty(item.RIPEMD160)) rootNode.Nodes.Add("RIPEMD160: " + item.RIPEMD160);
                                if (!string.IsNullOrEmpty(item.SHA3_256)) rootNode.Nodes.Add("SHA3-256: " + item.SHA3_256);
                                if (!string.IsNullOrEmpty(item.SHA3_384)) rootNode.Nodes.Add("SHA3-384: " + item.SHA3_384);
                                if (!string.IsNullOrEmpty(item.SHA3_512)) rootNode.Nodes.Add("SHA3-512: " + item.SHA3_512);
                                roots.Add(rootNode);
                            }

                            List<MoonTreeNode> allNodes = _allSums == null
                                ? new List<MoonTreeNode>()
                                : new List<MoonTreeNode>(_allSums);
                            allNodes.AddRange(roots);

                            CancellationTokenSource duplicateSource = new CancellationTokenSource();
                            tokenSource = duplicateSource;
                            try
                            {
                                SetEnumerationUi(true);
                                lblCalculating.Text = Options.TranslationList["lblCalculatingNow"].ToString();

                                MoonTreeNode[] identicalNodes = await Task.Run(
                                    () => BuildIdenticalNodes(listing, duplicateSource.Token, listingErrors), duplicateSource.Token);
                                duplicateSource.Token.ThrowIfCancellationRequested();

                                SumResult.Sums.AddRange(listing);
                                _allSums = allNodes.ToArray();
                                _identicalSums = identicalNodes;
                                _groupedSums = null;

                                SumView.BeginUpdate();
                                try
                                {
                                    SumView.Nodes.Clear();
                                    SumView.Nodes.AddRange(_allSums);
                                    SumView.ExpandAll();
                                }
                                finally
                                {
                                    SumView.EndUpdate();
                                }

                                FilterTable();
                            }
                            finally
                            {
                                if (ReferenceEquals(tokenSource, duplicateSource))
                                    tokenSource = null;
                                duplicateSource.Dispose();
                                SetEnumerationUi(false);
                            }
                        }
                        ShowFileOperationErrors(listingErrors);
                    }
                }
                else
                {
                    CalculateSums();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                FluentMessageBox.Show(this, ex.Message, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (!sourceReleased)
                {
                    if (ReferenceEquals(tokenSource, scanSource))
                        tokenSource = null;
                    scanSource.Dispose();
                    SetEnumerationUi(false);
                }

                _processingInput = false;
                if (_closeAfterHashing && tokenSource == null)
                {
                    _closeAfterHashing = false;
                    Close();
                }
                else if (tokenSource == null)
                {
                    ProcessPendingShellRequests();
                }
            }
        }

        private void SetEnumerationUi(bool scanning)
        {
            btnUpdate.Enabled = !scanning;
            btnOptions.Enabled = !scanning;
            btnSaveJson.Enabled = !scanning;
            btnBrowse.Enabled = !scanning;
            txtPath.Enabled = !scanning;
            SumView.Enabled = !scanning;
            AllowDrop = !scanning;
            btnCancelHashing.Visible = scanning;
            btnCancelHashing.Enabled = true;
            btnCancelHashing.Text = Options.TranslationList["btnCancelHashing"].ToString();

            if (scanning)
            {
                lblCalculating.Text = Options.TranslationList["lblScanningFiles"].ToString();
                lblCalculating.Visible = true;
            }
            else
            {
                lblCalculating.Text = Options.TranslationList["lblCalculating"].ToString();
                lblCalculating.Visible = SumView.Nodes.Count == 0;
            }
        }

        private static FileEnumerationResult EnumerateInputFiles(string[] inputs, CancellationToken cancellationToken)
        {
            FileEnumerationResult result = new FileEnumerationResult();
            Stack<string> pendingDirectories = new Stack<string>();
            HashSet<string> visitedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string input in inputs)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                try
                {
                    FileAttributes attributes = File.GetAttributes(input);
                    if ((attributes & FileAttributes.Directory) == 0)
                    {
                        result.Files.Add(input);
                        continue;
                    }

                    if ((attributes & FileAttributes.ReparsePoint) != 0)
                    {
                        result.SkippedReparseDirectories++;
                        continue;
                    }

                    string fullPath = Path.GetFullPath(input);
                    if (visitedDirectories.Add(fullPath))
                        pendingDirectories.Push(fullPath);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    result.AddError(input, ex);
                }
            }

            while (pendingDirectories.Count > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string directory = pendingDirectories.Pop();

                try
                {
                    foreach (string file in Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        result.Files.Add(file);
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    result.AddError(directory, ex);
                }

                try
                {
                    foreach (string subDirectory in Directory.EnumerateDirectories(directory, "*", SearchOption.TopDirectoryOnly))
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        try
                        {
                            FileAttributes attributes = File.GetAttributes(subDirectory);
                            if ((attributes & FileAttributes.ReparsePoint) != 0)
                            {
                                result.SkippedReparseDirectories++;
                                continue;
                            }

                            string fullPath = Path.GetFullPath(subDirectory);
                            if (visitedDirectories.Add(fullPath))
                                pendingDirectories.Push(fullPath);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch (Exception ex)
                        {
                            result.AddError(subDirectory, ex);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    result.AddError(directory, ex);
                }
            }

            return result;
        }

        private void CalculateSums(bool analyzeJson = false)
        {
            if (!analyzeJson && !Options.HasActiveHash(Options.CurrentOptions.HashOptions))
            {
                FluentMessageBox.Show(this, Options.TranslationList["noActiveHash"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Text = "Hashing";

            tokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = tokenSource.Token;
            List<string> hashErrors = new List<string>();
            HashOptions activeHashOptions = CloneHashOptions(Options.CurrentOptions.HashOptions);
            Dictionary<string, List<FileSummary>> expectedSummariesByFile = analyzeJson
                ? BuildExpectedSummariesByFile(_fileSummaries)
                : new Dictionary<string, List<FileSummary>>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, HashOptions> hashOptionsByFile = analyzeJson
                ? BuildVerificationHashOptions(expectedSummariesByFile, activeHashOptions)
                : new Dictionary<string, HashOptions>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> verifiedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            MoonTreeNode[] computedIdenticalNodes = new MoonTreeNode[0];

            Task.Factory.StartNew(() =>
            {
                lblCalculating.Invoke((MethodInvoker)delegate
                {
                    btnUpdate.Enabled = false;
                    btnOptions.Enabled = false;
                    btnSaveJson.Enabled = false;
                    btnBrowse.Enabled = false;
                    txtPath.Enabled = false;
                    SumView.Enabled = false;
                    AllowDrop = false;
                    lblCalculating.Text = Options.TranslationList["lblCalculatingNow"].ToString();
                    lblCalculating.Visible = true;
                    btnCancelHashing.Visible = true;
                });

                _fileCounter = 0;
                _timer.Reset();
                _timer.Start();
                _totalSize = 0;
                Stopwatch progressTimer = Stopwatch.StartNew();

                for (int fileIndex = 0; fileIndex < _fileList.Count; fileIndex++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string file = _fileList[fileIndex];
                    _fileCounter++;
                    if (progressTimer.ElapsedMilliseconds >= 100 || fileIndex == 0)
                    {
                        string fileName = Path.GetFileName(file);
                        string fileExtension = Path.GetExtension(file);
                        if (fileName.Length > 28)
                            fileName = fileName.Substring(0, 27) + " ... " + fileExtension;

                        int currentFile = _fileCounter;
                        string progressText = string.Format("{0}/{1} - '{2}'", currentFile, _fileList.Count, fileName);
                        lblCalculating.Invoke((MethodInvoker)delegate
                        {
                            lblCalculating.Text = progressText;
                            lblCalculating.Visible = true;
                            btnCancelHashing.Visible = true;
                        });
                        progressTimer.Restart();
                    }

                    try
                    {
                        _totalSize += new FileInfo(file).Length;

                        HashOptions fileHashOptions;
                        string normalizedFile = analyzeJson ? NormalizeVerificationPath(file) : file;
                        if (!analyzeJson || !hashOptionsByFile.TryGetValue(normalizedFile, out fileHashOptions))
                            fileHashOptions = activeHashOptions;

                        SumResult result = Utilities.CalculateSums(file, cancellationToken, fileHashOptions);
                        if (result != null)
                        {
                            if (analyzeJson)
                            {
                                List<FileSummary> expectedSummaries;
                                if (expectedSummariesByFile.TryGetValue(normalizedFile, out expectedSummaries) &&
                                    VerifyExpectedSummaries(result, expectedSummaries))
                                    verifiedFiles.Add(result.File);

                                ClearUnselectedHashes(result, activeHashOptions);
                            }

                            SumResult.Sums.Add(result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        hashErrors.Add(FormatFileOperationError(Options.TranslationList["operationReadHash"].ToString(), file, ex));
                    }
                }

                _timer.Stop();
                cancellationToken.ThrowIfCancellationRequested();
                computedIdenticalNodes = BuildIdenticalNodes(SumResult.Sums.ToArray(), cancellationToken, hashErrors);
            }, cancellationToken).ContinueWith(previousTask =>
            {
                _timer.Stop();

                if (previousTask.IsFaulted && previousTask.Exception != null)
                    hashErrors.Add(previousTask.Exception.GetBaseException().Message);

                bool wasCancelled = tokenSource != null && tokenSource.IsCancellationRequested;
                _analyzedFiles = analyzeJson ? verifiedFiles.ToList() : null;

                SumView.BeginUpdate();
                try
                {
                    SumView.Nodes.Clear();
                    _allSums = null;

                    if (SumResult.Sums != null && SumResult.Sums.Count > 0)
                    {
                        List<MoonTreeNode> nodes = new List<MoonTreeNode>(SumResult.Sums.Count);
                        if (analyzeJson)
                        {
                            for (int i = 0; i < SumResult.Sums.Count; i++)
                            {
                                SumResult sum = SumResult.Sums[i];
                                MoonTreeNode rootNode = new MoonTreeNode(sum.File)
                                {
                                    ForeColor = Fluent.AccentText,
                                    Tag = new FileNodeInfo(sum, Options.ThemeFlag)
                                };

                                if (Options.CurrentOptions.HashOptions.MD5)
                                {
                                    rootNode.Nodes.Add("MD5: " + sum.MD5);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA1)
                                {
                                    rootNode.Nodes.Add("SHA1: " + sum.SHA1);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA256)
                                {
                                    rootNode.Nodes.Add("SHA256: " + sum.SHA256);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA384)
                                {
                                    rootNode.Nodes.Add("SHA384: " + sum.SHA384);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA512)
                                {
                                    rootNode.Nodes.Add("SHA512: " + sum.SHA512);
                                }

                                if (Options.CurrentOptions.HashOptions.CRC32)
                                {
                                    rootNode.Nodes.Add("CRC32: " + sum.CRC32);
                                }

                                if (Options.CurrentOptions.HashOptions.RIPEMD160)
                                {
                                    rootNode.Nodes.Add("RIPEMD160: " + sum.RIPEMD160);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA3_256)
                                {
                                    rootNode.Nodes.Add("SHA3-256: " + sum.SHA3_256);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA3_384)
                                {
                                    rootNode.Nodes.Add("SHA3-384: " + sum.SHA3_384);
                                }

                                if (Options.CurrentOptions.HashOptions.SHA3_512)
                                {
                                    rootNode.Nodes.Add("SHA3-512: " + sum.SHA3_512);
                                }

                                nodes.Add(rootNode);
                            }
                        }
                        else
                        {
                            foreach (SumResult sum in SumResult.Sums)
                            {
                                MoonTreeNode rootNode = new MoonTreeNode(sum.File)
                                {
                                    ForeColor = Fluent.AccentText,
                                    Tag = new FileNodeInfo(sum, Options.ThemeFlag)
                                };

                                if (Options.CurrentOptions.HashOptions.MD5) rootNode.Nodes.Add("MD5: " + sum.MD5);
                                if (Options.CurrentOptions.HashOptions.SHA1) rootNode.Nodes.Add("SHA1: " + sum.SHA1);
                                if (Options.CurrentOptions.HashOptions.SHA256) rootNode.Nodes.Add("SHA256: " + sum.SHA256);
                                if (Options.CurrentOptions.HashOptions.SHA384) rootNode.Nodes.Add("SHA384: " + sum.SHA384);
                                if (Options.CurrentOptions.HashOptions.SHA512) rootNode.Nodes.Add("SHA512: " + sum.SHA512);
                                if (Options.CurrentOptions.HashOptions.CRC32) rootNode.Nodes.Add("CRC32: " + sum.CRC32);
                                if (Options.CurrentOptions.HashOptions.RIPEMD160) rootNode.Nodes.Add("RIPEMD160: " + sum.RIPEMD160);
                                if (Options.CurrentOptions.HashOptions.SHA3_256) rootNode.Nodes.Add("SHA3-256: " + sum.SHA3_256);
                                if (Options.CurrentOptions.HashOptions.SHA3_384) rootNode.Nodes.Add("SHA3-384: " + sum.SHA3_384);
                                if (Options.CurrentOptions.HashOptions.SHA3_512) rootNode.Nodes.Add("SHA3-512: " + sum.SHA3_512);

                                nodes.Add(rootNode);
                            }
                        }

                        SumView.Nodes.AddRange(nodes.ToArray());
                        SumView.ExpandAll();
                        _allSums = SumView.Nodes.Cast<MoonTreeNode>().ToArray();
                    }
                    _identicalSums = wasCancelled ? new MoonTreeNode[0] : computedIdenticalNodes;
                    _groupedSums = null;
                }
                finally
                {
                    SumView.EndUpdate();
                }

                FilterTable();

                lblCalculating.Text = Options.TranslationList["lblCalculatingNow"].ToString();
                lblCalculating.Visible = false;
                btnCancelHashing.Visible = false;
                btnCancelHashing.Enabled = true;
                btnCancelHashing.Text = Options.TranslationList["btnCancelHashing"].ToString();
                btnOptions.Enabled = true;
                btnUpdate.Enabled = true;
                btnSaveJson.Enabled = true;
                btnBrowse.Enabled = true;
                txtPath.Enabled = true;
                SumView.Enabled = true;
                AllowDrop = true;

                if (SumView.Nodes.Count > 0)
                {
                    SumView.Nodes[0].EnsureVisible();
                    Text = string.Format("Hashing - {0}m : {1}s / {2} {3} ({4})", _timer.Elapsed.Minutes, _timer.Elapsed.Seconds, _fileCounter, Options.TranslationList["filesWord"].ToString(), ByteSize.FromBytes(_totalSize));
                }
                else
                {
                    Text = "Hashing";
                }

                if (!wasCancelled && _tempIndex > -1 && _tempIndex < SumResult.Sums.Count)
                {
                    Utilities.SearchVirusTotal(SumResult.Sums[_tempIndex].SHA256);
                    _tempIndex = -1;
                }

                if (analyzeJson && !wasCancelled && !_closeAfterHashing)
                {
                    if (_analyzedFiles != null && _analyzedFiles.Count > 0)
                    {
                        using (AnalyzedForm form = new AnalyzedForm(_analyzedFiles.Distinct()))
                            form.ShowDialog(this);
                    }
                    else if (hashErrors.Count == 0)
                    {
                        FluentMessageBox.ShowTopMost(this, Options.TranslationList["corruptedJSON"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (hashErrors.Count > 0 && !_closeAfterHashing)
                {
                    string details = string.Join(Environment.NewLine, hashErrors.Take(10));
                    if (hashErrors.Count > 10)
                        details += Environment.NewLine + Options.TranslationList["fileOperationErrorsMore"].ToString()
                            .Replace("{COUNT}", (hashErrors.Count - 10).ToString(CultureInfo.InvariantCulture));

                    FluentMessageBox.Show(this, details, Options.TranslationList["fileOperationErrorsTitle"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                CancellationTokenSource completedSource = tokenSource;
                tokenSource = null;
                if (completedSource != null)
                    completedSource.Dispose();

                if (_closeAfterHashing)
                {
                    _closeAfterHashing = false;
                    Close();
                }
                else
                {
                    ProcessPendingShellRequests();
                }
            }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        internal void FixColor()
        {
            foreach (MoonTreeNode node in SumView.Nodes)
            {
                FileNodeInfo fileInfo = node.Tag as FileNodeInfo;
                if (fileInfo != null && string.Equals(fileInfo.ThemeFlag, Options.ThemeFlag, StringComparison.Ordinal))
                    node.ForeColor = Fluent.AccentText;
            }

            CheckMenuItemsByHashes();
            Fluent.StyleMenu(helperMenu);
            Fluent.StyleMenu(trayMenu);
            Fluent.StyleMenu(_toolsMenu);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!AllowDrop)
                return;

            DetectFiles((string[])e.Data.GetData(DataFormats.FileDrop, false));
        }

        private void RestoreWindowState()
        {
            Screen[] screens = Screen.AllScreens;
            Screen primary = Screen.PrimaryScreen ?? screens[0];
            Rectangle primaryArea = primary.WorkingArea;

            Size requestedSize = Options.CurrentOptions.WindowSize;
            if (requestedSize.Width < 320 || requestedSize.Height < 240)
                requestedSize = new Size(960, 700);
            Rectangle requestedBounds;
            if (Options.CurrentOptions.WindowLocation.HasValue)
                requestedBounds = new Rectangle(Options.CurrentOptions.WindowLocation.Value, requestedSize);
            else
                requestedBounds = CenterBounds(primaryArea, requestedSize);

            Screen target = null;
            foreach (Screen screen in screens)
            {
                if (IsWindowVisibleOnScreen(requestedBounds, screen.WorkingArea))
                {
                    target = screen;
                    break;
                }
            }

            if (target == null)
            {
                target = primary;
                requestedBounds = CenterBounds(target.WorkingArea, requestedSize);
            }
            else
            {
                requestedBounds = FitBounds(requestedBounds, target.WorkingArea);
            }

            StartPosition = FormStartPosition.Manual;
            Bounds = requestedBounds;
            WindowState = Options.CurrentOptions.WindowState == FormWindowState.Maximized
                ? FormWindowState.Maximized
                : FormWindowState.Normal;
        }

        private static bool IsWindowVisibleOnScreen(Rectangle bounds, Rectangle workingArea)
        {
            Rectangle intersection = Rectangle.Intersect(bounds, workingArea);
            int minimumWidth = Math.Min(160, bounds.Width);
            int minimumHeight = Math.Min(72, bounds.Height);
            return intersection.Width >= minimumWidth && intersection.Height >= minimumHeight;
        }

        private static Rectangle CenterBounds(Rectangle workingArea, Size size)
        {
            size.Width = Math.Min(size.Width, workingArea.Width);
            size.Height = Math.Min(size.Height, workingArea.Height);
            return new Rectangle(
                workingArea.Left + Math.Max(0, (workingArea.Width - size.Width) / 2),
                workingArea.Top + Math.Max(0, (workingArea.Height - size.Height) / 2),
                size.Width,
                size.Height);
        }

        private static Rectangle FitBounds(Rectangle bounds, Rectangle workingArea)
        {
            int width = Math.Min(bounds.Width, workingArea.Width);
            int height = Math.Min(bounds.Height, workingArea.Height);
            int left = Math.Min(Math.Max(bounds.Left, workingArea.Left), workingArea.Right - width);
            int top = Math.Min(Math.Max(bounds.Top, workingArea.Top), workingArea.Bottom - height);
            return new Rectangle(left, top, width, height);
        }

        private void SaveWindowState()
        {
            Options.CurrentOptions.WindowState = this.WindowState;

            if (this.WindowState == FormWindowState.Normal)
            {
                Options.CurrentOptions.WindowLocation = this.Location;
                Options.CurrentOptions.WindowSize = this.Size;
            }
            else
            {
                Options.CurrentOptions.WindowLocation = this.RestoreBounds.Location;
                Options.CurrentOptions.WindowSize = this.RestoreBounds.Size;
            }
        }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x001A && Options.CurrentOptions.Appearance == AppearanceMode.System)
            {
                Options.InitializePalette();
                Options.ApplyTheme(this);
                FixColor();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LayoutModernShell();
            SetSearchWatermark();
        }

        private void topPanel_Resize(object sender, EventArgs e)
        {
            LayoutModernShell();
        }

        private void BuildModernLayout()
        {
            topPanel.BorderStyle = BorderStyle.None;
            topPanel.Height = Fluent.Scale(topPanel, 68);
            topPanel.Padding = Padding.Empty;
            botPanel.BorderStyle = BorderStyle.None;
            botPanel.Padding = new Padding(16, 10, 16, 16);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            label24.Font = Fluent.UiSemibold(16f);
            lblversion.Font = Fluent.Ui(9f);
            lblCalculating.Font = Fluent.Ui(12f);
            lblCalculating.TextAlign = ContentAlignment.MiddleCenter;
            lblCalculating.BackColor = Fluent.Surface;
            lblCalculating.HintText = Options.TranslationList["lblCalculating"].ToString();
            lblCalculating.Subtitle = Options.TranslationList["dropHintSubtitle"].ToString();

            txtPath.Font = Fluent.Ui(10f);
            txtPath.BorderStyle = BorderStyle.None;
            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            btnBrowse.Tag = "primary";
            btnBrowse.Text = Options.TranslationList["btnBrowse"].ToString();
            btnBrowse.Glyph = Fluent.Glyphs.Add;
            btnCancelHashing.Tag = "primary";
            btnCancelHashing.Glyph = Fluent.Glyphs.Cancel;

            _btnTools.Font = Fluent.Ui(9f);
            btnSaveJson.Font = Fluent.Ui(9f);
            btnSaveJson.Text = Options.TranslationList["btnSaveJson"].ToString();
            btnSaveJson.Glyph = Fluent.Glyphs.Download;
            btnSaveJson.Variant = ButtonVariant.Primary;
            btnSaveJson.DialogResult = DialogResult.None;
            btnUpdate.Font = Fluent.Ui(9f);
            btnUpdate.Glyph = Fluent.Glyphs.Refresh;
            btnOptions.Font = Fluent.Ui(9f);
            btnOptions.Glyph = Fluent.Glyphs.Settings;
            btnBrowse.Font = Fluent.UiSemibold(9f);

            SumView.BorderStyle = BorderStyle.None;
            SumView.Dock = DockStyle.Fill;
            lblCalculating.Dock = DockStyle.Fill;
            SumView.SendToBack();
            lblCalculating.BringToFront();
            btnCancelHashing.BringToFront();

            LayoutModernShell();
        }

        private void WrapPathInput()
        {
            Control parent = txtPath.Parent;
            if (parent == null) return;

            Rectangle bounds = txtPath.Bounds;
            AnchorStyles anchor = txtPath.Anchor;
            parent.Controls.Remove(txtPath);

            _pathInput = new InputBox
            {
                Name = "pathInput",
                Location = bounds.Location,
                Size = bounds.Size,
                Anchor = anchor
            };
            _pathInput.SetLeading(new IconLabel
            {
                Glyph = Fluent.Glyphs.Search,
                Fallback = "⌕",
                IconSize = 12f,
                GlyphColor = Fluent.TextSecondary
            });
            _pathInput.SetEditor(txtPath);

            parent.Controls.Remove(btnBrowse);
            btnBrowse.Anchor = AnchorStyles.None;
            btnBrowse.Margin = Padding.Empty;
            _pathInput.AddTrailing(btnBrowse);

            txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            parent.Controls.Add(_pathInput);
            _pathInput.BringToFront();
            LayoutModernShell();
            pictureBox2.Visible = false;
        }

        private void LayoutModernShell()
        {
            if (topPanel == null || topPanel.IsDisposed)
                return;

            int width = topPanel.ClientSize.Width;
            int rightMargin = Fluent.Scale(this, 18);
            int gap = Fluent.Scale(this, 6);
            int y = Fluent.Scale(this, 16);
            int height = Fluent.Scale(this, 36);

            int totalWidth = gap * (_modernButtons.Length - 1);
            for (int i = 0; i < _modernButtonWidths.Length; i++)
                totalWidth += Fluent.Scale(this, _modernButtonWidths[i]);

            int actionX = Math.Max(0, width - rightMargin - totalWidth);
            int x = actionX;
            for (int i = 0; i < _modernButtons.Length; i++)
            {
                int buttonWidth = Fluent.Scale(this, _modernButtonWidths[i]);
                _modernButtons[i].SetBounds(x, y, buttonWidth, height);
                _modernButtons[i].Anchor = AnchorStyles.Top | AnchorStyles.Right;
                x += buttonWidth + gap;
            }

            pictureBox1.SetBounds(Fluent.Scale(this, 18), Fluent.Scale(this, 12),
                Fluent.Scale(this, 42), Fluent.Scale(this, 42));
            label24.SetBounds(Fluent.Scale(this, 72), Fluent.Scale(this, 8),
                Fluent.Scale(this, 135), Fluent.Scale(this, 26));
            lblversion.SetBounds(Fluent.Scale(this, 74), Fluent.Scale(this, 36),
                Fluent.Scale(this, 140), Fluent.Scale(this, 18));

            pictureBox2.Visible = false;

            int searchLeft = Fluent.Scale(this, 220);
            int searchRight = actionX - gap;
            int searchWidth = Math.Max(Fluent.Scale(this, 150), searchRight - searchLeft);
            if (_pathInput != null)
            {
                btnBrowse.Width = Fluent.Scale(_pathInput, 112);
                _pathInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                _pathInput.SetBounds(searchLeft, y, searchWidth, height);
            }
            else
            {
                int browseWidth = Fluent.Scale(this, 112);
                int browseX = searchLeft + searchWidth - browseWidth;
                btnBrowse.SetBounds(browseX, y, browseWidth, height);
                btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                txtPath.SetBounds(searchLeft, y + Fluent.Scale(this, 2),
                    Math.Max(Fluent.Scale(this, 40), searchWidth - browseWidth - gap), Fluent.Scale(this, 32));
                txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        private void SetSearchWatermark()
        {
            string watermark = Options.TranslationList == null
                ? "Filter files by name, path or hash"
                : Options.TranslationList["filterResults"].ToString();

            Fluent.SetPlaceholder(txtPath, watermark);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnSaveJson_Click(object sender, EventArgs e)
        {
            ShowExportMenu();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (AboutForm form = new AboutForm())
                form.ShowDialog(this);
        }

        private void sumView_NodeMouseClick(object sender, MoonTreeNodeMouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                SumView.SelectedNode = e.Node;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }


        private void SaveSettingsSafely()
        {
            string error;
            if (Options.TrySaveSettings(out error))
            {
                _settingsSaveWarningShown = false;
                return;
            }

            if (_settingsSaveWarningShown)
                return;

            _settingsSaveWarningShown = true;
            FluentMessageBox.Show(this,
                Options.TranslationList["settingsSaveFailed"].ToString().Replace("{ERROR}", error),
                Options.TranslationList["fileOperationErrorsTitle"].ToString(),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tokenSource != null)
            {
                _closeAfterHashing = true;
                tokenSource.Cancel();
                e.Cancel = true;
                return;
            }

            if (_allowExit)
            {
                SaveWindowState();
                SaveSettingsSafely();
            }
            else
            {
                e.Cancel = true;
                trayIcon.Visible = true;
                this.Hide();
            }
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            using (OptionsForm f = new OptionsForm(this))
                f.ShowDialog(this);

            if (Options.CurrentOptions.StayOnTop)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }

            CheckMenuItemsByHashes();

            _allowExit = !Options.CurrentOptions.TrayIcon;

            // change only CRC32 format
            if (!OptionsForm.HashesChanged && OptionsForm.CRC32FormatChanged && Options.CurrentOptions.HashOptions.CRC32)
            {
                if (Options.CurrentOptions.CRC32Decimal)
                {
                    foreach (SumResult x in SumResult.Sums)
                    {
                        x.ConvertCRC32ToDecimal();
                    }
                }
                else
                {
                    foreach (SumResult x in SumResult.Sums)
                    {
                        x.ConvertCRC32ToHexadecimal();
                    }
                }

                RefreshSumList();
            }

            // change only character casing
            if (!OptionsForm.HashesChanged && OptionsForm.CasingChanged)
            {
                if (Options.CurrentOptions.LowerCasing)
                {
                    foreach (SumResult x in SumResult.Sums)
                    {
                        x.ConvertToLowerCasing();
                    }
                }
                else
                {
                    foreach (SumResult x in SumResult.Sums)
                    {
                        x.ConvertToUpperCasing();
                    }
                }

                RefreshSumList();
            }

            // re-calculate everything
            if (OptionsForm.HashesChanged && SumResult.Sums.Count > 0)
            {
                _identicalSums = null;
                _groupedSums = null;
                ReCalculateSums();
            }
            else
            {
                _identicalSums = null;
                _groupedSums = null;
                if (_allSums != null)
                    FilterTable();
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
                this.Activate();
                this.Focus();
                trayIcon.Visible = false;
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                this.Show();
                this.Activate();
                this.Focus();
                trayIcon.Visible = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _allowExit = true;
            Application.Exit();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchVirusTotal();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "All files|*.*";
                dialog.Multiselect = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                    DetectFiles(dialog.FileNames);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SearchByHash(true);
        }

        private void SearchByHash(bool ddg)
        {
            MoonTreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null || SumView.Nodes.Count == 0)
                return;

            DuplicateHashMatch match = selectedNode.Tag as DuplicateHashMatch;
            if (match != null && !string.IsNullOrEmpty(match.Hash))
            {
                Utilities.SearchHash(match.Hash, ddg);
                return;
            }

            DuplicateGroupInfo group = selectedNode.Tag as DuplicateGroupInfo;
            if (group == null && selectedNode.Parent != null)
                group = selectedNode.Parent.Tag as DuplicateGroupInfo;
            if (group != null && !string.IsNullOrEmpty(group.Hash))
            {
                Utilities.SearchHash(group.Hash, ddg);
                return;
            }

            if (selectedNode.Nodes.Count == 0)
                Utilities.SearchHash(selectedNode.Text, ddg);
        }

        private SumResult GetSelectedSumResult()
        {
            MoonTreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null)
                return null;

            SumResult taggedResult = selectedNode.Tag as SumResult;
            if (taggedResult != null)
                return taggedResult;

            FileNodeInfo fileInfo = selectedNode.Tag as FileNodeInfo;
            if (fileInfo != null)
                return fileInfo.Result;

            DuplicateGroupInfo group = selectedNode.Tag as DuplicateGroupInfo;
            if (group == null && selectedNode.Parent != null)
                group = selectedNode.Parent.Tag as DuplicateGroupInfo;
            if (group != null)
                return group.Items.Length == 0 ? null : group.Items[0];

            MoonTreeNode fileNode = selectedNode.Nodes.Count > 0
                ? selectedNode
                : selectedNode.Parent ?? selectedNode;
            int index = SumResult.Sums.FindIndex(item => string.Equals(item.File, fileNode.Text, StringComparison.OrdinalIgnoreCase));
            return index < 0 ? null : SumResult.Sums[index];
        }

        private void FindFile()
        {
            SumResult selectedResult = GetSelectedSumResult();
            if (selectedResult != null)
            {
                ShowFindFileError(selectedResult.File);
                return;
            }

            MoonTreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null)
                return;

            MoonTreeNode fileNode = selectedNode.Nodes.Count > 0
                ? selectedNode
                : selectedNode.Parent ?? selectedNode;
            ShowFindFileError(fileNode.Text);
        }

        private void ShowFindFileError(string file)
        {
            Exception error;
            if (!Utilities.TryFindFile(file, out error))
                FluentMessageBox.Show(this, FormatFileOperationError(Options.TranslationList["operationShowFileExplorer"].ToString(), file, error),
                    Options.TranslationList["fileOperationErrorsTitle"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SearchByHash(false);
        }

        private void botPanel_Resize(object sender, EventArgs e)
        {
            btnCancelHashing.Left = (botPanel.ClientSize.Width - btnCancelHashing.Width) / 2;
            btnCancelHashing.Top = (botPanel.ClientSize.Height - btnCancelHashing.Height) / 2;
        }

        private void btnCancelHashing_Click(object sender, EventArgs e)
        {
            if (tokenSource == null || tokenSource.IsCancellationRequested)
                return;

            tokenSource.Cancel();
            btnCancelHashing.Enabled = false;
            btnCancelHashing.Text = Options.TranslationList["btnCancelHashingPressed"].ToString();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FindFile();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            FilterTable();
        }

        private void FilterTable()
        {
            if (_allSums == null)
                return;

            string term = txtPath.Text.Trim();
            MoonTreeNode[] source = BuildGroupedDisplayNodes();
            SumView.BeginUpdate();
            try
            {
                SumView.Nodes.Clear();
                if (term.Length == 0)
                {
                    SumView.Nodes.AddRange(source);
                    SumView.ExpandAll();
                    return;
                }

                List<MoonTreeNode> filteredNodes = new List<MoonTreeNode>(source.Length);
                foreach (MoonTreeNode node in source)
                {
                    if (NodeMatchesSearch(node, term))
                        filteredNodes.Add(node);
                }

                SumView.Nodes.AddRange(filteredNodes.ToArray());
                SumView.ExpandAll();
            }
            finally
            {
                SumView.EndUpdate();
            }
        }

        private MoonTreeNode[] BuildGroupedDisplayNodes()
        {
            if (_groupedSums != null)
                return _groupedSums;

            if (_allSums == null || _allSums.Length == 0)
                return new MoonTreeNode[0];

            if (_identicalSums == null)
                _identicalSums = BuildIdenticalNodes();

            Dictionary<SumResult, int> originalIndexes = new Dictionary<SumResult, int>();
            for (int i = 0; i < _allSums.Length; i++)
            {
                SumResult item = GetNodeSumResult(_allSums[i]);
                if (item != null && !originalIndexes.ContainsKey(item))
                    originalIndexes.Add(item, i);
            }

            HashSet<SumResult> groupedItems = new HashSet<SumResult>();
            List<DisplayNodeEntry> entries = new List<DisplayNodeEntry>(_allSums.Length + _identicalSums.Length);
            int sequence = 0;

            foreach (MoonTreeNode groupNode in _identicalSums)
            {
                DuplicateGroupInfo group = groupNode.Tag as DuplicateGroupInfo;
                if (group == null || group.Items.Length < 2)
                    continue;

                int firstIndex = int.MaxValue;
                foreach (SumResult item in group.Items)
                {
                    int index;
                    if (originalIndexes.TryGetValue(item, out index) && index < firstIndex)
                        firstIndex = index;
                }

                if (firstIndex == int.MaxValue)
                    continue;

                foreach (SumResult item in group.Items)
                    groupedItems.Add(item);
                entries.Add(new DisplayNodeEntry(firstIndex, sequence++, groupNode));
            }

            for (int i = 0; i < _allSums.Length; i++)
            {
                MoonTreeNode node = _allSums[i];
                SumResult item = GetNodeSumResult(node);
                if (item == null || !groupedItems.Contains(item))
                    entries.Add(new DisplayNodeEntry(i, sequence++, node));
            }

            entries.Sort((left, right) =>
            {
                int indexComparison = left.Index.CompareTo(right.Index);
                return indexComparison != 0 ? indexComparison : left.Sequence.CompareTo(right.Sequence);
            });

            MoonTreeNode[] result = new MoonTreeNode[entries.Count];
            for (int i = 0; i < entries.Count; i++)
                result[i] = entries[i].Node;
            _groupedSums = result;
            return _groupedSums;
        }

        private sealed class DisplayNodeEntry
        {
            internal readonly int Index;
            internal readonly int Sequence;
            internal readonly MoonTreeNode Node;

            internal DisplayNodeEntry(int index, int sequence, MoonTreeNode node)
            {
                Index = index;
                Sequence = sequence;
                Node = node;
            }
        }

        private static bool NodeMatchesSearch(MoonTreeNode node, string term)
        {
            if (ContainsSearchTerm(node.Text, term))
                return true;

            SumResult taggedResult = node.Tag as SumResult;
            if (taggedResult != null)
                return SumResultMatchesSearch(taggedResult, term);

            FileNodeInfo fileInfo = node.Tag as FileNodeInfo;
            if (fileInfo != null && SumResultMatchesSearch(fileInfo.Result, term))
                return true;

            DuplicateHashMatch hashMatch = node.Tag as DuplicateHashMatch;
            if (hashMatch != null)
                return ContainsSearchTerm(hashMatch.AlgorithmName, term) || ContainsSearchTerm(hashMatch.Hash, term);

            DuplicateGroupInfo group = node.Tag as DuplicateGroupInfo;
            if (group != null)
            {
                foreach (DuplicateHashMatch match in group.HashMatches)
                {
                    if (ContainsSearchTerm(match.AlgorithmName, term) || ContainsSearchTerm(match.Hash, term))
                        return true;
                }
                foreach (SumResult item in group.Items)
                {
                    if (SumResultMatchesSearch(item, term))
                        return true;
                }
                return false;
            }

            foreach (MoonTreeNode child in node.Nodes)
            {
                if (ContainsSearchTerm(child.Text, term))
                    return true;
            }

            return false;
        }

        private static bool SumResultMatchesSearch(SumResult item, string term)
        {
            return ContainsSearchTerm(item.File, term) ||
                   ContainsSearchTerm(item.MD5, term) ||
                   ContainsSearchTerm(item.SHA1, term) ||
                   ContainsSearchTerm(item.SHA256, term) ||
                   ContainsSearchTerm(item.SHA384, term) ||
                   ContainsSearchTerm(item.SHA512, term) ||
                   ContainsSearchTerm(item.RIPEMD160, term) ||
                   ContainsSearchTerm(item.CRC32, term) ||
                   ContainsSearchTerm(item.SHA3_256, term) ||
                   ContainsSearchTerm(item.SHA3_384, term) ||
                   ContainsSearchTerm(item.SHA3_512, term);
        }

        private static bool ContainsSearchTerm(string value, string term)
        {
            return !string.IsNullOrEmpty(value) && value.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void SumView_NodeMouseDoubleClick(object sender, MoonTreeNodeMouseEventArgs e)
        {

        }
    }
}
