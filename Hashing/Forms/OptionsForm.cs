using System;
using System.Windows.Forms;

namespace Hashing
{
    public partial class OptionsForm : Form
    {
        MainForm _main;

        internal static bool HashesChanged;
        internal static bool CasingChanged;
        internal static bool CRC32FormatChanged;

        public OptionsForm(MainForm main)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            _main = main;

            LoadSettings();
            Options.ApplyTheme(this);

            Translate();

            HashesChanged = false;
            CasingChanged = false;
            CRC32FormatChanged = false;
        }
        private void Translate()
        {
            label27.Text = Options.TranslationList["label27"].ToString();
            lbl88.Text = Options.TranslationList["lbl88"].ToString();
            chkLower.Text = Options.TranslationList["chkLower"].ToString();
            chkCRCFormat.Text = Options.TranslationList["chkCRCFormat"].ToString();
            chkSingleInstance.Text = Options.TranslationList["chkSingleInstance"].ToString();
            chkStayOnTop.Text = Options.TranslationList["chkStayOnTop"].ToString();
            chkTray.Text = Options.TranslationList["chkTray"].ToString();
            chkHigh.Text = Options.TranslationList["chkHigh"].ToString();
            okbtn.Text = Options.TranslationList["button7"].ToString();
            label3.Text = Options.TranslationList["label3"].ToString();
            this.Text = Options.TranslationList["btnOptions"].ToString();
        }

        private void LoadSettings()
        {
            switch (Options.CurrentOptions.Color)
            {
                case Theme.Amber:
                    rAmber.Checked = true;
                    break;
                case Theme.Jade:
                    rJade.Checked = true;
                    break;
                case Theme.Ruby:
                    rRuby.Checked = true;
                    break;
                case Theme.Silver:
                    rSilver.Checked = true;
                    break;
                case Theme.Azurite:
                    rAzurite.Checked = true;
                    break;
                case Theme.Amethyst:
                    rAmethyst.Checked = true;
                    break;
            }

            chkMD5.Checked = Options.CurrentOptions.HashOptions.MD5;
            chkCRC32.Checked = Options.CurrentOptions.HashOptions.CRC32;
            chkRIPEMD160.Checked = Options.CurrentOptions.HashOptions.RIPEMD160;
            chkSHA1.Checked = Options.CurrentOptions.HashOptions.SHA1;
            chkSHA256.Checked = Options.CurrentOptions.HashOptions.SHA256;
            chkSHA384.Checked = Options.CurrentOptions.HashOptions.SHA384;
            chkSHA512.Checked = Options.CurrentOptions.HashOptions.SHA512;
            chkSHA32.Checked = Options.CurrentOptions.HashOptions.SHA3_256;
            chkSHA33.Checked = Options.CurrentOptions.HashOptions.SHA3_384;
            chkSHA35.Checked = Options.CurrentOptions.HashOptions.SHA3_512;

            chkLower.Checked = Options.CurrentOptions.LowerCasing;
            chkTray.Checked = Options.CurrentOptions.TrayIcon;
            chkHigh.Checked = Options.CurrentOptions.HighPriority;
            chkCRCFormat.Checked = Options.CurrentOptions.CRC32Decimal;
            chkStayOnTop.Checked = Options.CurrentOptions.StayOnTop;
            chkSingleInstance.Checked = Options.CurrentOptions.SingleInstance;

            if (Options.CurrentOptions.LanguageCode == LanguageCode.EN) radioEnglish.Checked = true;
            if (Options.CurrentOptions.LanguageCode == LanguageCode.EL) radioHellenic.Checked = true;
            if (Options.CurrentOptions.LanguageCode == LanguageCode.CN) radioChinese.Checked = true;
        }

        private void OptionsForm_Load(object sender, EventArgs e)
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

        private void okbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oceantheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Azurite;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void magmatheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Ruby;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void zergtheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Amethyst;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void carameltheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Amber;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void limetheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Jade;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void minimaltheme_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.Color = Theme.Silver;
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            _main.FixColor();
        }

        private void chkMD5_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.MD5 = chkMD5.Checked;
            HashesChanged = true;
        }

        private void chkSHA1_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA1 = chkSHA1.Checked;
            HashesChanged = true;
        }

        private void chkSHA256_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA256 = chkSHA256.Checked;
            HashesChanged = true;
        }

        private void chkRIPEMD160_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.RIPEMD160 = chkRIPEMD160.Checked;
            HashesChanged = true;
        }

        private void chkSHA512_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA512 = chkSHA512.Checked;
            HashesChanged = true;
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

        private void chkSHA384_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA384 = chkSHA384.Checked;
            HashesChanged = true;
        }

        private void chkCRC32_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.CRC32 = chkCRC32.Checked;
            HashesChanged = true;
        }

        private void chkLower_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.LowerCasing = chkLower.Checked;
            CasingChanged = true;
        }

        private void chkCRCFormat_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.CRC32Decimal = chkCRCFormat.Checked;
            CRC32FormatChanged = true;
        }

        private void chkSingleInstance_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.SingleInstance = chkSingleInstance.Checked;
            Options.SaveSettings();
        }

        private void chkStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.StayOnTop = chkStayOnTop.Checked;
        }

        private void radioEnglish_Click(object sender, EventArgs e)
        {
            radioEnglish.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.EN;
            Options.SaveSettings();
            Options.LoadTranslation();
            _main.Translate();
            Translate();
        }

        private void radioHellenic_Click(object sender, EventArgs e)
        {
            radioHellenic.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.EL;
            Options.SaveSettings();
            Options.LoadTranslation();
            _main.Translate();
            Translate();
        }

        private void radioChinese_Click(object sender, EventArgs e)
        {
            radioChinese.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.CN;
            Options.SaveSettings();
            Options.LoadTranslation();
            _main.Translate();
            Translate();
        }

        private void pictureBox86_Click(object sender, EventArgs e)
        {
            radioEnglish.PerformClick();
        }

        private void pictureBox88_Click(object sender, EventArgs e)
        {
            radioHellenic.PerformClick();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            radioChinese.PerformClick();
        }

        private void chkSHA32_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA3_256 = chkSHA32.Checked;
            HashesChanged = true;
        }

        private void chkSHA33_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA3_384 = chkSHA33.Checked;
            HashesChanged = true;
        }

        private void chkSHA35_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.HashOptions.SHA3_512 = chkSHA35.Checked;
            HashesChanged = true;
        }
    }
}
