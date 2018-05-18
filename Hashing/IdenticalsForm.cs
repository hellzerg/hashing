using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace Hashing
{
    public partial class IdenticalsForm : Form
    {
        List<SumResult> _identicals;
        List<string> _md5s = new List<string>();

        string _helper = string.Empty;

        public IdenticalsForm(List<SumResult> list)
        {
            InitializeComponent();
            Options.ApplyTheme(this);
            helperMenu.Renderer = new ToolStripRendererMaterial();

            _identicals = list;
            ListIdenticals();

            ConfigureGUI();
        }

        private void ConfigureGUI()
        {
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

        private void ListIdenticals()
        {
            foreach (SumResult sr in _identicals)
            {
                if (!_md5s.Contains(sr.MD5)) _md5s.Add(sr.MD5);
            }

            foreach (string x in _md5s)
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
}
