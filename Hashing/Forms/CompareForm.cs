using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class CompareForm : Form
    {
        InputBox _expectedInput;
        public CompareForm()
        {
            InitializeComponent();
            BuildModernLayout();
            lblNoMatch.Tag = "secondary";
            WrapExpectedInput();
            btnCompare.Tag = "primary";
            Options.ApplyTheme(this);

            label1.Text = Options.TranslationList["label1"].ToString();
            btnPaste.Text = Options.TranslationList["btnPaste"].ToString();
            btnCompare.Text = Options.TranslationList["btnCompare"].ToString();
            chkRemove.Text = Options.TranslationList["chkRemove"].ToString();
            label2.Text = Options.TranslationList["label2"].ToString();
            lblNoMatch.Text = Options.TranslationList["lblNoMatch"].ToString();
            this.Text = Options.TranslationList["btnCompare"].ToString();

            LoadActiveHash();

            KeyPreview = true;

            if (Options.CurrentOptions.LowerCasing)
            {
                txtExpected.CharacterCasing = CharacterCasing.Lower;
            }
            else
            {
                txtExpected.CharacterCasing = CharacterCasing.Upper;
            }
        }


        private void BuildModernLayout()
        {
            ClientSize = new System.Drawing.Size(920, 590);
            MinimumSize = new System.Drawing.Size(800, 560);
            panel1.Height = 66;
            panel1.BorderStyle = BorderStyle.None;

            Control[] firstRow = { chkMD5, chkSHA1, chkSHA256, chkSHA384, chkSHA512 };
            Control[] secondRow = { chkCRC32, chkRIPEMD160, chkSHA32, chkSHA33, chkSHA35 };
            LayoutAlgorithmRow(firstRow, 7);
            LayoutAlgorithmRow(secondRow, 35);

            label1.Location = new System.Drawing.Point(16, 80);
            txtExpected.SetBounds(16, 106, ClientSize.Width - 32, 34);
            txtExpected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtExpected.Font = Fluent.Ui(11f);
            chkRemove.Location = new System.Drawing.Point(16, 151);
            btnCompare.SetBounds(ClientSize.Width - 168, 146, 152, 36);
            btnPaste.SetBounds(ClientSize.Width - 328, 146, 152, 36);
            btnCompare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPaste.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.Location = new System.Drawing.Point(16, 192);
            panel2.SetBounds(16, 218, ClientSize.Width - 32, ClientSize.Height - 234);
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BorderStyle = BorderStyle.None;
            panel2.Padding = new Padding(4);
            resultBox.Font = Fluent.Mono(9.5f);
            lblNoMatch.Font = Fluent.Ui(12f);
            lblNoMatch.Visible = false;
        }


        private void WrapExpectedInput()
        {
            Control parent = txtExpected.Parent;
            if (parent == null) return;

            Rectangle bounds = txtExpected.Bounds;
            AnchorStyles anchor = txtExpected.Anchor;
            parent.Controls.Remove(txtExpected);
            _expectedInput = new InputBox
            {
                Name = "expectedHashInput",
                Location = bounds.Location,
                Size = bounds.Size,
                Anchor = anchor
            };
            _expectedInput.SetEditor(txtExpected);
            txtExpected.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            parent.Controls.Add(_expectedInput);
            _expectedInput.BringToFront();
            txtExpected.Font = Fluent.Mono(10f);
        }

        private void LayoutAlgorithmRow(Control[] controls, int y)
        {
            const int gap = 20;
            int totalWidth = gap * (controls.Length - 1);

            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].Font = Fluent.Ui(9f);
                totalWidth += controls[i].GetPreferredSize(System.Drawing.Size.Empty).Width;
            }

            int x = Math.Max(12, (panel1.ClientSize.Width - totalWidth) / 2);
            for (int i = 0; i < controls.Length; i++)
            {
                int width = controls[i].GetPreferredSize(System.Drawing.Size.Empty).Width;
                controls[i].SetBounds(x, y, width, 24);
                x += width + gap;
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

            txtExpected.Select();
        }

        private void LoadActiveHash()
        {
            //switch (Options.CurrentOptions.ActiveHash)
            //{
            //    case 1:
            //        chkMD5.Checked = true;
            //        break;
            //    case 2:
            //        chkSHA1.Checked = true;
            //        break;
            //    case 3:
            //        chkSHA256.Checked = true;
            //        break;
            //    case 4:
            //        chkSHA384.Checked = true;
            //        break;
            //    case 5:
            //        chkSHA512.Checked = true;
            //        break;
            //    case 6:
            //        chkCRC32.Checked = true;
            //        break;
            //    case 7:
            //        chkRIPEMD160.Checked = true;
            //        break;
            //}

            chkSHA35.Checked = Options.CurrentOptions.HashOptions.SHA3_512;
            chkSHA33.Checked = Options.CurrentOptions.HashOptions.SHA3_384;
            chkSHA32.Checked = Options.CurrentOptions.HashOptions.SHA3_256;
            chkRIPEMD160.Checked = Options.CurrentOptions.HashOptions.RIPEMD160;
            chkSHA512.Checked = Options.CurrentOptions.HashOptions.SHA512;
            chkSHA384.Checked = Options.CurrentOptions.HashOptions.SHA384;
            chkSHA256.Checked = Options.CurrentOptions.HashOptions.SHA256;
            chkSHA1.Checked = Options.CurrentOptions.HashOptions.SHA1;
            chkCRC32.Checked = Options.CurrentOptions.HashOptions.CRC32;
            chkMD5.Checked = Options.CurrentOptions.HashOptions.MD5;
        }

        private void Compare()
        {
            string expected = txtExpected.Text.Trim();
            resultBox.Items.Clear();
            if (expected.Length == 0)
            {
                lblNoMatch.Visible = false;
                return;
            }

            HashSet<string> addedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (SumResult x in SumResult.Sums)
            {
                bool match =
                    (chkMD5.Checked && string.Equals(x.MD5, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA1.Checked && string.Equals(x.SHA1, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA256.Checked && string.Equals(x.SHA256, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA384.Checked && string.Equals(x.SHA384, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA512.Checked && string.Equals(x.SHA512, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkCRC32.Checked && string.Equals(x.CRC32, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkRIPEMD160.Checked && string.Equals(x.RIPEMD160, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA32.Checked && string.Equals(x.SHA3_256, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA33.Checked && string.Equals(x.SHA3_384, expected, StringComparison.OrdinalIgnoreCase)) ||
                    (chkSHA35.Checked && string.Equals(x.SHA3_512, expected, StringComparison.OrdinalIgnoreCase));

                if (match && addedFiles.Add(x.File))
                    resultBox.Items.Add(x.File);
            }

            if (resultBox.Items.Count > 0)
            {
                lblNoMatch.Visible = false;
            }
            else
            {
                lblNoMatch.Visible = true;
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            Compare();
        }

        private void chkRemove_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemove.Checked)
            {
                txtExpected.Text = txtExpected.Text.Replace("-", string.Empty).Trim();
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Clipboard.GetText()))
                {
                    txtExpected.Text = Clipboard.GetText().Trim();
                }
            }
            catch { }
            finally
            {
                txtExpected.SelectionStart = txtExpected.Text.Length;
                txtExpected.Select();
            }
        }

        private void CompareForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void chkMD5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMD5.Checked) Options.CurrentOptions.ActiveHash = 1;
        }

        private void chkSHA1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA1.Checked) Options.CurrentOptions.ActiveHash = 2;
        }

        private void chkSHA256_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA256.Checked) Options.CurrentOptions.ActiveHash = 3;
        }

        private void chkSHA384_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA384.Checked) Options.CurrentOptions.ActiveHash = 4;
        }

        private void chkSHA512_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA512.Checked) Options.CurrentOptions.ActiveHash = 5;
        }

        private void chkCRC32_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCRC32.Checked) Options.CurrentOptions.ActiveHash = 6;
        }

        private void chkRIPEMD160_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRIPEMD160.Checked) Options.CurrentOptions.ActiveHash = 7;
        }

        private void chkSHA32_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA32.Checked) Options.CurrentOptions.ActiveHash = 8;
        }

        private void chkSHA33_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA33.Checked) Options.CurrentOptions.ActiveHash = 9;
        }

        private void chkSHA35_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSHA35.Checked) Options.CurrentOptions.ActiveHash = 10;
        }

        private void resultBox_DoubleClick(object sender, EventArgs e)
        {
            if (resultBox.SelectedIndex > -1)
            {
                Utilities.FindFile(resultBox.SelectedItem.ToString());
            }
        }

        private void txtExpected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCompare.PerformClick();
            }
        }
    }
}
