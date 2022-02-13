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
        public Theme Color { get; set; }
        public HashOptions HashOptions { get; set; }
        public bool LowerCasing { get; set; }
        public bool TrayIcon { get; set; }
        public bool HighPriority { get; set; }
        public bool CRC32Decimal { get; set; }
        public short ActiveHash { get; set; }
        public bool StayOnTop { get; set; }
        public bool SingleInstance { get; set; }
        public Size WindowSize { get; set; }
        public Point? WindowLocation { get; set; }
        public FormWindowState WindowState { get; set; }

        public LanguageCode LanguageCode { get; set; }

        public SettingsJson()
        {
            HashOptions = new HashOptions();
        }
    }

    public class Options
    {
        internal static Color ForegroundColor = Color.FromArgb(153, 102, 204);
        internal static Color ForegroundAccentColor = Color.FromArgb(134, 89, 179);

        internal static Color BackgroundColor = Color.FromArgb(20, 20, 20);

        internal readonly static string ThemeFlag = "themeable";
        readonly static string SettingsFile = Application.StartupPath + "\\Hashing.json";

        internal static SettingsJson CurrentOptions = new SettingsJson();

        internal static dynamic TranslationList;

        internal static IEnumerable<Control> GetSelfAndChildrenRecursive(Control parent)
        {
            List<Control> controls = new List<Control>();

            foreach (Control child in parent.Controls)
            {
                controls.AddRange(GetSelfAndChildrenRecursive(child));
            }

            controls.Add(parent);
            return controls;
        }

        internal static void ApplyTheme(Form f)
        {
            switch (CurrentOptions.Color)
            {
                case Theme.Amber:
                    SetTheme(f, Color.FromArgb(195, 146, 0), Color.FromArgb(171, 128, 0));
                    break;
                case Theme.Jade:
                    SetTheme(f, Color.FromArgb(70, 175, 105), Color.FromArgb(61, 153, 92));
                    break;
                case Theme.Ruby:
                    SetTheme(f, Color.FromArgb(205, 22, 39), Color.FromArgb(155, 17, 30));
                    break;
                case Theme.Silver:
                    SetTheme(f, Color.Gray, Color.DimGray);
                    break;
                case Theme.Azurite:
                    SetTheme(f, Color.FromArgb(0, 127, 255), Color.FromArgb(0, 111, 223));
                    break;
                case Theme.Amethyst:
                    SetTheme(f, Color.FromArgb(153, 102, 204), Color.FromArgb(134, 89, 179));
                    break;
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
            if (File.Exists(SettingsFile))
            {
                File.Delete(SettingsFile);

                using (FileStream fs = File.Open(SettingsFile, FileMode.OpenOrCreate))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, CurrentOptions);
                }
            }
        }

        internal static void LoadSettings()
        {
            if (!File.Exists(SettingsFile))
            {
                CurrentOptions.Color = Theme.Amethyst;
                CurrentOptions.LowerCasing = false;
                CurrentOptions.TrayIcon = false;
                CurrentOptions.HighPriority = false;
                CurrentOptions.CRC32Decimal = false;
                CurrentOptions.ActiveHash = 1;
                CurrentOptions.StayOnTop = false;
                CurrentOptions.SingleInstance = true;

                CurrentOptions.HashOptions.MD5 = false;
                CurrentOptions.HashOptions.SHA1 = true;
                CurrentOptions.HashOptions.SHA256 = true;
                CurrentOptions.HashOptions.SHA384 = false;
                CurrentOptions.HashOptions.SHA512 = false;
                CurrentOptions.HashOptions.CRC32 = false;
                CurrentOptions.HashOptions.RIPEMD160 = false;
                CurrentOptions.HashOptions.SHA3_256 = false;
                CurrentOptions.HashOptions.SHA3_384 = false;
                CurrentOptions.HashOptions.SHA3_512 = false;

                CurrentOptions.WindowLocation = null;
                CurrentOptions.WindowSize = new Size(820, 632);
                CurrentOptions.WindowState = FormWindowState.Normal;

                using (FileStream fs = File.Open(SettingsFile, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, CurrentOptions);
                }
            }
            else
            {
                CurrentOptions = JsonConvert.DeserializeObject<SettingsJson>(File.ReadAllText(SettingsFile));

                if (CurrentOptions.WindowSize.IsEmpty)
                {
                    CurrentOptions.WindowSize = new Size(820, 632);
                    SaveSettings();
                }
            }

            LoadTranslation();
        }

        internal static void LoadTranslation()
        {
            // load proper translation list
            if (CurrentOptions.LanguageCode == LanguageCode.EN) TranslationList = JObject.Parse(Properties.Resources.EN);
            if (CurrentOptions.LanguageCode == LanguageCode.EL) TranslationList = JObject.Parse(Properties.Resources.EL);
            if (CurrentOptions.LanguageCode == LanguageCode.CN) TranslationList = JObject.Parse(Properties.Resources.CN);
        }
    }
}
