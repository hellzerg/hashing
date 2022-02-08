using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Hashing
{
    public partial class IdenticalsForm : Form
    {
        List<SumResult> _identicals;
        List<string> _uniqueHashes = new List<string>();

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
            GetActiveHash();
        }

        private void ConfigureGUI()
        {
            radioSHA35.Enabled = Options.CurrentOptions.HashOptions.SHA3_512;
            radioSHA35.Checked = Options.CurrentOptions.HashOptions.SHA3_512;

            radioSHA33.Enabled = Options.CurrentOptions.HashOptions.SHA3_384;
            radioSHA33.Checked = Options.CurrentOptions.HashOptions.SHA3_384;

            radioSHA32.Enabled = Options.CurrentOptions.HashOptions.SHA3_256;
            radioSHA32.Checked = Options.CurrentOptions.HashOptions.SHA3_256;

            radioRIPEMD160.Enabled = Options.CurrentOptions.HashOptions.RIPEMD160;
            radioRIPEMD160.Checked = Options.CurrentOptions.HashOptions.RIPEMD160;

            radioCRC32.Enabled = Options.CurrentOptions.HashOptions.CRC32;
            radioCRC32.Checked = Options.CurrentOptions.HashOptions.CRC32;

            radioSHA512.Enabled = Options.CurrentOptions.HashOptions.SHA512;
            radioSHA512.Checked = Options.CurrentOptions.HashOptions.SHA512;

            radioSHA384.Enabled = Options.CurrentOptions.HashOptions.SHA384;
            radioSHA384.Checked = Options.CurrentOptions.HashOptions.SHA384;

            radioSHA256.Enabled = Options.CurrentOptions.HashOptions.SHA256;
            radioSHA256.Checked = Options.CurrentOptions.HashOptions.SHA256;

            radioSHA1.Enabled = Options.CurrentOptions.HashOptions.SHA1;
            radioSHA1.Checked = Options.CurrentOptions.HashOptions.SHA1;

            radioMD5.Enabled = Options.CurrentOptions.HashOptions.MD5;
            radioMD5.Checked = Options.CurrentOptions.HashOptions.MD5;
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
                        File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(_identicals, Formatting.Indented));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void GetActiveHash()
        {
            if (radioMD5.Checked)
            {
                ListIdenticalsByMD5();
                return;
            }

            if (radioSHA1.Checked)
            {
                ListIdenticalsBySHA1();
                return;
            }

            if (radioSHA256.Checked)
            {
                ListIdenticalsBySHA256();
                return;
            }

            if (radioSHA384.Checked)
            {
                ListIdenticalsBySHA384();
                return;
            }

            if (radioSHA512.Checked)
            {
                ListIdenticalsBySHA512();
                return;
            }

            if (radioCRC32.Checked)
            {
                ListIdenticalsByCRC32();
                return;
            }

            if (radioRIPEMD160.Checked)
            {
                ListIdenticalsByRIPEMD160();
                return;
            }

            if (radioSHA32.Checked)
            {
                ListIdenticalsBySHA3_256();
                return;
            }

            if (radioSHA33.Checked)
            {
                ListIdenticalsBySHA3_384();
                return;
            }

            if (radioSHA35.Checked)
            {
                ListIdenticalsBySHA3_512();
                return;
            }

            return;
        }

        private void ListIdenticalsByMD5()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.MD5)) _uniqueHashes.Add(sr.MD5);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.MD5 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA1()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA1)) _uniqueHashes.Add(sr.SHA1);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA1 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA256()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA256)) _uniqueHashes.Add(sr.SHA256);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA256 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA384()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA384)) _uniqueHashes.Add(sr.SHA384);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA384 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA512()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA512)) _uniqueHashes.Add(sr.SHA512);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA512 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsByCRC32()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.CRC32)) _uniqueHashes.Add(sr.CRC32);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.CRC32 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsByRIPEMD160()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.RIPEMD160)) _uniqueHashes.Add(sr.RIPEMD160);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.RIPEMD160 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA3_256()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA3_256)) _uniqueHashes.Add(sr.SHA3_256);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA3_256 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA3_384()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA3_384)) _uniqueHashes.Add(sr.SHA3_384);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA3_384 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

            SumView.ExpandAll();
        }

        private void ListIdenticalsBySHA3_512()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_uniqueHashes.Contains(sr.SHA3_512)) _uniqueHashes.Add(sr.SHA3_512);
            }

            foreach (string x in _uniqueHashes)
            {
                TreeNode rootNode = new TreeNode(x);
                rootNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in _identicals)
                {
                    if (y.SHA3_512 == x)
                    {
                        rootNode.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(rootNode);
            }

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

        private void radioMD5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioMD5.Checked)
            {
                _helper = "MD5: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.MD5;
                        break;
                    }
                }
            }
        }

        private void radioSHA1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA1.Checked)
            {
                _helper = "SHA1: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA1;
                        break;
                    }
                }
            }
        }

        private void radioSHA256_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA256.Checked)
            {
                _helper = "SHA256: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA256;
                        break;
                    }
                }
            }
        }

        private void radioRIPEMD160_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRIPEMD160.Checked)
            {
                _helper = "RIPEMD160: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.RIPEMD160;
                        break;
                    }
                }
            }
        }

        private void radioSHA384_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA384.Checked)
            {
                _helper = "SHA384: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA384;
                        break;
                    }
                }
            }
        }

        private void radioSHA512_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA512.Checked)
            {
                _helper = "SHA512: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA512;
                        break;
                    }
                }
            }
        }

        private void radioCRC32_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCRC32.Checked)
            {
                _helper = "CRC32: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.CRC32;
                        break;
                    }
                }
            }
        }

        private void radioSHA32_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA32.Checked)
            {
                _helper = "SHA3-256: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA3_256;
                        break;
                    }
                }
            }
        }

        private void radioSHA33_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA33.Checked)
            {
                _helper = "SHA3-384: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA3_384;
                        break;
                    }
                }
            }
        }

        private void radioSHA35_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSHA35.Checked)
            {
                _helper = "SHA3-512: ";

                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = _identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA3_512;
                        break;
                    }
                }
            }
        }
    }
}
