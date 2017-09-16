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
    public partial class OptionsForm : Form
    {
        MainForm _main;

        public OptionsForm(MainForm main)
        {
            InitializeComponent();
            _main = main;

            LoadSettings();
            Options.ApplyTheme(this);
        }

        private void LoadSettings()
        {
            switch (Options.CurrentOptions.Color)
            {
                case Theme.Caramel:
                    carameltheme.Checked = true;
                    break;
                case Theme.Lime:
                    limetheme.Checked = true;
                    break;
                case Theme.Magma:
                    magmatheme.Checked = true;
                    break;
                case Theme.Minimal:
                    minimaltheme.Checked = true;
                    break;
                case Theme.Ocean:
                    oceantheme.Checked = true;
                    break;
                case Theme.Zerg:
                    zergtheme.Checked = true;
                    break;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oceantheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Ocean;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void magmatheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Magma;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void zergtheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Zerg;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void carameltheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Caramel;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void limetheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Lime;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void minimaltheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Minimal;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }
    }
}
