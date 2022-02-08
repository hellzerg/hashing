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
    public partial class MainForm : Form
    {
        // Always on top syscall... used TopMost property instead

        //static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        //const UInt32 SWP_NOSIZE = 0x0001;
        //const UInt32 SWP_NOMOVE = 0x0002;
        //const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        List<string> _fileList;
        List<FileSummary> _fileSummaries;
        List<string> _analyzedFiles;

        bool _allowExit;

        // used when auto-enabling SHA256, for VirusTotal
        int _tempIndex = -1;

        // used for checking if all files are JSON files
        bool _allFilesAreJson = false;

        // used for counting total size of calculated sum
        FileInfo _fileInfo;
        long _totalSize;
        Stopwatch _timer;
        int _fileCounter;
        string _tempFileName = string.Empty;
        string _tempFileExtension = string.Empty;

        // used for cancelling hashing procedure
        CancellationTokenSource tokenSource;

        readonly string _latestVersionLink = "https://raw.githubusercontent.com/hellzerg/hashing/master/version.txt";

        string _noNewVersionMessage = "You already have the latest version!";
        string _betaVersionMessage = "You are using an experimental version!";

        private string NewVersionMessage(string latestVersion)
        {
            return Options.TranslationList["newVersion"].ToString().Replace("{LATEST}", latestVersion).Replace("{CURRENT}", Program.GetCurrentVersionToString());
        }

        public MainForm(string[] files)
        {
            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            this.DoubleBuffered = true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Options.ApplyTheme(this);
            CheckForIllegalCrossThreadCalls = false;

            _allowExit = !Options.CurrentOptions.TrayIcon;
            _fileList = new List<string>();

            helperMenu.Renderer = new MoonMenuRenderer();
            trayMenu.Renderer = new MoonMenuRenderer();

            _timer = new Stopwatch();

            if (Options.CurrentOptions.HighPriority)
            {
                Utilities.EnableHighPriority();
            }
            else
            {
                Utilities.DisableHighPriority();
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

            if (files.Count() > 0)
            {
                DetectFiles(files);
            }

            RestoreWindowState();

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
            ToolStripMenuItem x = (ToolStripMenuItem)sender;

            if (x.ForeColor == Color.White) x.ForeColor = Options.ForegroundColor;
            else x.ForeColor = x.ForeColor = Color.White;

            if (x.Tag.ToString() == "md5") Options.CurrentOptions.HashOptions.MD5 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha1") Options.CurrentOptions.HashOptions.SHA1 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha256") Options.CurrentOptions.HashOptions.SHA256 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha384") Options.CurrentOptions.HashOptions.SHA384 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha512") Options.CurrentOptions.HashOptions.SHA512 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "crc32") Options.CurrentOptions.HashOptions.CRC32 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "ripemd160") Options.CurrentOptions.HashOptions.RIPEMD160 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha32") Options.CurrentOptions.HashOptions.SHA3_256 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha33") Options.CurrentOptions.HashOptions.SHA3_384 = (x.ForeColor == Options.ForegroundColor);
            if (x.Tag.ToString() == "sha35") Options.CurrentOptions.HashOptions.SHA3_512 = (x.ForeColor == Options.ForegroundColor);

            if (SumResult.Sums.Count > 0)
            {
                ReCalculateSums();
            }
        }

        private void CheckMenuItemsByHashes()
        {
            if (Options.CurrentOptions.HashOptions.MD5) itemMD5.ForeColor = Options.ForegroundColor;
            else itemMD5.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA1) itemSHA1.ForeColor = Options.ForegroundColor;
            else itemSHA1.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA256) itemSHA256.ForeColor = Options.ForegroundColor;
            else itemSHA256.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA384) itemSHA384.ForeColor = Options.ForegroundColor;
            else itemSHA384.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA512) itemSHA512.ForeColor = Options.ForegroundColor;
            else itemSHA512.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.CRC32) itemCRC32.ForeColor = Options.ForegroundColor;
            else itemCRC32.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.RIPEMD160) itemRIPEMD160.ForeColor = Options.ForegroundColor;
            else itemRIPEMD160.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA3_256) itemSHA32.ForeColor = Options.ForegroundColor;
            else itemSHA32.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA3_384) itemSHA33.ForeColor = Options.ForegroundColor;
            else itemSHA33.ForeColor = Color.White;

            if (Options.CurrentOptions.HashOptions.SHA3_512) itemSHA35.ForeColor = Options.ForegroundColor;
            else itemSHA35.ForeColor = Color.White;
        }

        internal void Translate(bool skipFull = false)
        {
            Dictionary<string, string> translationList = Options.TranslationList.ToObject<Dictionary<string, string>>();

            if (!skipFull)
            {
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
                    if (x.Key == null || x.Key == string.Empty) continue;
                    element = this.Controls.Find(x.Key, true).FirstOrDefault();

                    if (element == null) continue;

                    element.Text = x.Value;
                }

            }

            lblversion.Text = lblversion.Text.Replace("{VN}", Program.GetCurrentVersionToString());
        }

        private void EnableSHA256()
        {
            Options.CurrentOptions.HashOptions.SHA256 = true;
            Options.SaveSettings();

            OptionsForm.HashesChanged = true;

            ReCalculateSums();
        }

        private string NewDownloadLink(string latestVersion)
        {
            return string.Format("https://github.com/hellzerg/hashing/releases/download/{0}/Hashing-{0}.exe", latestVersion);
        }

        private void CheckForUpdate()
        {
            WebClient client = new WebClient
            {
                Encoding = Encoding.UTF8
            };

            string latestVersion = string.Empty;
            try
            {
                latestVersion = client.DownloadString(_latestVersionLink).Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!string.IsNullOrEmpty(latestVersion))
            {
                if (float.Parse(latestVersion) > Program.GetCurrentVersion())
                {
                    if (MessageBox.Show(NewVersionMessage(latestVersion), "Hashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            client.DownloadFile(NewDownloadLink(latestVersion), tempFile);

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
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else if (float.Parse(latestVersion) == Program.GetCurrentVersion())
                {
                    MessageBox.Show(_noNewVersionMessage, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(_betaVersionMessage, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Copy()
        {
            string s = string.Empty;
            bool all = false;

            if (SumView.SelectedNode.Nodes.Count > 0)
            {
                s = SumView.SelectedNode.Text + Environment.NewLine + Environment.NewLine;

                foreach (TreeNode node in SumView.SelectedNode.Nodes)
                {
                    s += node.Text + Environment.NewLine;
                }

                all = true;
            }
            else
            {
                s = SumView.SelectedNode.Text;
                all = false;
            }

            Utilities.CopyToClipboard(s, all);
        }

        private void Remove()
        {
            if (SumView.Nodes.Count > 0)
            {
                if (SumView.SelectedNode.Nodes.Count > 0)
                {
                    int i = SumResult.Sums.FindIndex(x => x.File.Equals(SumView.SelectedNode.Text));
                    SumResult.Sums.RemoveAt(i);
                    SumView.Nodes.Remove(SumView.SelectedNode);
                }
                else
                {
                    int i = SumResult.Sums.FindIndex(x => x.File.Equals(SumView.SelectedNode.Parent.Text));
                    SumResult.Sums.RemoveAt(i);
                    SumView.Nodes.Remove(SumView.SelectedNode.Parent);
                }
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
            if (SumView.Nodes.Count > 0)
            {
                int index = -1;

                try
                {
                    if (SumView.SelectedNode.Nodes.Count > 0)
                    {
                        index = SumResult.Sums.FindIndex(x => x.File.Equals(SumView.SelectedNode.Text));
                    }
                    else
                    {
                        index = SumResult.Sums.FindIndex(x => x.File.Equals(SumView.SelectedNode.Parent.Text));
                    }
                }
                catch { }

                if (index > -1)
                {
                    if (!string.IsNullOrEmpty(SumResult.Sums[index].SHA256))
                    {
                        Utilities.SearchVirusTotal(SumResult.Sums[index].SHA256);
                        return;
                    }
                    // enableSHA256Message
                    if (MessageBox.Show(Options.TranslationList["enableSHA256Message"].ToString(), "Hashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        EnableSHA256();
                        _tempIndex = index;
                    }
                }
            }
        }

        private void RefreshSumList()
        {
            if ((SumResult.Sums != null) && (SumResult.Sums.Count > 0))
            {
                SumView.Nodes.Clear();

                foreach (SumResult sr in SumResult.Sums)
                {
                    TreeNode rootNode = new TreeNode(sr.File);

                    rootNode.ForeColor = Options.ForegroundColor;
                    rootNode.Tag = Options.ThemeFlag;

                    if (Options.CurrentOptions.HashOptions.MD5) rootNode.Nodes.Add("MD5: " + sr.MD5);
                    if (Options.CurrentOptions.HashOptions.SHA1) rootNode.Nodes.Add("SHA1: " + sr.SHA1);
                    if (Options.CurrentOptions.HashOptions.SHA256) rootNode.Nodes.Add("SHA256: " + sr.SHA256);
                    if (Options.CurrentOptions.HashOptions.SHA384) rootNode.Nodes.Add("SHA384: " + sr.SHA384);
                    if (Options.CurrentOptions.HashOptions.SHA512) rootNode.Nodes.Add("SHA512: " + sr.SHA512);
                    if (Options.CurrentOptions.HashOptions.CRC32) rootNode.Nodes.Add("CRC32: " + sr.CRC32);
                    if (Options.CurrentOptions.HashOptions.RIPEMD160) rootNode.Nodes.Add("RIPEMD160: " + sr.RIPEMD160);
                    if (Options.CurrentOptions.HashOptions.SHA3_256) rootNode.Nodes.Add("SHA3-256: " + sr.SHA3_256);
                    if (Options.CurrentOptions.HashOptions.SHA3_384) rootNode.Nodes.Add("SHA3-384: " + sr.SHA3_384);
                    if (Options.CurrentOptions.HashOptions.SHA3_512) rootNode.Nodes.Add("SHA3-512: " + sr.SHA3_512);

                    SumView.Nodes.Add(rootNode);
                    SumView.ExpandAll();
                }
            }
        }

        private void SaveAsJSON()
        {
            if (SumView.Nodes.Count > 0)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JSON file|*.json";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(SumResult.Sums, Formatting.Indented, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void FindIdenticals()
        {
            if (SumView.Nodes.Count == 0) return;

            List<SumResult> list = new List<SumResult>();

            if (Options.CurrentOptions.HashOptions.MD5) list = FindIdenticalsByMD5();
            if (Options.CurrentOptions.HashOptions.SHA1) list = FindIdenticalsBySHA1();
            if (Options.CurrentOptions.HashOptions.SHA256) list = FindIdenticalsBySHA256();
            if (Options.CurrentOptions.HashOptions.SHA384) list = FindIdenticalsBySHA384();
            if (Options.CurrentOptions.HashOptions.SHA512) list = FindIdenticalsBySHA512();
            if (Options.CurrentOptions.HashOptions.CRC32) list = FindIdenticalsByCRC32();
            if (Options.CurrentOptions.HashOptions.RIPEMD160) list = FindIdenticalsByRIPEMD160();
            if (Options.CurrentOptions.HashOptions.SHA3_256) list = FindIdenticalsBySHA3_256();
            if (Options.CurrentOptions.HashOptions.SHA3_384) list = FindIdenticalsBySHA3_384();
            if (Options.CurrentOptions.HashOptions.SHA3_512) list = FindIdenticalsBySHA3_512();

            if (SumView.Nodes.Count > 0 && list.Count > 0)
            {
                IdenticalsForm f = new IdenticalsForm(list);
                f.ShowDialog();
            }
            else if (SumView.Nodes.Count > 0 && list.Count == 0)
            {
                MessageBox.Show(Options.TranslationList["noIdenticalsMessage"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private List<SumResult> FindIdenticalsByMD5()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.MD5 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.MD5 == s && x.File != s && !string.IsNullOrEmpty(x.MD5) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA1()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA1 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA1 == s && x.File != s && !string.IsNullOrEmpty(x.SHA1) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA256()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA256 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA256 == s && x.File != s && !string.IsNullOrEmpty(x.SHA256) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA384()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA384 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA384 == s && x.File != s && !string.IsNullOrEmpty(x.SHA384) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA512()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA512 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA512 == s && x.File != s && !string.IsNullOrEmpty(x.SHA512) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsByCRC32()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.CRC32 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.CRC32 == s && x.File != s && !string.IsNullOrEmpty(x.CRC32) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsByRIPEMD160()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.RIPEMD160 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.RIPEMD160 == s && x.File != s && !string.IsNullOrEmpty(x.RIPEMD160) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA3_256()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA3_256 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA3_256 == s && x.File != s && !string.IsNullOrEmpty(x.SHA3_256) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA3_384()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA3_384 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA3_384 == s && x.File != s && !string.IsNullOrEmpty(x.SHA3_384) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private List<SumResult> FindIdenticalsBySHA3_512()
        {
            IEnumerable<string> similars = from p in SumResult.Sums group p by p.SHA3_512 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.SHA3_512 == s && x.File != s && !string.IsNullOrEmpty(x.SHA3_512) && !string.IsNullOrEmpty(s)))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private void ReCalculateSums()
        {
            if (!Options.CurrentOptions.HashOptions.MD5 && !Options.CurrentOptions.HashOptions.SHA1 && !Options.CurrentOptions.HashOptions.SHA256 && !Options.CurrentOptions.HashOptions.SHA384 && !Options.CurrentOptions.HashOptions.SHA512 && !Options.CurrentOptions.HashOptions.SHA3_384 && !Options.CurrentOptions.HashOptions.CRC32 && !Options.CurrentOptions.HashOptions.SHA3_256 && !Options.CurrentOptions.HashOptions.SHA3_256 && !Options.CurrentOptions.HashOptions.SHA3_512)
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

        private string ListIncompatibles(List<string> files)
        {
            StringBuilder result = new StringBuilder();

            foreach (string s in files)
            {
                result.Append(s + Environment.NewLine);
            }

            return result.ToString();
        }

        private List<FileSummary> AnalyzeJsonFiles()
        {
            List<FileSummary> fileSummaries = new List<FileSummary>();
            List<string> incompatibles = new List<string>();

            foreach (string s in _fileList)
            {
                try
                {
                    fileSummaries.AddRange(JsonConvert.DeserializeObject<List<FileSummary>>(File.ReadAllText(s)));
                }
                catch //(Exception ex)
                {
                    incompatibles.Add(s);
                    continue;
                }
            }

            _fileList.RemoveAll(x => x.EndsWith(".json"));

            if (incompatibles.Count > 0)
            {
                TopMostMessageBox.Show(Options.TranslationList["unsupportedJSON1"].ToString() + ListIncompatibles(incompatibles) + Options.TranslationList["unsupportedJSON2"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (FileSummary json in fileSummaries)
            {
                _fileList.Add(json.File);
            }

            return fileSummaries;
        }

        private void DetectFiles(string[] files)
        {
            //this.AllowDrop = false;
            _fileList.Clear();

            foreach (string f in files)
            {
                if (Directory.Exists(f))
                {
                    foreach (string x in Directory.GetFiles(f, "*", SearchOption.AllDirectories))
                    {
                        _fileList.Add(x);
                    }
                }
                if (File.Exists(f))
                {
                    _fileList.Add(f);
                }
            }

            // check if all files are JSON files
            _allFilesAreJson = true;

            foreach (string y in _fileList)
            {
                if (!y.EndsWith(".json"))
                {
                    _allFilesAreJson = false;
                    break;
                }
            }

            if (_allFilesAreJson)
            {
                FeatureBox ab = new FeatureBox();
                ab.ShowDialog(this);

                if (FeatureBox.SelectedAction == FeatureAction.Calculate)
                {
                    CalculateSums();
                }
                else if (FeatureBox.SelectedAction == FeatureAction.AnalyzeJSON)
                {
                    Clear();
                    _fileSummaries = AnalyzeJsonFiles();
                    _fileSummaries.RemoveAll(x => !File.Exists(x.File));

                    if (_fileSummaries != null)
                    {
                        if (_fileSummaries.Count > 0)
                        {
                            CalculateSums(true);
                        }
                        else
                        {
                            TopMostMessageBox.Show(Options.TranslationList["invalidPath"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (FeatureBox.SelectedAction == FeatureAction.ListJSON)
                {
                    List<SumResult> listing = new List<SumResult>();

                    foreach (string s in _fileList)
                    {
                        try
                        {
                            listing.AddRange(JsonConvert.DeserializeObject<List<SumResult>>(File.ReadAllText(s)));
                        }
                        catch { continue; }
                    }

                    if (listing.Count > 0)
                    {
                        SumResult.Sums.AddRange(listing);
                        lblCalculating.Visible = false;
                        List<TreeNode> roots = new List<TreeNode>();

                        for (int i = 0; i < listing.Count; i++)
                        {
                            TreeNode rootNode = new TreeNode(listing[i].File);

                            rootNode.ForeColor = Options.ForegroundColor;
                            rootNode.Tag = Options.ThemeFlag;

                            if (!string.IsNullOrEmpty(listing[i].MD5)) rootNode.Nodes.Add("MD5: " + listing[i].MD5);
                            if (!string.IsNullOrEmpty(listing[i].SHA1)) rootNode.Nodes.Add("SHA1: " + listing[i].SHA1);
                            if (!string.IsNullOrEmpty(listing[i].SHA256)) rootNode.Nodes.Add("SHA256: " + listing[i].SHA256);
                            if (!string.IsNullOrEmpty(listing[i].SHA384)) rootNode.Nodes.Add("SHA384: " + listing[i].SHA384);
                            if (!string.IsNullOrEmpty(listing[i].SHA512)) rootNode.Nodes.Add("SHA512: " + listing[i].SHA512);
                            if (!string.IsNullOrEmpty(listing[i].CRC32)) rootNode.Nodes.Add("CRC32: " + listing[i].CRC32);
                            if (!string.IsNullOrEmpty(listing[i].RIPEMD160)) rootNode.Nodes.Add("RIPEMD160: " + listing[i].RIPEMD160);
                            if (!string.IsNullOrEmpty(listing[i].SHA3_256)) rootNode.Nodes.Add("SHA3-256: " + listing[i].SHA3_256);
                            if (!string.IsNullOrEmpty(listing[i].SHA3_384)) rootNode.Nodes.Add("SHA3-384: " + listing[i].SHA3_384);
                            if (!string.IsNullOrEmpty(listing[i].SHA3_512)) rootNode.Nodes.Add("SHA3-512: " + listing[i].SHA3_512);

                            roots.Add(rootNode);
                        }

                        SumView.Nodes.AddRange(roots.ToArray());
                        SumView.ExpandAll();
                    }
                }
            }
            else
            {
                CalculateSums();
            }

            this.AllowDrop = true;
        }

        private void CalculateSums(bool analyzeJson = false)
        {
            this.Text = "Hashing";

            tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;

            Task.Factory.StartNew(() =>
            {
                lblCalculating.Invoke((MethodInvoker)delegate
                {
                    btnUpdate.Enabled = false;
                    btnOptions.Enabled = false;
                    btnSaveJson.Enabled = false;
                    btnFindIdenticals.Enabled = false;
                    btnCompare.Enabled = false;
                    btnBrowse.Enabled = false;
                    btnCalculate.Enabled = false;
                    txtPath.Enabled = false;
                    lblCalculating.Text = Options.TranslationList["lblCalculatingNow"].ToString();
                    lblCalculating.Visible = true;
                    btnCancelHashing.Visible = true;
                }
                );

                _fileCounter = 0;
                _timer.Reset();
                _timer.Start();
                _totalSize = 0;

                foreach (string f in _fileList)
                {
                    ct.ThrowIfCancellationRequested();

                    if (File.Exists(f))
                    {
                        _fileCounter++;

                        _tempFileName = Path.GetFileName(f);
                        _tempFileExtension = Path.GetExtension(f);
                        if (_tempFileName.Length > 28) _tempFileName = _tempFileName.Substring(0, 27) + " ... " + _tempFileExtension;

                        lblCalculating.Invoke((MethodInvoker)delegate
                        {
                            lblCalculating.Text = string.Format("{0}/{1} - '{2}'", _fileCounter, _fileList.Count, _tempFileName);
                            lblCalculating.Visible = true;
                            btnCancelHashing.Visible = true;
                        });

                        _fileInfo = new FileInfo(f);
                        _totalSize += _fileInfo.Length;

                        SumResult.Sums.Add(Utilities.CalculateSums(f));
                    }
                    else { continue; }
                }

                _timer.Stop();

            }, tokenSource.Token
            )
            .ContinueWith((prevTask) =>
            {
                // if cancelled: reset UI
                if (tokenSource.IsCancellationRequested)
                {
                    lblCalculating.Invoke((MethodInvoker)delegate
                    {
                        _timer.Stop();
                        btnCancelHashing.Visible = false;
                        btnCancelHashing.Enabled = true;
                        btnCancelHashing.Text = Options.TranslationList["btnCancelHashing"].ToString();
                    });
                }

                if ((SumResult.Sums != null) && (SumResult.Sums.Count > 0))
                {
                    SumView.Nodes.Clear();

                    TreeNode rootNode = null;
                    _analyzedFiles = new List<string>();

                    if (analyzeJson)
                    {
                        for (int i = 0; i < SumResult.Sums.Count; i++)
                        {
                            rootNode = new TreeNode(SumResult.Sums[i].File);

                            rootNode.ForeColor = Options.ForegroundColor;
                            rootNode.Tag = Options.ThemeFlag;

                            if (Options.CurrentOptions.HashOptions.MD5)
                            {
                                rootNode.Nodes.Add("MD5: " + SumResult.Sums[i].MD5);
                                if (SumResult.Sums[i].MD5 == _fileSummaries[i].MD5) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA1)
                            {
                                rootNode.Nodes.Add("SHA1: " + SumResult.Sums[i].SHA1);
                                if (SumResult.Sums[i].SHA1 == _fileSummaries[i].SHA1) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA256)
                            {

                                rootNode.Nodes.Add("SHA256: " + SumResult.Sums[i].SHA256);
                                if (SumResult.Sums[i].SHA256 == _fileSummaries[i].SHA256) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA384)
                            {
                                rootNode.Nodes.Add("SHA384: " + SumResult.Sums[i].SHA384);
                                if (SumResult.Sums[i].SHA384 == _fileSummaries[i].SHA384) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA512)
                            {
                                rootNode.Nodes.Add("SHA512: " + SumResult.Sums[i].SHA512);
                                if (SumResult.Sums[i].SHA512 == _fileSummaries[i].SHA512) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.CRC32)
                            {
                                rootNode.Nodes.Add("CRC32: " + SumResult.Sums[i].CRC32);
                                if (SumResult.Sums[i].CRC32 == _fileSummaries[i].CRC32) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.RIPEMD160)
                            {
                                rootNode.Nodes.Add("RIPEMD160: " + SumResult.Sums[i].RIPEMD160);
                                if (SumResult.Sums[i].RIPEMD160 == _fileSummaries[i].RIPEMD160) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA3_256)
                            {
                                rootNode.Nodes.Add("SHA3-256: " + SumResult.Sums[i].SHA3_256);
                                if (SumResult.Sums[i].SHA3_256 == _fileSummaries[i].SHA3_256) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA3_384)
                            {
                                rootNode.Nodes.Add("SHA3-384: " + SumResult.Sums[i].SHA3_384);
                                if (SumResult.Sums[i].SHA3_384 == _fileSummaries[i].SHA3_384) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            if (Options.CurrentOptions.HashOptions.SHA3_512)
                            {
                                rootNode.Nodes.Add("SHA3-512: " + SumResult.Sums[i].SHA3_512);
                                if (SumResult.Sums[i].SHA3_512 == _fileSummaries[i].SHA3_512) _analyzedFiles.Add(_fileSummaries[i].File);
                            }

                            SumView.Invoke((MethodInvoker)delegate
                            {
                                SumView.Nodes.Add(rootNode);
                            });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < SumResult.Sums.Count; i++)
                        {
                            rootNode = new TreeNode(SumResult.Sums[i].File);

                            rootNode.ForeColor = Options.ForegroundColor;
                            rootNode.Tag = Options.ThemeFlag;

                            if (Options.CurrentOptions.HashOptions.MD5) rootNode.Nodes.Add("MD5: " + SumResult.Sums[i].MD5);
                            if (Options.CurrentOptions.HashOptions.SHA1) rootNode.Nodes.Add("SHA1: " + SumResult.Sums[i].SHA1);
                            if (Options.CurrentOptions.HashOptions.SHA256) rootNode.Nodes.Add("SHA256: " + SumResult.Sums[i].SHA256);
                            if (Options.CurrentOptions.HashOptions.SHA384) rootNode.Nodes.Add("SHA384: " + SumResult.Sums[i].SHA384);
                            if (Options.CurrentOptions.HashOptions.SHA512) rootNode.Nodes.Add("SHA512: " + SumResult.Sums[i].SHA512);
                            if (Options.CurrentOptions.HashOptions.CRC32) rootNode.Nodes.Add("CRC32: " + SumResult.Sums[i].CRC32);
                            if (Options.CurrentOptions.HashOptions.RIPEMD160) rootNode.Nodes.Add("RIPEMD160: " + SumResult.Sums[i].RIPEMD160);
                            if (Options.CurrentOptions.HashOptions.SHA3_256) rootNode.Nodes.Add("SHA3-256: " + SumResult.Sums[i].SHA3_256);
                            if (Options.CurrentOptions.HashOptions.SHA3_384) rootNode.Nodes.Add("SHA3-384: " + SumResult.Sums[i].SHA3_384);
                            if (Options.CurrentOptions.HashOptions.SHA3_512) rootNode.Nodes.Add("SHA3-512: " + SumResult.Sums[i].SHA3_512);

                            SumView.Invoke((MethodInvoker)delegate
                            {
                                SumView.Nodes.Add(rootNode);
                            });
                        }
                    }

                    SumView.Invoke((MethodInvoker)delegate
                    {
                        SumView.ExpandAll();
                    });

                    lblCalculating.Invoke((MethodInvoker)delegate
                    {
                        lblCalculating.Text = Options.TranslationList["lblCalculatingNow"].ToString();
                        lblCalculating.Visible = false;
                        btnCancelHashing.Visible = false;
                        btnOptions.Enabled = true;
                        btnUpdate.Enabled = true;
                        btnSaveJson.Enabled = true;
                        btnFindIdenticals.Enabled = true;
                        btnCompare.Enabled = true;
                        btnBrowse.Enabled = true;
                        btnCalculate.Enabled = true;
                        txtPath.Enabled = true;

                        this.Text = string.Format("Hashing - {0}m : {1}s / {2} {3} ({4})", _timer.Elapsed.Minutes, _timer.Elapsed.Seconds, _fileCounter, Options.TranslationList["filesWord"].ToString(), ByteSize.FromBytes(_totalSize));
                    });

                    try
                    {
                        SumView.Nodes[0].EnsureVisible();
                    }
                    catch { }

                    // opening VirusTotal, after enabling SHA256 hash
                    if (_tempIndex > -1)
                    {
                        Utilities.SearchVirusTotal(SumResult.Sums[_tempIndex].SHA256);
                        _tempIndex = -1;
                    }
                }


                // if JSON analyzation is enabled, show results
                if (analyzeJson)
                {
                    if (_analyzedFiles.Distinct().Count() > 0)
                    {
                        AnalyzedForm f = new AnalyzedForm(_analyzedFiles.Distinct());
                        f.ShowDialog();
                    }
                    else
                    {
                        TopMostMessageBox.Show(Options.TranslationList["corruptedJSON"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            );
        }

        internal void FixColor()
        {
            foreach (TreeNode node in SumView.Nodes)
            {
                if ((string)node.Tag == Options.ThemeFlag)
                {
                    node.ForeColor = Options.ForegroundColor;
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (!Options.CurrentOptions.HashOptions.MD5 && !Options.CurrentOptions.HashOptions.SHA1 && !Options.CurrentOptions.HashOptions.SHA256 && !Options.CurrentOptions.HashOptions.SHA384 && !Options.CurrentOptions.HashOptions.SHA512 && !Options.CurrentOptions.HashOptions.SHA3_512 && !Options.CurrentOptions.HashOptions.CRC32 && !Options.CurrentOptions.HashOptions.SHA3_256 && !Options.CurrentOptions.HashOptions.SHA3_384 && !Options.CurrentOptions.HashOptions.SHA3_512)
            {
                MessageBox.Show(Options.TranslationList["noActiveHash"].ToString(), "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DetectFiles((string[])e.Data.GetData(DataFormats.FileDrop, false));
            }
        }

        private void RestoreWindowState()
        {
            this.WindowState = Options.CurrentOptions.WindowState;
            this.Size = Options.CurrentOptions.WindowSize;

            if (Options.CurrentOptions.WindowLocation != null)
            {
                this.Location = (Point)Options.CurrentOptions.WindowLocation;
            }
            else
            {
                this.CenterToScreen();
            }
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            
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
            SaveAsJSON();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AboutForm f = new AboutForm();
            f.ShowDialog();
        }

        private void sumView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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

        private void btnFindIdenticals_Click(object sender, EventArgs e)
        {
            FindIdenticals();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_allowExit)
            {
                SaveWindowState();
                Options.SaveSettings();
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
            OptionsForm f = new OptionsForm(this);
            f.ShowDialog();

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
                ReCalculateSums();
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

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (SumView.Nodes.Count > 0)
            {
                CompareForm f = new CompareForm();
                f.ShowDialog();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SearchVirusTotal();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files|*.*";
            dialog.Multiselect = false;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dialog.FileName;
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPath.Text))
            {
                if (File.Exists(txtPath.Text))
                {
                    DetectFiles(new string[] { txtPath.Text });
                    txtPath.Clear();
                }

                if (Directory.Exists(txtPath.Text))
                {
                    DetectFiles(new string[] { txtPath.Text });
                    txtPath.Clear();
                }
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
            if (SumView.Nodes.Count > 0)
            {
                if (SumView.SelectedNode.Nodes.Count == 0)
                {
                    Utilities.SearchHash(SumView.SelectedNode.Text, ddg);
                }
            }
        }

        private void FindFile()
        {
            if (SumView.Nodes.Count > 0)
            {
                if (SumView.SelectedNode.Nodes.Count > 0)
                {
                    if (File.Exists(SumView.SelectedNode.Text)) Process.Start("explorer.exe", "/select, " + SumView.SelectedNode.Text);
                }
                else
                {
                    if (File.Exists(SumView.SelectedNode.Parent.Text)) Process.Start("explorer.exe", "/select, " + SumView.SelectedNode.Parent.Text);
                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SearchByHash(false);
        }

        private void botPanel_Resize(object sender, EventArgs e)
        {
            btnCancelHashing.Left = (botPanel.Width - btnCancelHashing.Width) / 2;
            btnCancelHashing.Top = (botPanel.Height - btnCancelHashing.Height + 74) / 2;
        }

        private void btnCancelHashing_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();

            btnCancelHashing.Enabled = false;
            btnCancelHashing.Text = Options.TranslationList["btnCancelHashingPressed"].ToString();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            FindFile();
        }
    }
}
