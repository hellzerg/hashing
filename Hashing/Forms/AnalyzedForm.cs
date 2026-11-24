using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class AnalyzedForm : Form
    {
        public AnalyzedForm(IEnumerable<string> files)
        {
            InitializeComponent();

            Options.ApplyTheme(this);
            listFiles.Font = Fluent.Ui(10f);

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
