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
        }

        private void CompareForm_Load(object sender, EventArgs e)
        {

        }

        private void Compare()
        {
            if (!string.IsNullOrEmpty(txtExpected.Text))
            {
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

                    if (chkRIPEMD160.Checked)
                    {
                        if (x.RIPEMD160 == txtExpected.Text) resultBox.Items.Add(x.File);
                    }
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
    }
}
