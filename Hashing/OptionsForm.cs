using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            chkMD5.Checked = Options.CurrentOptions.HashOptions.MD5;
            chkRIPEMD160.Checked = Options.CurrentOptions.HashOptions.RIPEMD160;
            chkSHA1.Checked = Options.CurrentOptions.HashOptions.SHA1;
            chkSHA256.Checked = Options.CurrentOptions.HashOptions.SHA256;
            chkSHA512.Checked = Options.CurrentOptions.HashOptions.SHA512;

            chkTray.Checked = Options.CurrentOptions.TrayIcon;
            chkHigh.Checked = Options.CurrentOptions.HighPriority;
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

        private void chkMD5_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.MD5 = chkMD5.Checked;
        }

        private void chkSHA1_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA1 = chkSHA1.Checked;
        }

        private void chkSHA256_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA256 = chkSHA256.Checked;
        }

        private void chkRIPEMD160_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.RIPEMD160 = chkRIPEMD160.Checked;
        }

        private void chkSHA512_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA512 = chkSHA512.Checked;
        }

        private void chkTray_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.TrayIcon = chkTray.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HighPriority = chkHigh.Checked;
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Options.CurrentOptions.HighPriority)
            {
                Utilities.EnableHighPriority();
            }
            else
            {
                Utilities.DisableHighPriority();
            }
        }
    }
}
