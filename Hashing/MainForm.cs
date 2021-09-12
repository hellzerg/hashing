using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        readonly string _latestVersionLink = "https://raw.githubusercontent.com/hellzerg/hashing/master/version.txt";
        readonly string _releasesLink = "https://github.com/hellzerg/hashing/releases";

        readonly string _noNewVersionMessage = "You already have the latest version!";
        readonly string _betaVersionMessage = "You are using an experimental version!";

        private string NewVersionMessage(string latest)
        {
            return string.Format("There is a new version available!\n\nLatest version: {0}\nCurrent version: {1}\n\nDo you want to download it now?", latest, Program.GetCurrentVersionToString());
        }

        public MainForm(string[] files)
        {
            InitializeComponent();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Options.ApplyTheme(this);
            CheckForIllegalCrossThreadCalls = false;

            _allowExit = !Options.CurrentOptions.TrayIcon;
            _fileList = new List<string>();

            helperMenu.Renderer = new ToolStripRendererMaterial();
            trayMenu.Renderer = new ToolStripRendererMaterial();

            if (Options.CurrentOptions.HighPriority)
            {
                Utilities.EnableHighPriority();
            }
            else
            {
                Utilities.DisableHighPriority();
            }

            if (files.Count() > 0)
            {
                DetectFiles(files);
            }
        }

        private void EnableSHA256()
        {
            Options.CurrentOptions.HashOptions.SHA256 = true;
            Options.SaveSettings();

            OptionsForm.HashesChanged = true;

            ReCalculateSums();
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
                latestVersion = client.DownloadString(_latestVersionLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!string.IsNullOrEmpty(latestVersion))
            {
                if (float.Parse(latestVersion) > Program.GetCurrentVersion())
                {
                    if (MessageBox.Show(NewVersionMessage(latestVersion), "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            Process.Start(_releasesLink);
                        }
                        catch { }
                    }
                }
                else if (float.Parse(latestVersion) == Program.GetCurrentVersion())
                {
                    MessageBox.Show(_noNewVersionMessage, "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(_betaVersionMessage, "No update available", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                lblCalculating.Text = "Drag and drop files here...";
                lblCalculating.Visible = true;
            }
        }

        private void Clear()
        {
            this.AllowDrop = false;

            SumView.Nodes.Clear();
            SumResult.Sums.Clear();

            lblCalculating.Text = "Drag and drop files here...";
            lblCalculating.Visible = true;

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

                    if (MessageBox.Show("VirusTotal recognizes files only by their SHA256 hash!\n\nDo you wish to enable SHA256 for this operation?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(SumResult.Sums, Formatting.Indented));
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

            if (SumView.Nodes.Count > 0 && list.Count > 0)
            {
                IdenticalsForm f = new IdenticalsForm(list);
                f.ShowDialog();
            }
            else if (SumView.Nodes.Count > 0 && list.Count == 0)
            {
                MessageBox.Show("No identical files have been found!", "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ReCalculateSums()
        {
            if (!Options.CurrentOptions.HashOptions.MD5 && !Options.CurrentOptions.HashOptions.SHA1 && !Options.CurrentOptions.HashOptions.SHA256 && !Options.CurrentOptions.HashOptions.SHA384 && !Options.CurrentOptions.HashOptions.SHA512 && !Options.CurrentOptions.HashOptions.RIPEMD160 && !Options.CurrentOptions.HashOptions.CRC32)
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
                TopMostMessageBox.Show("Unsupported JSON files detected:\n\n" + ListIncompatibles(incompatibles) + "\nThese files will not be analyzed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            foreach (FileSummary json in fileSummaries)
            {
                _fileList.Add(json.File);
            }

            return fileSummaries;
        }

        private void DetectFiles(string[] files)
        {
            this.AllowDrop = false;

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
                if (TopMostMessageBox.Show("Hashing detected that all files are in JSON, do you wish to analyze them?\nThis will clear previously added files!", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Clear();
                    _fileSummaries = AnalyzeJsonFiles();

                    if (_fileSummaries != null)
                    {
                        if (_fileSummaries.Count > 0)
                        {
                            CalculateSums(true);
                        }
                    }
                }
                else
                {
                    CalculateSums();
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
            Task.Factory.StartNew(() =>
            {
                lblCalculating.Invoke((MethodInvoker)delegate
                {
                    btnClear.Enabled = false;
                    btnOptions.Enabled = false;
                    btnSaveJson.Enabled = false;
                    btnFindIdenticals.Enabled = false;
                    btnCompare.Enabled = false;
                    btnBrowse.Enabled = false;
                    btnCalculate.Enabled = false;
                    txtPath.Enabled = false;
                    lblCalculating.Text = "Calculating...";
                    lblCalculating.Visible = true;
                }
                );

                foreach (string f in _fileList)
                {
                    if (File.Exists(f))
                    {
                        SumResult.Sums.Add(Utilities.CalculateSums(f));
                    }
                }
            }
            )
            .ContinueWith((prevTask) =>
            {
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
                        lblCalculating.Text = "Calculating...";
                        lblCalculating.Visible = false;
                        btnClear.Enabled = true;
                        btnOptions.Enabled = true;
                        btnSaveJson.Enabled = true;
                        btnFindIdenticals.Enabled = true;
                        btnCompare.Enabled = true;
                        btnBrowse.Enabled = true;
                        btnCalculate.Enabled = true;
                        txtPath.Enabled = true;
                    }
                    );

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
                        TopMostMessageBox.Show("All files analyzed by JSON are corrupted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!Options.CurrentOptions.HashOptions.MD5 && !Options.CurrentOptions.HashOptions.SHA1 && !Options.CurrentOptions.HashOptions.SHA256 && !Options.CurrentOptions.HashOptions.SHA384 && !Options.CurrentOptions.HashOptions.SHA512 && !Options.CurrentOptions.HashOptions.RIPEMD160 && !Options.CurrentOptions.HashOptions.CRC32)
            {
                MessageBox.Show("No active hashes! Please select at least one hash from options!", "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            lblversion.Text = "Version: " + Program.GetCurrentVersionToString();
            trayIcon.Visible = false;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SearchByHash(false);
        }
    }
}
