using Newtonsoft.Json;
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

        public SettingsJson()
        {
            HashOptions = new HashOptions();
        }
    }

    public class Options
    {
        internal static Color ForegroundColor = Color.MediumOrchid;
        internal static Color ForegroundAccentColor = Color.DarkOrchid;

        internal static Color BackgroundColor = Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));

        internal readonly static string ThemeFlag = "themeable";
        readonly static string SettingsFile = Application.StartupPath + "\\Hashing.json";

        internal static SettingsJson CurrentOptions = new SettingsJson();

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
                case Theme.Caramel:
                    SetTheme(f, Color.DarkOrange, Color.Chocolate);
                    break;
                case Theme.Lime:
                    SetTheme(f, Color.LimeGreen, Color.ForestGreen);
                    break;
                case Theme.Magma:
                    SetTheme(f, Color.Tomato, Color.Red);
                    break;
                case Theme.Minimal:
                    SetTheme(f, Color.Gray, Color.DimGray);
                    break;
                case Theme.Ocean:
                    SetTheme(f, Color.DodgerBlue, Color.RoyalBlue);
                    break;
                case Theme.Zerg:
                    SetTheme(f, Color.MediumOrchid, Color.DarkOrchid);
                    break;
            }
        }

        private static void SetTheme(Form f, Color c1, Color c2)
        {
            ForegroundColor = c1;
            ForegroundAccentColor = c2;

            GetSelfAndChildrenRecursive(f).OfType<Button>().ToList().ForEach(b => b.BackColor = c1);
            GetSelfAndChildrenRecursive(f).OfType<Button>().ToList().ForEach(b => b.FlatAppearance.BorderColor = c1);
            GetSelfAndChildrenRecursive(f).OfType<Button>().ToList().ForEach(b => b.FlatAppearance.MouseDownBackColor = c2);
            GetSelfAndChildrenRecursive(f).OfType<Button>().ToList().ForEach(b => b.FlatAppearance.MouseOverBackColor = c2);

            foreach (Label tmp in GetSelfAndChildrenRecursive(f).OfType<Label>().ToList())
            {
                if ((string)tmp.Tag == ThemeFlag)
                {
                    tmp.ForeColor = c1;
                }
            }
            foreach (LinkLabel tmp in GetSelfAndChildrenRecursive(f).OfType<LinkLabel>().ToList())
            {
                if ((string)tmp.Tag == ThemeFlag)
                {
                    tmp.LinkColor = c1;
                    tmp.VisitedLinkColor = c1;
                    tmp.ActiveLinkColor = c2;
                }
            }
            foreach (CheckBox tmp in GetSelfAndChildrenRecursive(f).OfType<CheckBox>().ToList())
            {
                if ((string)tmp.Tag == ThemeFlag)
                {
                    tmp.ForeColor = c1;
                }
            }
            foreach (RadioButton tmp in GetSelfAndChildrenRecursive(f).OfType<RadioButton>().ToList())
            {
                if ((string)tmp.Tag == ThemeFlag)
                {
                    tmp.ForeColor = c1;
                }
            }
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
                CurrentOptions.Color = Theme.Zerg;
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
        }
    }
}
