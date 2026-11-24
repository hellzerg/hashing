using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class IdenticalsForm : Form
    {
        readonly List<SumResult> _identicals;

        string _helper = string.Empty;

        public IdenticalsForm(List<SumResult> list)
        {
            InitializeComponent();
            BuildModernLayout();
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


        private void BuildModernLayout()
        {
            ClientSize = new System.Drawing.Size(920, 590);
            MinimumSize = new System.Drawing.Size(820, 560);
            panel1.Height = 48;
            panel1.BorderStyle = BorderStyle.None;
            boxSelectHash.SetBounds((panel1.ClientSize.Width - 220) / 2, 7, 220, 32);
            boxSelectHash.Anchor = AnchorStyles.Top;
            panel2.Padding = new Padding(12);
            panel2.BorderStyle = BorderStyle.None;
            SumView.BorderStyle = BorderStyle.None;
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
            TreeNode selectedNode = SumView.SelectedNode;
            if (selectedNode == null)
                return;

            if (selectedNode.Nodes.Count == 0)
            {
                Utilities.CopyToClipboard(selectedNode.Text, true);
                return;
            }

            System.Text.StringBuilder text = new System.Text.StringBuilder();
            text.Append(_helper).Append(selectedNode.Text).AppendLine().AppendLine();
            foreach (TreeNode node in selectedNode.Nodes)
                text.AppendLine(node.Text);

            Utilities.CopyToClipboard(text.ToString(), true);
        }

        private void SaveJson()
        {
            if (SumView.Nodes.Count == 0)
                return;

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "JSON file|*.json";
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

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

        private void ListIdenticals(HashCode algo)
        {
            Func<SumResult, string> getHash = GetHashSelector(algo);
            if (getHash == null)
                return;

            List<TreeNode> nodes = new List<TreeNode>();
            foreach (IGrouping<string, SumResult> group in _identicals
                .Where(item => !string.IsNullOrEmpty(getHash(item)))
                .GroupBy(getHash, StringComparer.OrdinalIgnoreCase)
                .Where(group => group.Skip(1).Any()))
            {
                TreeNode hashNode = new TreeNode(group.Key);
                hashNode.ForeColor = Options.ForegroundColor;

                foreach (SumResult item in group)
                    hashNode.Nodes.Add(item.File);

                nodes.Add(hashNode);
            }

            SumView.BeginUpdate();
            try
            {
                SumView.Nodes.Clear();
                SumView.Nodes.AddRange(nodes.ToArray());
                SumView.ExpandAll();
            }
            finally
            {
                SumView.EndUpdate();
            }
        }

        private static Func<SumResult, string> GetHashSelector(HashCode algorithm)
        {
            switch (algorithm)
            {
                case HashCode.MD5: return item => item.MD5;
                case HashCode.SHA1: return item => item.SHA1;
                case HashCode.SHA256: return item => item.SHA256;
                case HashCode.SHA384: return item => item.SHA384;
                case HashCode.SHA512: return item => item.SHA512;
                case HashCode.RIPEMD160: return item => item.RIPEMD160;
                case HashCode.CRC32: return item => item.CRC32;
                case HashCode.SHA3_256: return item => item.SHA3_256;
                case HashCode.SHA3_384: return item => item.SHA3_384;
                case HashCode.SHA3_512: return item => item.SHA3_512;
                default: return null;
            }
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
