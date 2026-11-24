using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Hashing
{
    [Serializable]
    public sealed class SettingsJson
    {
        public Theme Color { get; set; }
        public AppearanceMode Appearance { get; set; }
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
            Appearance = AppearanceMode.Dark;
            HashOptions = new HashOptions();
        }
    }

    public static class Options
    {
        internal static Color ForegroundColor = Color.FromArgb(153, 102, 204);
        internal static Color ForegroundAccentColor = Color.FromArgb(134, 89, 179);
        internal static Color BackgroundColor = Color.FromArgb(30, 30, 30);
        internal static readonly string ThemeFlag = "themeable";
        private static readonly string SettingsFile = Path.Combine(Application.StartupPath, "Hashing.json");

        internal static SettingsJson CurrentOptions = new SettingsJson();
        internal static dynamic TranslationList;
        internal static readonly List<string> StartupWarnings = new List<string>();
        private static bool _settingsReadFailed;

        internal static IEnumerable<Control> GetSelfAndChildrenRecursive(Control parent)
        {
            if (parent == null)
                yield break;

            Stack<Control> pending = new Stack<Control>();
            pending.Push(parent);

            while (pending.Count > 0)
            {
                Control current = pending.Pop();
                yield return current;

                for (int i = current.Controls.Count - 1; i >= 0; i--)
                    pending.Push(current.Controls[i]);
            }
        }

        internal static void ApplyTheme(Form form)
        {
            if (form == null) return;
            Fluent.Apply(form);
        }

        internal static bool HasActiveHash(HashOptions hashes)
        {
            return hashes != null && (hashes.MD5 || hashes.SHA1 || hashes.SHA256 || hashes.SHA384 || hashes.SHA512 ||
                hashes.CRC32 || hashes.RIPEMD160 || hashes.SHA3_256 || hashes.SHA3_384 || hashes.SHA3_512);
        }

        internal static void InitializePalette()
        {
            if (CurrentOptions == null)
                CurrentOptions = new SettingsJson();

            switch (CurrentOptions.Color)
            {
                case Theme.Amber:
                    ForegroundColor = Color.FromArgb(195, 146, 0);
                    ForegroundAccentColor = Color.FromArgb(171, 128, 0);
                    break;
                case Theme.Jade:
                    ForegroundColor = Color.FromArgb(70, 175, 105);
                    ForegroundAccentColor = Color.FromArgb(61, 153, 92);
                    break;
                case Theme.Ruby:
                    ForegroundColor = Color.FromArgb(205, 22, 39);
                    ForegroundAccentColor = Color.FromArgb(155, 17, 30);
                    break;
                case Theme.Silver:
                    ForegroundColor = Color.Gray;
                    ForegroundAccentColor = Color.DimGray;
                    break;
                case Theme.Azurite:
                    ForegroundColor = Color.FromArgb(0, 127, 255);
                    ForegroundAccentColor = Color.FromArgb(0, 111, 223);
                    break;
                default:
                    ForegroundColor = Color.FromArgb(153, 102, 204);
                    ForegroundAccentColor = Color.FromArgb(134, 89, 179);
                    break;
            }

            Fluent.RefreshPalette();
            BackgroundColor = Fluent.Background;
        }

        internal static void SaveSettings()
        {
            string temporaryFile = SettingsFile + ".tmp";
            string json = JsonConvert.SerializeObject(CurrentOptions, Formatting.Indented);

            try
            {
                File.WriteAllText(temporaryFile, json);

                if (File.Exists(SettingsFile))
                    File.Replace(temporaryFile, SettingsFile, null);
                else
                    File.Move(temporaryFile, SettingsFile);
            }
            finally
            {
                if (File.Exists(temporaryFile))
                    File.Delete(temporaryFile);
            }
        }

        internal static bool TrySaveSettings(out string error)
        {
            try
            {
                if (_settingsReadFailed && File.Exists(SettingsFile))
                {
                    error = "The existing settings file was not loaded and has been preserved. Fix file access and restart before saving settings.";
                    return false;
                }

                _settingsReadFailed = false;
                SaveSettings();
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal static void LoadSettings()
        {
            StartupWarnings.Clear();
            _settingsReadFailed = false;
            bool recovered = false;
            string backupPath = null;
            string loadError = null;
            string saveError = null;

            if (!File.Exists(SettingsFile))
            {
                CurrentOptions = CreateDefaultSettings();
                try
                {
                    SaveSettings();
                }
                catch (Exception ex)
                {
                    saveError = ex.Message;
                }
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(SettingsFile);
                    if (string.IsNullOrWhiteSpace(json))
                        throw new JsonSerializationException("The settings file is empty.");

                    JObject.Parse(json);
                    CurrentOptions = CreateDefaultSettings();
                    JsonConvert.PopulateObject(json, CurrentOptions);
                    NormalizeSettings();
                }
                catch (Exception ex)
                {
                    loadError = ex.Message;
                    CurrentOptions = CreateDefaultSettings();

                    if (ex is JsonException || ex is System.Text.DecoderFallbackException)
                    {
                        recovered = true;
                        backupPath = TryBackupCorruptSettings();

                        try
                        {
                            SaveSettings();
                        }
                        catch (Exception saveException)
                        {
                            saveError = saveException.Message;
                        }
                    }
                    else
                    {
                        _settingsReadFailed = true;
                    }
                }
            }

            InitializePalette();
            LoadTranslation();

            if (recovered)
            {
                string backup = backupPath ?? TranslationList["settingsBackupUnavailable"].ToString();
                string saveFailure = string.IsNullOrWhiteSpace(saveError)
                    ? string.Empty
                    : Environment.NewLine + TranslationList["settingsRecoverySaveFailed"].ToString().Replace("{ERROR}", saveError);

                StartupWarnings.Add(TranslationList["settingsRecovery"].ToString()
                    .Replace("{BACKUP}", backup)
                    .Replace("{ERROR}", loadError ?? string.Empty)
                    .Replace("{SAVEERROR}", saveFailure));
            }
            else if (_settingsReadFailed)
            {
                StartupWarnings.Add(TranslationList["settingsReadFailed"].ToString().Replace("{ERROR}", loadError ?? string.Empty));
            }
            else if (!string.IsNullOrWhiteSpace(saveError))
            {
                StartupWarnings.Add(TranslationList["settingsCreateFailed"].ToString().Replace("{ERROR}", saveError));
            }
        }

        private static SettingsJson CreateDefaultSettings()
        {
            return new SettingsJson
            {
                Color = Theme.Amethyst,
                Appearance = AppearanceMode.Dark,
                HashOptions = new HashOptions
                {
                    SHA1 = true,
                    SHA256 = true
                },
                LowerCasing = false,
                TrayIcon = false,
                HighPriority = false,
                CRC32Decimal = false,
                ActiveHash = 1,
                StayOnTop = false,
                SingleInstance = true,
                WindowLocation = null,
                WindowSize = new Size(960, 700),
                WindowState = FormWindowState.Normal,
                LanguageCode = LanguageCode.EN
            };
        }

        private static void NormalizeSettings()
        {
            if (CurrentOptions == null)
                CurrentOptions = CreateDefaultSettings();

            if (!Enum.IsDefined(typeof(Theme), CurrentOptions.Color))
                CurrentOptions.Color = Theme.Amethyst;
            if (!Enum.IsDefined(typeof(AppearanceMode), CurrentOptions.Appearance))
                CurrentOptions.Appearance = AppearanceMode.Dark;
            if (!Enum.IsDefined(typeof(LanguageCode), CurrentOptions.LanguageCode))
                CurrentOptions.LanguageCode = LanguageCode.EN;
            if (CurrentOptions.HashOptions == null)
                CurrentOptions.HashOptions = CreateDefaultSettings().HashOptions;
            if (CurrentOptions.ActiveHash < 0 || CurrentOptions.ActiveHash > 9)
                CurrentOptions.ActiveHash = 1;
            if (CurrentOptions.WindowSize.Width < 320 || CurrentOptions.WindowSize.Height < 240)
                CurrentOptions.WindowSize = new Size(960, 700);
            if (CurrentOptions.WindowState != FormWindowState.Normal &&
                CurrentOptions.WindowState != FormWindowState.Maximized &&
                CurrentOptions.WindowState != FormWindowState.Minimized)
                CurrentOptions.WindowState = FormWindowState.Normal;
        }

        private static string TryBackupCorruptSettings()
        {
            try
            {
                if (!File.Exists(SettingsFile))
                    return null;

                string backupPath = SettingsFile + ".corrupt-" +
                    DateTime.Now.ToString("yyyyMMdd-HHmmssfff", System.Globalization.CultureInfo.InvariantCulture) + ".bak";
                File.Copy(SettingsFile, backupPath, false);
                return backupPath;
            }
            catch
            {
                return null;
            }
        }

        internal static void LoadTranslation()
        {
            switch (CurrentOptions.LanguageCode)
            {
                case LanguageCode.EL:
                    TranslationList = JObject.Parse(Properties.Resources.EL);
                    break;
                case LanguageCode.CN:
                    TranslationList = JObject.Parse(Properties.Resources.CN);
                    break;
                case LanguageCode.DE:
                    TranslationList = JObject.Parse(Properties.Resources.DE);
                    break;
                default:
                    TranslationList = JObject.Parse(Properties.Resources.EN);
                    break;
            }
        }
    }
}
