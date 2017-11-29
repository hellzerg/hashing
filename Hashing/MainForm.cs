using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Hashing
{
    public partial class MainForm : Form
    {
        List<string> _fileList;

        bool _allowExit;

        public MainForm(string[] files)
        {
            InitializeComponent();

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
                CalculateSums(files);
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

                    CalculateSums(currentList);
                }
            }
        }

        private void CalculateSums(string[] files)
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

            Task.Factory.StartNew(() =>
            {
                lblCalculating.Invoke((MethodInvoker)delegate
                {
                    button1.Enabled = false;
                    button4.Enabled = false;
                    button7.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
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

                        SumView.Invoke((MethodInvoker)delegate
                        {
                            SumView.Nodes.Add(rootNode);
                            SumView.ExpandAll();
                        });
                    }

                    lblCalculating.Invoke((MethodInvoker)delegate
                    {
                        lblCalculating.Text = "Calculating...";
                        lblCalculating.Visible = false;
                        button1.Enabled = true;
                        button4.Enabled = true;
                        button7.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                    }
                    );
                }
            }
            );

            this.AllowDrop = true;
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
                CalculateSums((string[])e.Data.GetData(DataFormats.FileDrop, false));
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

        private void button7_Click(object sender, EventArgs e)
        {
            SaveAsJSON();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            FindIdenticals();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_allowExit)
            {
                Options.SaveSettings();
            }
            else
            {
                e.Cancel = true;
                trayIcon.Visible = true;
                this.Hide();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OptionsForm f = new OptionsForm(this);
            f.ShowDialog();

            _allowExit = !Options.CurrentOptions.TrayIcon;

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

            if (OptionsForm.HashesChanged)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (SumView.Nodes.Count > 0)
            {
                CompareForm f = new CompareForm();
                f.ShowDialog();
            }
        }
    }
}
