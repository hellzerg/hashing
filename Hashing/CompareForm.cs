using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            txtExpected.Select();
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
    }
}
