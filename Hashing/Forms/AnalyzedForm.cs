using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hashing
{
    public partial class AnalyzedForm : Form
    {
        public AnalyzedForm(IEnumerable<string> files)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            Options.ApplyTheme(this);

            foreach (string x in files) listFiles.Items.Add("[✓] " + x);
        }

        private void AnalyzedForm_Load(object sender, EventArgs e)
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
    }
}
