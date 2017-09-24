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
        List<SumResult> Identicals;
        List<string> MD5s = new List<string>();

        public IdenticalsForm(List<SumResult> list)
        {
            InitializeComponent();
            Options.ApplyTheme(this);
            radioButton1.Checked = true;
            Identicals = list;
            ListIdenticals();
            helperMenu.Renderer = new ToolStripRendererMaterial();
        }

        private void Copy()
        {
            string s = string.Empty;

            if (SumView.SelectedNode.Nodes.Count > 0)
            {
                s = SumView.SelectedNode.Text + Environment.NewLine + Environment.NewLine;

                foreach (TreeNode node in SumView.SelectedNode.Nodes)
                {
                    s += node.Text + Environment.NewLine;
                }
            }
            else
            {
                s = SumView.SelectedNode.Text;
            }

            Utilities.CopyToClipboard(s);
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
                        File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(Identicals, Formatting.Indented));
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
            foreach (SumResult sr in Identicals)
            {
                if (!MD5s.Contains(sr.MD5)) MD5s.Add(sr.MD5);
            }

            foreach (string x in MD5s)
            {
                TreeNode node = new TreeNode(x);
                node.ForeColor = Options.ForegroundColor;

                foreach (SumResult y in Identicals)
                {
                    if (y.MD5 == x)
                    {
                        node.Nodes.Add(y.File);
                    }
                }

                SumView.Nodes.Add(node);
            }

            SumView.ExpandAll();
        }

        private void CompareForm_Load(object sender, EventArgs e)
        {
            
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.MD5;
                        break; 
                    }
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA1;
                        break;
                    }
                }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA256;
                        break;
                    }
                }
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.RIPEMD160;
                        break;
                    }
                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA384;
                        break;
                    }
                }
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                SumResult tmp;
                foreach (TreeNode node in SumView.Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        tmp = Identicals.Find(x => x.File == child.Text);
                        node.Text = tmp.SHA512;
                        break;
                    }
                }
            }
        }
    }
}
