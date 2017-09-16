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
        List<string> FileList;

        public MainForm(string[] files)
        {
            InitializeComponent();
            Options.ApplyTheme(this);
            CheckForIllegalCrossThreadCalls = false;
            helperMenu.Renderer = new ToolStripRendererMaterial();

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

        private List<SumResult> CompareSums()
        {
            // get identical files

            IEnumerable<string> similars = from p in SumResult.Sums group p by p.MD5 into g where g.Count() > 1 select g.Key;
            List<SumResult> duplicates = new List<SumResult>();

            foreach (string s in similars)
            {
                foreach (SumResult sr in SumResult.Sums.FindAll(x => x.MD5 == s && x.File != s))
                {
                    duplicates.Add(sr);
                }
            }

            return duplicates;
        }

        private void CalculateSums(string[] files)
        {
            this.AllowDrop = false;
            FileList = new List<string>();

            foreach (string f in files)
            {
                if (Directory.Exists(f))
                {
                    foreach (string x in Directory.GetFiles(f, "*", SearchOption.AllDirectories))
                    {
                        FileList.Add(x);
                    }
                }
                if (File.Exists(f))
                {
                    FileList.Add(f);
                }
            }
            // calculate all sums for all files
            Task.Factory.StartNew(() =>
            {
                lblCalculating.Invoke((MethodInvoker)delegate
                {
                    button1.Enabled = false;
                    button7.Enabled = false;
                    button2.Enabled = false;
                    lblCalculating.Text = "Calculating...";
                    lblCalculating.Visible = true;
                }
                );

                foreach (string f in FileList)
                {
                    if (File.Exists(f))
                    {
                        SumResult.Sums.Add(Utilities.CalculateSums(f));
                    }
                }
            }
            )
            // add nodes and expand them
            .ContinueWith((prevTask) =>
            {
                if ((SumResult.Sums != null) && (SumResult.Sums.Count > 0))
                {
                    SumView.Nodes.Clear();

                    foreach (SumResult sr in SumResult.Sums)
                    {
                        TreeNode node = new TreeNode(sr.File);

                        node.ForeColor = Options.ForegroundColor;
                        node.Tag = Options.ThemeFlag;
                        node.Nodes.Add("MD5: " + sr.MD5);
                        node.Nodes.Add("SHA1: " + sr.SHA1);
                        node.Nodes.Add("SHA256: " + sr.SHA256);
                        node.Nodes.Add("RIPEMD160: " + sr.RIPEMD160);

                        SumView.Invoke((MethodInvoker)delegate
                        {
                            SumView.Nodes.Add(node);
                            SumView.ExpandAll();
                        });
                    }

                    lblCalculating.Invoke((MethodInvoker)delegate
                    {
                        lblCalculating.Text = "Calculating...";
                        lblCalculating.Visible = false;
                        button1.Enabled = true;
                        button7.Enabled = true;
                        button2.Enabled = true;
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
            CalculateSums((string[])e.Data.GetData(DataFormats.FileDrop, false));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblversion.Text = "Version: " + Program.GetCurrentVersionToString();
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
            List<SumResult> list = CompareSums();
            if (SumView.Nodes.Count > 0 && list.Count > 0)
            {
                CompareForm f = new CompareForm(list);
                f.ShowDialog();
            }
            else if (SumView.Nodes.Count > 0 && list.Count == 0)
            {
                MessageBox.Show("No identical files have been found!", "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.SaveSettings();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OptionsForm f = new OptionsForm(this);
            f.ShowDialog();
        }
    }
}
