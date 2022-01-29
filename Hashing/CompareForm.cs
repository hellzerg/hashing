using System;
using System.Windows.Forms;

namespace Hashing
{
    public partial class CompareForm : Form
    {
        public CompareForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
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
            switch (Options.CurrentOptions.ActiveHash)
            {
                case 1:
                    chkMD5.Checked = true;
                    break;
                case 2:
                    chkSHA1.Checked = true;
                    break;
                case 3:
                    chkSHA256.Checked = true;
                    break;
                case 4:
                    chkSHA384.Checked = true;
                    break;
                case 5:
                    chkSHA512.Checked = true;
                    break;
                case 6:
                    chkCRC32.Checked = true;
                    break;
                case 7:
                    chkRIPEMD160.Checked = true;
                    break;
            }
        }

        private void Compare()
        {
            if (!string.IsNullOrEmpty(txtExpected.Text))
            {
                txtExpected.Text.Trim();
                resultBox.Items.Clear();

                foreach (SumResult x in SumResult.Sums)
                {
                    if (chkMD5.Checked)
                    {
                        if (x.MD5 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkSHA1.Checked)
                    {
                        if (x.SHA1 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkSHA256.Checked)
                    {
                        if (x.SHA256 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkSHA384.Checked)
                    {
                        if (x.SHA384 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkSHA512.Checked)
                    {
                        if (x.SHA512 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkCRC32.Checked)
                    {
                        if (x.CRC32 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }

                    if (chkRIPEMD160.Checked)
                    {
                        if (x.RIPEMD160 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }
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
    }
}
