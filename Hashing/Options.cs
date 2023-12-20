using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Hashing
{
    [Serializable]
    public class SettingsJson
    {
        public Theme Color { get; set; } = Theme.Amethyst;
        public HashOptions HashOptions { get; set; } = new HashOptions();
        public bool LowerCasing { get; set; }
        public bool TrayIcon { get; set; }
        public bool HighPriority { get; set; }
        public bool CRC32Decimal { get; set; }
        public short ActiveHash { get; set; }
        public bool StayOnTop { get; set; }
        public bool SingleInstance { get; set; }

        public Size WindowSize { get; set; } = new Size(820, 632);
        public Point? WindowLocation { get; set; }
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;

        public LanguageCode LanguageCode { get; set; }
    }

    public class Options
    {
        internal static Color ForegroundColor;
        internal static Color ForegroundAccentColor;
        internal static Color BackgroundColor = Color.FromArgb(20, 20, 20);
        internal static SettingsJson CurrentOptions = new SettingsJson();
        internal static dynamic TranslationList;

        internal static string ThemeFlag = "ThemeTag";

        private static readonly Dictionary<Theme, (Color, Color)> ThemeColors = new Dictionary<Theme, (Color, Color)>
        {
            { Theme.Amber, (Color.FromArgb(195, 146, 0), Color.FromArgb(171, 128, 0)) },
            { Theme.Jade, (Color.FromArgb(70, 175, 105), Color.FromArgb(61, 153, 92)) },
            { Theme.Ruby, (Color.FromArgb(205, 22, 39), Color.FromArgb(155, 17, 30)) },
            { Theme.Silver, (Color.Gray, Color.DimGray) },
            { Theme.Azurite, (Color.FromArgb(0, 127, 255), Color.FromArgb(0, 111, 223)) },
            { Theme.Amethyst, (Color.FromArgb(153, 102, 204), Color.FromArgb(134, 89, 179)) },
        };

        internal static IEnumerable<Control> GetSelfAndChildrenRecursive(Control parent)
        {
            return parent.Controls.Cast<Control>().SelectMany(GetSelfAndChildrenRecursive).Append(parent);
        }

        internal static void ApplyTheme(Form f)
        {
            if (ThemeColors.TryGetValue(CurrentOptions.Color, out var colors))
            {
                SetTheme(f, colors.Item1, colors.Item2);
            }
        }

        private static void SetTheme(Form f, Color c1, Color c2)
        {
            dynamic c;
            ForegroundColor = c1;
            ForegroundAccentColor = c2;
            GetSelfAndChildrenRecursive(f).ToList().ForEach(x =>
            {
                c = x;
                if (x is Button)
                {
                    c.BackColor = c1;
                    c.FlatAppearance.BorderColor = c1;
                    c.FlatAppearance.MouseDownBackColor = c2;
                    c.FlatAppearance.MouseOverBackColor = c2;
                    c.FlatAppearance.BorderSize = 0;
                }
                if (x is Label || x is CheckBox || x is RadioButton)
                {
                    if ((string)c.Tag == ThemeFlag)
                    {
                        c.ForeColor = c1;
                    }
                }
                if (x is LinkLabel)
                {
                    if ((string)c.Tag == ThemeFlag)
                    {
                        c.LinkColor = c1;
                        c.VisitedLinkColor = c1;
                        c.ActiveLinkColor = c2;
                    }
                }
                c.Invalidate();
            });
        }

        internal static void SaveSettings()
        {
            string settingsFile = GetSettingsFilePath();
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(CurrentOptions, Formatting.Indented));
        }

        internal static void LoadSettings()
        {
            string settingsFile = GetSettingsFilePath();
            if (File.Exists(settingsFile))
            {
                CurrentOptions = JsonConvert.DeserializeObject<SettingsJson>(File.ReadAllText(settingsFile));
            }
            else
            {
                SaveSettings();
            }
            LoadTranslation();
        }

        private static string GetSettingsFilePath()
        {
            return Path.Combine(Application.StartupPath, "Hashing.json");
        }

        internal static void LoadTranslation()
        {
            var languageResources = new Dictionary<LanguageCode, string>
    {
        { LanguageCode.EN, Properties.Resources.EN },
        { LanguageCode.EL, Properties.Resources.EL },
        { LanguageCode.CN, Properties.Resources.CN },
        { LanguageCode.DE, Properties.Resources.DE },
    };

            if (languageResources.TryGetValue(CurrentOptions.LanguageCode, out var resource))
            {
                TranslationList = JObject.Parse(resource);
            }
            else
            {
                TranslationList = JObject.Parse(Properties.Resources.EN);
            }
        }

    }
}
