using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Hashing
{
    public partial class IdenticalsForm : Form
    {
        List<SumResult> _identicals;
        IEnumerable<string> _uniqueHashes = new List<string>();

        string _helper = string.Empty;

        public IdenticalsForm(List<SumResult> list)
        {
            InitializeComponent();
            Options.ApplyTheme(this);
            helperMenu.Renderer = new MoonMenuRenderer();

            this.Text = Options.TranslationList["btnFindIdenticals"].ToString();
            copyToolStripMenuItem.Text = Options.TranslationList["copyToolStripMenuItem"].ToString();
            saveAsJSONToolStripMenuItem.Text = Options.TranslationList["btnSaveJson"].ToString();

            ConfigureGUI();
            _identicals = list;

            if (boxSelectHash.Items.Count > 0) boxSelectHash.SelectedIndex = 0;

            SumView.Select();
            SumView.Focus();
            if (SumView.Nodes.Count > 0) SumView.Nodes[0].EnsureVisible();

            toolStripMenuItem1.Text = Options.TranslationList["toolStripMenuItem4"].ToString();
        }

        private void ConfigureGUI()
        {
            if (Options.CurrentOptions.HashOptions.MD5) boxSelectHash.Items.Add("MD5");
            if (Options.CurrentOptions.HashOptions.SHA1) boxSelectHash.Items.Add("SHA1");
            if (Options.CurrentOptions.HashOptions.SHA256) boxSelectHash.Items.Add("SHA256");
            if (Options.CurrentOptions.HashOptions.SHA384) boxSelectHash.Items.Add("SHA384");
            if (Options.CurrentOptions.HashOptions.SHA512) boxSelectHash.Items.Add("SHA512");
            if (Options.CurrentOptions.HashOptions.RIPEMD160) boxSelectHash.Items.Add("RIPEMD160");
            if (Options.CurrentOptions.HashOptions.CRC32) boxSelectHash.Items.Add("CRC32");
            if (Options.CurrentOptions.HashOptions.SHA3_256) boxSelectHash.Items.Add("SHA3-256");
            if (Options.CurrentOptions.HashOptions.SHA3_384) boxSelectHash.Items.Add("SHA3-384");
            if (Options.CurrentOptions.HashOptions.SHA3_512) boxSelectHash.Items.Add("SHA3-512");
        }

        private void Copy()
        {
            string total = string.Empty;

            if (SumView.SelectedNode.Nodes.Count > 0)
            {
                total = _helper + SumView.SelectedNode.Text + Environment.NewLine + Environment.NewLine;

                foreach (TreeNode node in SumView.SelectedNode.Nodes)
                {
                    total += node.Text + Environment.NewLine;
                }
            }
            else
            {
                total = SumView.SelectedNode.Text;
            }

            Utilities.CopyToClipboard(total, true);
        }

        private void SaveJson()
        {
            if (SumView.Nodes.Count > 0)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "JSON file|*.json";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(_identicals, Formatting.Indented, new JsonSerializerSettings
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

        private void ListIdenticals(HashCode algo)
        {
            List<TreeNode> nodes = new List<TreeNode>();

            if (algo == HashCode.MD5)
            {
                _uniqueHashes = _identicals.Select(x => x.MD5).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.MD5 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA1)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA1).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA1 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA256)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA256).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA256 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA384)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA384).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA384 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA512)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA512).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA512 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.RIPEMD160)
            {
                _uniqueHashes = _identicals.Select(x => x.RIPEMD160).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.RIPEMD160 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.CRC32)
            {
                _uniqueHashes = _identicals.Select(x => x.CRC32).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.CRC32 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA3_256)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA3_256).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA3_256 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA3_384)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA3_384).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA3_384 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            if (algo == HashCode.SHA3_512)
            {
                _uniqueHashes = _identicals.Select(x => x.SHA3_512).Distinct();

                foreach (string x in _uniqueHashes)
                {
                    TreeNode fileNode = new TreeNode(x);
                    fileNode.ForeColor = Options.ForegroundColor;

                    _identicals.FindAll(z => z.SHA3_512 == x).ForEach(y =>
                    {
                        fileNode.Nodes.Add(y.File);
                    });

                    nodes.Add(fileNode);
                }
            }

            SumView.Nodes.AddRange(nodes.ToArray());
            SumView.ExpandAll();
        }

        private void CompareForm_Load(object sender, EventArgs e)
        {
            if (Options.CurrentOptions.StayOnTop)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void SumView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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

        private void saveAsJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveJson();
        }

        private void boxSelectHash_SelectedIndexChanged(object sender, EventArgs e)
        {
            SumView.Nodes.Clear();

            _helper = $"{boxSelectHash.SelectedItem}: ";

            switch (boxSelectHash.SelectedItem.ToString())
            {
                case "MD5":
                    ListIdenticals(HashCode.MD5);
                    break;

                case "SHA1":
                    ListIdenticals(HashCode.SHA1);
                    break;

                case "SHA256":
                    ListIdenticals(HashCode.SHA256);
                    break;

                case "SHA384":
                    ListIdenticals(HashCode.SHA384);
                    break;

                case "SHA512":
                    ListIdenticals(HashCode.SHA512);
                    break;

                case "RIPEMD160":
                    ListIdenticals(HashCode.RIPEMD160);
                    break;

                case "CRC32":
                    ListIdenticals(HashCode.CRC32);
                    break;

                case "SHA3-256":
                    ListIdenticals(HashCode.SHA3_256);
                    break;

                case "SHA3-384":
                    ListIdenticals(HashCode.SHA3_384);
                    break;

                case "SHA3-512":
                    ListIdenticals(HashCode.SHA3_512);
                    break;
            }

            SumView.Select();
            SumView.Focus();
            if (SumView.Nodes.Count > 0) SumView.Nodes[0].EnsureVisible();
        }

        private void IdenticalsForm_Resize(object sender, EventArgs e)
        {
            boxSelectHash.Location = new System.Drawing.Point()
            {
                X = panel1.Width / 2 - boxSelectHash.Width / 2,
                Y = panel1.Height / 2 - boxSelectHash.Height / 2
            };
        }

        private void FindFile()
        {
            if (SumView.Nodes.Count > 0)
            {
                if (SumView.SelectedNode.Nodes.Count > 0)
                {
                    Utilities.FindFile(SumView.SelectedNode.Nodes[0].Text);
                }
                else
                {
                    Utilities.FindFile(SumView.SelectedNode.Text);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FindFile();
        }
    }
}
