using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class OptionsForm : Form
    {
        private readonly MainForm _main;
        private FluentLabel _appearanceLabel;
        private FluentLabel _generalSettingsLabel;
        private MoonCheck chkExplorerContextMenu;
        private Card _hashesCard;
        private Card _generalSettingsCard;
        private MoonRadio _appearanceSystem;
        private MoonRadio _appearanceLight;
        private MoonRadio _appearanceDark;
        private bool _isInitializing;
        private bool _settingsSaveWarningShown;

        internal static bool HashesChanged;
        internal static bool CasingChanged;
        internal static bool CRC32FormatChanged;

        public OptionsForm(MainForm main)
        {
            InitializeComponent();
            _main = main;
            _isInitializing = true;

            HashesChanged = false;
            CasingChanged = false;
            CRC32FormatChanged = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _isInitializing = true;
            try
            {
                BuildModernLayout();
                LoadSettings();
                Translate();
            }
            finally
            {
                _isInitializing = false;
            }

            Options.ApplyTheme(this);

            HashesChanged = false;
            CasingChanged = false;
            CRC32FormatChanged = false;
        }

        private void SaveSettingsSafely()
        {
            string error;
            if (Options.TrySaveSettings(out error))
            {
                _settingsSaveWarningShown = false;
                return;
            }

            if (_settingsSaveWarningShown)
                return;

            _settingsSaveWarningShown = true;
            FluentMessageBox.Show(this,
                Options.TranslationList["settingsSaveFailed"].ToString().Replace("{ERROR}", error),
                Options.TranslationList["fileOperationErrorsTitle"].ToString(),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            _appearanceLabel.Text = Options.TranslationList["labelAppearance"].ToString();
            _generalSettingsLabel.Text = Options.TranslationList["labelSettings"].ToString();
            chkExplorerContextMenu.Text = Options.TranslationList["chkExplorerContextMenu"].ToString();
            _appearanceSystem.Text = Options.TranslationList["appearanceSystem"].ToString();
            _appearanceLight.Text = Options.TranslationList["appearanceLight"].ToString();
            _appearanceDark.Text = Options.TranslationList["appearanceDark"].ToString();
        }

        private int S(int value)
        {
            return Fluent.Scale(this, value);
        }

        private void SetLogicalBounds(Control control, int x, int y, int width, int height)
        {
            control.SetBounds(S(x), S(y), S(width), S(height));
        }

        private void BuildModernLayout()
        {
            ClientSize = new Size(S(780), S(550));
            MinimumSize = new Size(S(796), S(589));

            SetLogicalBounds(panel1, 16, 14, 358, 196);
            SetLogicalBounds(panel2, 398, 14, 366, 196);
            panel1.BorderStyle = BorderStyle.None;
            panel2.BorderStyle = BorderStyle.None;

            _hashesCard = new Card { Name = "hashesCard", Radius = 10 };
            _generalSettingsCard = new Card { Name = "generalSettingsCard", Radius = 10 };
            SetLogicalBounds(_hashesCard, 16, 224, 358, 258);
            SetLogicalBounds(_generalSettingsCard, 398, 224, 366, 258);
            Controls.Add(_hashesCard);
            Controls.Add(_generalSettingsCard);

            label27.AutoSize = false;
            label27.Font = Fluent.UiSemibold(11f);
            label27.TextAlign = ContentAlignment.MiddleLeft;
            label27.Tag = Options.ThemeFlag;
            SetLogicalBounds(label27, 12, 10, 330, 28);

            MoonRadio[] accentRadios = { rAmethyst, rAmber, rRuby, rJade, rAzurite, rSilver };
            foreach (MoonRadio radio in accentRadios)
            {
                radio.AutoSize = false;
                radio.Font = Fluent.Ui(9.5f);
                radio.Style = RadioStyle.Swatch;
                radio.UseVisualStyleBackColor = false;
            }

            SetLogicalBounds(rAmethyst, 18, 42, 150, 30);
            SetLogicalBounds(rAmber, 184, 42, 150, 30);
            SetLogicalBounds(rRuby, 18, 74, 150, 30);
            SetLogicalBounds(rJade, 184, 74, 150, 30);
            SetLogicalBounds(rAzurite, 18, 106, 150, 30);
            SetLogicalBounds(rSilver, 184, 106, 150, 30);

            rAmethyst.SwatchColor = Color.FromArgb(153, 102, 204);
            rAmber.SwatchColor = Color.FromArgb(195, 146, 0);
            rRuby.SwatchColor = Color.FromArgb(205, 22, 39);
            rJade.SwatchColor = Color.FromArgb(70, 175, 105);
            rAzurite.SwatchColor = Color.FromArgb(0, 127, 255);
            rSilver.SwatchColor = Color.Gray;

            _appearanceLabel = new FluentLabel
            {
                AutoSize = false,
                Location = new Point(S(12), S(149)),
                Size = new Size(S(88), S(30)),
                Font = Fluent.Ui(9.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Text = "Appearance"
            };

            Panel appearanceGroup = new Panel
            {
                Name = "appearanceGroup",
                BackColor = Color.Transparent,
                Location = new Point(S(104), S(147)),
                Size = new Size(S(242), S(34))
            };

            _appearanceSystem = CreateAppearanceOption("appearanceSystem", "System", 0, 82);
            _appearanceLight = CreateAppearanceOption("appearanceLight", "Light", 82, 80);
            _appearanceDark = CreateAppearanceOption("appearanceDark", "Dark", 162, 80);
            appearanceGroup.Controls.AddRange(new Control[] { _appearanceSystem, _appearanceLight, _appearanceDark });
            panel1.Controls.Add(_appearanceLabel);
            panel1.Controls.Add(appearanceGroup);

            label3.AutoSize = false;
            label3.Font = Fluent.UiSemibold(11f);
            label3.TextAlign = ContentAlignment.MiddleLeft;
            label3.Tag = Options.ThemeFlag;
            SetLogicalBounds(label3, 12, 10, 340, 28);

            MoonRadio[] languageRadios = { radioEnglish, radioHellenic, radioChinese, radioGerman };
            for (int i = 0; i < languageRadios.Length; i++)
            {
                MoonRadio radio = languageRadios[i];
                radio.AutoSize = false;
                radio.Font = Fluent.Ui(10f);
                radio.UseVisualStyleBackColor = false;
                SetLogicalBounds(radio, 68, 48 + i * 32, 270, 30);
            }

            SetLogicalBounds(pictureBox86, 27, 52, 36, 22);
            SetLogicalBounds(pictureBox88, 27, 84, 36, 22);
            SetLogicalBounds(pictureBox7, 27, 116, 36, 22);
            SetLogicalBounds(pictureBox1, 27, 148, 36, 22);

            lbl88.AutoSize = false;
            lbl88.Font = Fluent.UiSemibold(11f);
            lbl88.TextAlign = ContentAlignment.MiddleLeft;
            lbl88.Tag = Options.ThemeFlag;
            SetLogicalBounds(lbl88, 14, 10, 330, 28);
            _hashesCard.Controls.Add(lbl88);

            _generalSettingsLabel = new FluentLabel
            {
                AutoSize = false,
                Location = new Point(S(14), S(10)),
                Size = new Size(S(338), S(28)),
                Font = Fluent.UiSemibold(11f),
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = Options.ThemeFlag,
                Text = "General settings"
            };
            _generalSettingsCard.Controls.Add(_generalSettingsLabel);

            chkExplorerContextMenu = new MoonCheck
            {
                Name = "chkExplorerContextMenu",
                AutoSize = false,
                Text = Options.TranslationList["chkExplorerContextMenu"].ToString(),
                Style = CheckStyle.Switch
            };
            chkExplorerContextMenu.CheckedChanged += chkExplorerContextMenu_CheckedChanged;

            Control[] firstHashColumn = { chkMD5, chkSHA1, chkSHA384, chkSHA32, chkSHA35 };
            Control[] secondHashColumn = { chkCRC32, chkSHA256, chkSHA512, chkSHA33, chkRIPEMD160 };
            for (int row = 0; row < 5; row++)
            {
                Control left = firstHashColumn[row];
                left.AutoSize = false;
                left.Font = Fluent.Ui(9.5f);
                left.SetBounds(S(18), S(48 + row * 32), S(150), S(30));
                _hashesCard.Controls.Add(left);

                Control right = secondHashColumn[row];
                right.AutoSize = false;
                right.Font = Fluent.Ui(9.5f);
                right.SetBounds(S(178), S(48 + row * 32), S(160), S(30));
                _hashesCard.Controls.Add(right);
            }

            Control[] settingsControls = { chkLower, chkCRCFormat, chkSingleInstance, chkStayOnTop, chkTray, chkHigh, chkExplorerContextMenu };
            for (int i = 0; i < settingsControls.Length; i++)
            {
                Control control = settingsControls[i];
                control.AutoSize = false;
                control.SetBounds(S(16), S(48 + i * 28), S(334), S(26));
                control.Font = Fluent.Ui(9.5f);
                MoonCheck toggle = control as MoonCheck;
                if (toggle != null) toggle.Style = CheckStyle.Switch;
                _generalSettingsCard.Controls.Add(control);
            }

            SetLogicalBounds(okbtn, 612, 500, 152, 34);
            okbtn.Tag = "primary";
        }

        private MoonRadio CreateAppearanceOption(string name, string text, int x, int width)
        {
            MoonRadio option = new MoonRadio
            {
                Name = name,
                AutoSize = false,
                Location = new Point(S(x), 0),
                Size = new Size(S(width), S(32)),
                Style = RadioStyle.Segment,
                Font = Fluent.Ui(9.5f),
                Text = text
            };
            option.CheckedChanged += appearance_CheckedChanged;
            return option;
        }

        private void appearance_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            if (_isInitializing || radio == null || !radio.Checked)
                return;

            Options.CurrentOptions.Appearance = _appearanceSystem.Checked ? AppearanceMode.System
                : _appearanceLight.Checked ? AppearanceMode.Light
                : AppearanceMode.Dark;

            Options.InitializePalette();
            try
            {
                Options.ApplyTheme(this);
                Options.ApplyTheme(_main);
            }
            finally
            {
                Fluent.CompletePaletteChange();
            }

            _main.FixColor();
            SaveSettingsSafely();
        }

        private void ChangeAccentTheme(Theme theme, object sender)
        {
            if (_isInitializing)
                return;

            RadioButton radio = sender as RadioButton;
            if (radio != null && !radio.Checked)
                return;

            Options.CurrentOptions.Color = theme;
            Options.InitializePalette();
            Options.ApplyTheme(this);
            Options.ApplyTheme(_main);
            Fluent.CompletePaletteChange();
            _main.FixColor();
            SaveSettingsSafely();
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

            _appearanceSystem.Checked = Options.CurrentOptions.Appearance == AppearanceMode.System;
            _appearanceLight.Checked = Options.CurrentOptions.Appearance == AppearanceMode.Light;
            _appearanceDark.Checked = Options.CurrentOptions.Appearance == AppearanceMode.Dark;

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
            chkExplorerContextMenu.Checked = ShellIntegration.IsRegistered();

            if (Options.CurrentOptions.LanguageCode == LanguageCode.EN) radioEnglish.Checked = true;
            if (Options.CurrentOptions.LanguageCode == LanguageCode.EL) radioHellenic.Checked = true;
            if (Options.CurrentOptions.LanguageCode == LanguageCode.CN) radioChinese.Checked = true;
            if (Options.CurrentOptions.LanguageCode == LanguageCode.DE) radioGerman.Checked = true;

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
            ChangeAccentTheme(Theme.Azurite, sender);
        }

        private void magmatheme_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAccentTheme(Theme.Ruby, sender);
        }

        private void zergtheme_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAccentTheme(Theme.Amethyst, sender);
        }

        private void carameltheme_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAccentTheme(Theme.Amber, sender);
        }

        private void limetheme_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAccentTheme(Theme.Jade, sender);
        }

        private void minimaltheme_CheckedChanged(object sender, EventArgs e)
        {
            ChangeAccentTheme(Theme.Silver, sender);
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
            if (_isInitializing)
                return;

            bool requestedPriority = chkHigh.Checked;
            string error;
            ProcessPriorityClass priority = requestedPriority ? ProcessPriorityClass.High : ProcessPriorityClass.Normal;
            if (Utilities.TrySetProcessPriority(priority, out error))
            {
                Options.CurrentOptions.HighPriority = requestedPriority;
                return;
            }

            _isInitializing = true;
            try
            {
                chkHigh.Checked = Options.CurrentOptions.HighPriority;
            }
            finally
            {
                _isInitializing = false;
            }

            FluentMessageBox.Show(this,
                Options.TranslationList["highPriorityFailed"].ToString().Replace("{ERROR}", error),
                "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            SaveSettingsSafely();
        }

        private void chkExplorerContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || chkExplorerContextMenu == null)
                return;

            try
            {
                if (chkExplorerContextMenu.Checked)
                    ShellIntegration.RegisterContextMenu(Options.TranslationList["shellContextMenuVerb"].ToString());
                else
                    ShellIntegration.UnregisterContextMenu();
            }
            catch (Exception ex)
            {
                _isInitializing = true;
                try
                {
                    chkExplorerContextMenu.Checked = ShellIntegration.IsRegistered();
                }
                finally
                {
                    _isInitializing = false;
                }

                FluentMessageBox.Show(this,
                    Options.TranslationList["shellContextMenuFailed"].ToString().Replace("{ERROR}", ex.Message),
                    "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshExplorerContextMenuLabel()
        {
            if (!ShellIntegration.IsRegistered())
                return;

            try
            {
                ShellIntegration.RegisterContextMenu(Options.TranslationList["shellContextMenuVerb"].ToString());
            }
            catch (Exception ex)
            {
                FluentMessageBox.Show(this,
                    Options.TranslationList["shellContextMenuFailed"].ToString().Replace("{ERROR}", ex.Message),
                    "Hashing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            Options.CurrentOptions.StayOnTop = chkStayOnTop.Checked;
        }

        private void radioEnglish_Click(object sender, EventArgs e)
        {
            radioEnglish.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.EN;
            SaveSettingsSafely();
            Options.LoadTranslation();
            RefreshExplorerContextMenuLabel();
            _main.Translate();
            Translate();
        }

        private void radioHellenic_Click(object sender, EventArgs e)
        {
            radioHellenic.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.EL;
            SaveSettingsSafely();
            Options.LoadTranslation();
            RefreshExplorerContextMenuLabel();
            _main.Translate();
            Translate();
        }

        private void radioChinese_Click(object sender, EventArgs e)
        {
            radioChinese.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.CN;
            SaveSettingsSafely();
            Options.LoadTranslation();
            RefreshExplorerContextMenuLabel();
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

        private void radioGerman_Click(object sender, EventArgs e)
        {
            radioGerman.Checked = true;
            Options.CurrentOptions.LanguageCode = LanguageCode.DE;
            SaveSettingsSafely();
            Options.LoadTranslation();
            RefreshExplorerContextMenuLabel();
            _main.Translate();
            Translate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            radioGerman.PerformClick();
        }
    }
}
