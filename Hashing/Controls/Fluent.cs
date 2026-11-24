using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Hashing
{
    // Shared palette, icons and window chrome helpers for the modern (Fluent-inspired) look.
    internal static class Fluent
    {
        sealed class Palette
        {
            internal readonly Color Background;
            internal readonly Color Surface;
            internal readonly Color Control;
            internal readonly Color ControlHover;
            internal readonly Color ControlPressed;
            internal readonly Color Border;
            internal readonly Color BorderStrong;
            internal readonly Color Menu;
            internal readonly Color Text;
            internal readonly Color TextSecondary;
            internal readonly Color TextDisabled;
            internal readonly Color Danger;
            internal readonly Color Success;
            internal readonly Color Warning;
            internal readonly Color Caution;
            internal readonly Color Favorite;

            internal Palette(bool dark)
            {
                if (dark)
                {
                    Background = Color.FromArgb(32, 32, 32);
                    Surface = Color.FromArgb(43, 43, 43);
                    Control = Color.FromArgb(51, 51, 51);
                    ControlHover = Color.FromArgb(63, 63, 63);
                    ControlPressed = Color.FromArgb(40, 40, 40);
                    Border = Color.FromArgb(68, 68, 68);
                    BorderStrong = Color.FromArgb(98, 98, 98);
                    Menu = Color.FromArgb(43, 43, 43);
                    Text = Color.FromArgb(245, 245, 245);
                    TextSecondary = Color.FromArgb(190, 190, 190);
                    TextDisabled = Color.FromArgb(150, 150, 150);
                    Danger = Color.FromArgb(255, 107, 107);
                    Success = Color.FromArgb(108, 203, 95);
                    Warning = Color.FromArgb(252, 185, 0);
                    Caution = Color.FromArgb(247, 140, 64);
                    Favorite = Color.FromArgb(255, 200, 61);
                }
                else
                {
                    Background = Color.FromArgb(242, 242, 242);
                    Surface = Color.FromArgb(250, 250, 250);
                    Control = Color.FromArgb(245, 245, 245);
                    ControlHover = Color.FromArgb(233, 233, 233);
                    ControlPressed = Color.FromArgb(222, 222, 222);
                    Border = Color.FromArgb(218, 218, 218);
                    BorderStrong = Color.FromArgb(174, 174, 174);
                    Menu = Color.FromArgb(250, 250, 250);
                    Text = Color.FromArgb(28, 28, 28);
                    TextSecondary = Color.FromArgb(91, 91, 91);
                    TextDisabled = Color.FromArgb(106, 106, 106);
                    Danger = Color.FromArgb(196, 43, 28);
                    Success = Color.FromArgb(16, 124, 16);
                    Warning = Color.FromArgb(157, 93, 0);
                    Caution = Color.FromArgb(194, 84, 0);
                    Favorite = Color.FromArgb(154, 103, 0);
                }
            }
        }

        static Palette _palette;
        static Palette _previousPalette;

        static Palette CurrentPalette
        {
            get
            {
                if (_palette == null) _palette = CreatePalette();
                return _palette;
            }
        }

        static Palette CreatePalette()
        {
            AppearanceMode mode = Options.CurrentOptions != null ? Options.CurrentOptions.Appearance : AppearanceMode.Dark;
            bool dark = mode == AppearanceMode.Dark || (mode == AppearanceMode.System && !SystemUsesLightTheme());
            return new Palette(dark);
        }

        static bool SystemUsesLightTheme()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object value = key == null ? null : key.GetValue("AppsUseLightTheme");
                    return value == null || Convert.ToInt32(value) != 0;
                }
            }
            catch
            {
                return true;
            }
        }

        internal static bool IsDark { get { return CurrentPalette.Background.R < 128; } }

        internal static Color Background { get { return CurrentPalette.Background; } }
        internal static Color Surface { get { return CurrentPalette.Surface; } }
        internal static Color Control { get { return CurrentPalette.Control; } }
        internal static Color ControlHover { get { return CurrentPalette.ControlHover; } }
        internal static Color ControlPressed { get { return CurrentPalette.ControlPressed; } }
        internal static Color Border { get { return CurrentPalette.Border; } }
        internal static Color BorderStrong { get { return CurrentPalette.BorderStrong; } }
        internal static Color Menu { get { return CurrentPalette.Menu; } }
        internal static Color Text { get { return CurrentPalette.Text; } }
        internal static Color TextSecondary { get { return CurrentPalette.TextSecondary; } }
        internal static Color TextDisabled { get { return CurrentPalette.TextDisabled; } }
        internal static Color Danger { get { return CurrentPalette.Danger; } }
        internal static Color Success { get { return CurrentPalette.Success; } }
        internal static Color Warning { get { return CurrentPalette.Warning; } }
        internal static Color Caution { get { return CurrentPalette.Caution; } }
        internal static Color Favorite { get { return CurrentPalette.Favorite; } }

        internal static void RefreshPalette()
        {
            _previousPalette = CurrentPalette;
            _palette = CreatePalette();
        }

        internal static void CompletePaletteChange()
        {
            _previousPalette = null;
        }

        internal static Color Accent { get { return Options.ForegroundColor; } }
        internal static Color AccentText { get { return AccentTextOn(Control); } }
        internal static Color AccentHoverText { get { return AccentTextOn(ControlHover); } }
        internal static Color AccentTextOn(Color background) { return AccessibleAccentText(Accent, background); }
        internal static Color OnAccent { get { return OnColor(Accent); } }

        private static Color AccessibleAccentText(Color accent, Color background)
        {
            if (ContrastRatio(accent, background) >= 4.5)
                return accent;

            Color target = OnColor(background);
            float low = 0f;
            float high = 1f;

            for (int i = 0; i < 18; i++)
            {
                float middle = (low + high) / 2f;
                Color candidate = Blend(accent, target, middle);
                if (ContrastRatio(candidate, background) >= 4.5)
                    high = middle;
                else
                    low = middle;
            }

            return Blend(accent, target, high);
        }

        private static double ContrastRatio(Color foreground, Color background)
        {
            double foregroundLuminance = Luminance(foreground);
            double backgroundLuminance = Luminance(background);
            double lighter = Math.Max(foregroundLuminance, backgroundLuminance);
            double darker = Math.Min(foregroundLuminance, backgroundLuminance);
            return (lighter + 0.05) / (darker + 0.05);
        }

        // Windows 11 ships Segoe UI Variable (optical sizes, hinted for ClearType); Windows 10 falls back to Segoe UI.
        // GDI+ truncates family names to 31 chars, hence "Display Semib".
        internal static readonly string UiFont = ResolveFamily("Segoe UI Variable Text", "Segoe UI");
        internal static readonly string UiFontSemibold = ResolveFamily("Segoe UI Variable Text Semibold", "Segoe UI Semibold");
        internal static readonly string DisplayFont = ResolveFamily("Segoe UI Variable Display Semib", "Segoe UI Semibold");
        internal static readonly string MonoFont = ResolveFamily("Cascadia Mono SemiLight", "Cascadia Mono", "Consolas");

        static HashSet<string> _installedFamilies;

        static string ResolveFamily(params string[] candidates)
        {
            if (_installedFamilies == null)
            {
                _installedFamilies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                try
                {
                    using (InstalledFontCollection fonts = new InstalledFontCollection())
                    {
                        foreach (FontFamily f in fonts.Families) _installedFamilies.Add(f.Name);
                    }
                }
                catch { }
            }

            foreach (string name in candidates)
            {
                if (_installedFamilies.Contains(name)) return name;
            }
            return candidates[candidates.Length - 1];
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        sealed class LogFont
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName = string.Empty;
        }

        static readonly Dictionary<string, Font> _fontCache = new Dictionary<string, Font>(StringComparer.Ordinal);

        static Font CachedFont(string key, Func<Font> factory)
        {
            Font font;
            if (!_fontCache.TryGetValue(key, out font))
            {
                font = factory();
                _fontCache.Add(key, font);
            }
            return font;
        }

        internal static Font Ui(float points)
        {
            string key = "ui:" + points.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return CachedFont(key, () =>
            {
                using (Font regular = new Font(UiFont, points))
                {
                    try
                    {
                        LogFont logFont = new LogFont();
                        regular.ToLogFont(logFont);
                        logFont.lfWeight = 500;
                        return Font.FromLogFont(logFont);
                    }
                    catch
                    {
                        return new Font(UiFontSemibold, points);
                    }
                }
            });
        }

        internal static Font UiSemibold(float points)
        {
            string key = "semibold:" + points.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return CachedFont(key, () => new Font(UiFontSemibold, points));
        }

        internal static Font Display(float points)
        {
            string key = "display:" + points.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return CachedFont(key, () => new Font(DisplayFont, points));
        }

        internal static Font Mono(float points)
        {
            string key = "mono:" + points.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return CachedFont(key, () => new Font(MonoFont, points));
        }

        #region Text

        // Central text helpers: crisp GDI ClearType text that follows the system ClearType settings.
        internal static void DrawText(Graphics g, string text, Font font, Rectangle bounds, Color color, Color backColor, TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text) || bounds.Width <= 0 || bounds.Height <= 0) return;
            TextRenderer.DrawText(g, text, font, bounds, color, backColor, flags);
        }

        internal static void DrawText(Graphics g, string text, Font font, Rectangle bounds, Color color, TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text) || bounds.Width <= 0 || bounds.Height <= 0) return;
            TextRenderer.DrawText(g, text, font, bounds, color, flags);
        }

        internal static Size MeasureText(string text, Font font)
        {
            return TextRenderer.MeasureText(text, font);
        }

        internal static Size MeasureText(Graphics g, string text, Font font, Size proposedSize, TextFormatFlags flags)
        {
            return TextRenderer.MeasureText(g, text, font, proposedSize, flags);
        }

        internal static Size MeasureText(string text, Font font, Size proposedSize, TextFormatFlags flags)
        {
            return TextRenderer.MeasureText(text, font, proposedSize, flags);
        }

        internal static TextFormatFlags AlignmentFlags(ContentAlignment align)
        {
            TextFormatFlags f = TextFormatFlags.Default;
            switch (align)
            {
                case ContentAlignment.TopCenter: f = TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.TopRight: f = TextFormatFlags.Right; break;
                case ContentAlignment.MiddleLeft: f = TextFormatFlags.VerticalCenter; break;
                case ContentAlignment.MiddleCenter: f = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.MiddleRight: f = TextFormatFlags.VerticalCenter | TextFormatFlags.Right; break;
                case ContentAlignment.BottomLeft: f = TextFormatFlags.Bottom; break;
                case ContentAlignment.BottomCenter: f = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter; break;
                case ContentAlignment.BottomRight: f = TextFormatFlags.Bottom | TextFormatFlags.Right; break;
            }
            return f;
        }

        internal static TextFormatFlags AlignmentFlags(DataGridViewContentAlignment align)
        {
            switch (align)
            {
                case DataGridViewContentAlignment.TopLeft: return AlignmentFlags(ContentAlignment.TopLeft);
                case DataGridViewContentAlignment.TopCenter: return AlignmentFlags(ContentAlignment.TopCenter);
                case DataGridViewContentAlignment.TopRight: return AlignmentFlags(ContentAlignment.TopRight);
                case DataGridViewContentAlignment.MiddleCenter: return AlignmentFlags(ContentAlignment.MiddleCenter);
                case DataGridViewContentAlignment.MiddleRight: return AlignmentFlags(ContentAlignment.MiddleRight);
                case DataGridViewContentAlignment.BottomLeft: return AlignmentFlags(ContentAlignment.BottomLeft);
                case DataGridViewContentAlignment.BottomCenter: return AlignmentFlags(ContentAlignment.BottomCenter);
                case DataGridViewContentAlignment.BottomRight: return AlignmentFlags(ContentAlignment.BottomRight);
                default: return AlignmentFlags(ContentAlignment.MiddleLeft);
            }
        }

        #endregion

        internal static class Glyphs
        {
            internal const string Add = "\uE710";
            internal const string Edit = "\uE70F";
            internal const string Delete = "\uE74D";
            internal const string Copy = "\uE8C8";
            internal const string Contact = "\uE77B";
            internal const string Key = "\uE8D7";
            internal const string Link = "\uE71B";
            internal const string Lock = "\uE72E";
            internal const string Settings = "\uE713";
            internal const string More = "\uE712";
            internal const string Search = "\uE721";
            internal const string Cancel = "\uE711";
            internal const string Info = "\uE946";
            internal const string Download = "\uE896";
            internal const string Shield = "\uEA18";
            internal const string View = "\uE7B3";
            internal const string Hide = "\uED1A";
            internal const string Refresh = "\uE72C";
            internal const string Checkmark = "\uE73E";
            internal const string Warning = "\uE7BA";
            internal const string Clear = "\uE894";
            internal const string Star = "\uE734";
            internal const string StarFill = "\uE735";
            internal const string Tag = "\uE8EC";
            internal const string Partial = "\uE738";
            internal const string Filter = "\uE71C";
            internal const string Sort = "\uE8CB";
        }

        sealed class MenuGlyph
        {
            internal readonly string Value;
            internal MenuGlyph(string value) { Value = value; }
        }

        static readonly ConditionalWeakTable<ToolStripItem, MenuGlyph> MenuGlyphs = new ConditionalWeakTable<ToolStripItem, MenuGlyph>();

        static string _iconFamily;
        internal static string IconFamily
        {
            get
            {
                if (_iconFamily == null)
                {
                    try
                    {
                        using (InstalledFontCollection fonts = new InstalledFontCollection())
                        {
                            bool hasFluentIcons = false;
                            bool hasMdl2Assets = false;
                            foreach (FontFamily family in fonts.Families)
                            {
                                if (string.Equals(family.Name, "Segoe Fluent Icons", StringComparison.OrdinalIgnoreCase))
                                    hasFluentIcons = true;
                                else if (string.Equals(family.Name, "Segoe MDL2 Assets", StringComparison.OrdinalIgnoreCase))
                                    hasMdl2Assets = true;

                                if (hasFluentIcons) break;
                            }

                            _iconFamily = hasFluentIcons ? "Segoe Fluent Icons" : hasMdl2Assets ? "Segoe MDL2 Assets" : string.Empty;
                        }
                    }
                    catch
                    {
                        _iconFamily = string.Empty;
                    }
                }
                return _iconFamily;
            }
        }

        internal static bool HasIcons { get { return IconFamily.Length > 0; } }

        static readonly Dictionary<float, Font> _iconFonts = new Dictionary<float, Font>();
        internal static Font IconFont(float points)
        {
            Font f;
            if (!_iconFonts.TryGetValue(points, out f))
            {
                f = new Font(HasIcons ? IconFamily : UiFont, points, FontStyle.Regular, GraphicsUnit.Point);
                _iconFonts[points] = f;
            }
            return f;
        }

        internal static Color Blend(Color from, Color to, float amount)
        {
            amount = Math.Max(0f, Math.Min(1f, amount));
            return Color.FromArgb(
                (int)(from.A + (to.A - from.A) * amount),
                (int)(from.R + (to.R - from.R) * amount),
                (int)(from.G + (to.G - from.G) * amount),
                (int)(from.B + (to.B - from.B) * amount));
        }

        internal static double Luminance(Color c)
        {
            double r = c.R / 255.0;
            double g = c.G / 255.0;
            double b = c.B / 255.0;

            r = r <= 0.04045 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
            g = g <= 0.04045 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
            b = b <= 0.04045 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        internal static Color OnColor(Color color)
        {
            double luminance = Luminance(color);
            double whiteContrast = 1.05 / (luminance + 0.05);
            double blackContrast = (luminance + 0.05) / 0.05;
            return blackContrast >= whiteContrast ? Color.Black : Color.White;
        }

        static Color MapThemeColor(Color color, Palette from, Palette to)
        {
            if (from == null || to == null || color.IsEmpty || color.A != 255) return color;

            if (color.ToArgb() == from.Background.ToArgb()) return to.Background;
            if (color.ToArgb() == from.Surface.ToArgb()) return to.Surface;
            if (color.ToArgb() == from.Control.ToArgb()) return to.Control;
            if (color.ToArgb() == from.ControlHover.ToArgb()) return to.ControlHover;
            if (color.ToArgb() == from.ControlPressed.ToArgb()) return to.ControlPressed;
            if (color.ToArgb() == from.Border.ToArgb()) return to.Border;
            if (color.ToArgb() == from.BorderStrong.ToArgb()) return to.BorderStrong;
            if (color.ToArgb() == from.Menu.ToArgb()) return to.Menu;
            if (color.ToArgb() == from.Text.ToArgb()) return to.Text;
            if (color.ToArgb() == from.TextSecondary.ToArgb()) return to.TextSecondary;
            if (color.ToArgb() == from.TextDisabled.ToArgb()) return to.TextDisabled;
            if (color.ToArgb() == from.Danger.ToArgb()) return to.Danger;
            if (color.ToArgb() == from.Success.ToArgb()) return to.Success;
            if (color.ToArgb() == from.Warning.ToArgb()) return to.Warning;
            if (color.ToArgb() == from.Caution.ToArgb()) return to.Caution;
            if (color.ToArgb() == from.Favorite.ToArgb()) return to.Favorite;

            return color;
        }

        static void MapCellStyle(DataGridViewCellStyle style, Palette from, Palette to)
        {
            if (style == null) return;

            Color color = MapThemeColor(style.BackColor, from, to);
            if (color != style.BackColor) style.BackColor = color;

            color = MapThemeColor(style.ForeColor, from, to);
            if (color != style.ForeColor) style.ForeColor = color;

            color = MapThemeColor(style.SelectionBackColor, from, to);
            if (color != style.SelectionBackColor) style.SelectionBackColor = color;

            color = MapThemeColor(style.SelectionForeColor, from, to);
            if (color != style.SelectionForeColor) style.SelectionForeColor = color;
        }

        static void MapGridColors(DataGridView grid, Palette from, Palette to)
        {
            MapCellStyle(grid.DefaultCellStyle, from, to);
            MapCellStyle(grid.AlternatingRowsDefaultCellStyle, from, to);
            MapCellStyle(grid.RowsDefaultCellStyle, from, to);
            MapCellStyle(grid.ColumnHeadersDefaultCellStyle, from, to);
            MapCellStyle(grid.RowHeadersDefaultCellStyle, from, to);

            foreach (DataGridViewColumn column in grid.Columns)
            {
                MapCellStyle(column.DefaultCellStyle, from, to);
                MapCellStyle(column.HeaderCell.Style, from, to);
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                MapCellStyle(row.DefaultCellStyle, from, to);
                foreach (DataGridViewCell cell in row.Cells)
                {
                    MapCellStyle(cell.Style, from, to);
                }
            }
        }

        internal static Color SelectionColor
        {
            get { return Blend(Surface, Accent, 0.32f); }
        }

        internal static int Scale(Control c, int value)
        {
            return (int)Math.Round(value * c.DeviceDpi / 96.0);
        }

        internal static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 0.5f)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        internal static Color EffectiveBackColor(Control c)
        {
            while (c != null)
            {
                if (c.BackColor.A == 255) return c.BackColor;
                c = c.Parent;
            }
            return Background;
        }

        // Renders an icon glyph centered on its actual outline (not the font's line box),
        // so every icon sits optically centered in its square.
        internal static Bitmap GlyphImage(string glyph, Color color, int size)
        {
            Bitmap bmp = new Bitmap(size, size);
            if (!HasIcons) return bmp;

            using (Graphics g = Graphics.FromImage(bmp))
            using (FontFamily family = new FontFamily(IconFamily))
            using (GraphicsPath path = new GraphicsPath())
            using (SolidBrush b = new SolidBrush(color))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                path.AddString(glyph, family, (int)FontStyle.Regular, size, PointF.Empty, StringFormat.GenericTypographic);
                RectangleF bounds = path.GetBounds();
                if (bounds.Width <= 0 || bounds.Height <= 0) return bmp;

                float max = size * 0.94f;
                float scale = Math.Min(1f, max / Math.Max(bounds.Width, bounds.Height));

                using (Matrix m = new Matrix())
                {
                    m.Translate(size / 2f, size / 2f);
                    m.Scale(scale, scale);
                    m.Translate(-(bounds.X + bounds.Width / 2f), -(bounds.Y + bounds.Height / 2f));
                    path.Transform(m);
                }
                g.FillPath(b, path);
            }
            return bmp;
        }

        #region Native

        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        static extern int SetWindowTheme(IntPtr hwnd, string subAppName, string subIdList);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;
        const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        const int DWMWA_BORDER_COLOR = 34;
        const int DWMWA_CAPTION_COLOR = 35;
        const int EM_SETCUEBANNER = 0x1501;

        static int ToColorRef(Color c)
        {
            return c.R | (c.G << 8) | (c.B << 16);
        }

        #endregion

        static void WhenHandleReady(Control c, Action<Control> action)
        {
            if (c.IsHandleCreated) action(c);
            c.HandleCreated += (s, e) => action((Control)s);
        }

        // Theme-aware title bar, matching caption color and rounded corners (Windows 10 1809+ / 11).
        internal static void StyleWindow(Form f)
        {
            WhenHandleReady(f, c =>
            {
                try
                {
                    int dark = IsDark ? 1 : 0;
                    if (DwmSetWindowAttribute(c.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, 4) != 0)
                    {
                        DwmSetWindowAttribute(c.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_OLD, ref dark, 4);
                    }

                    int caption = ToColorRef(c.BackColor);
                    DwmSetWindowAttribute(c.Handle, DWMWA_CAPTION_COLOR, ref caption, 4);

                    int border = ToColorRef(Border);
                    DwmSetWindowAttribute(c.Handle, DWMWA_BORDER_COLOR, ref border, 4);

                    int round = 2;
                    DwmSetWindowAttribute(c.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref round, 4);
                }
                catch { }
            });
        }

        internal static void ThemeScrollBars(Control c)
        {
            WhenHandleReady(c, x =>
            {
                try { SetWindowTheme(x.Handle, IsDark ? "DarkMode_Explorer" : "Explorer", null); } catch { }
            });
        }

        internal static void SetPlaceholder(TextBox tb, string text)
        {
            WhenHandleReady(tb, x =>
            {
                try { SendMessage(x.Handle, EM_SETCUEBANNER, (IntPtr)1, text); } catch { }
            });
        }

        static void ApplyUiFont(Control control)
        {
            if (control.Font == null || control is IconLabel) return;
            string family = control.Font.FontFamily.Name;
            if (string.Equals(family, IconFamily, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(family, MonoFont, StringComparison.OrdinalIgnoreCase)) return;

            float size = (float)Math.Round(control.Font.SizeInPoints, 1);
            bool semibold = control.Font.Bold || family.IndexOf("Semibold", StringComparison.OrdinalIgnoreCase) >= 0;
            string expected = semibold ? UiFontSemibold : UiFont;
            if (string.Equals(control.Font.Name, expected, StringComparison.OrdinalIgnoreCase)) return;
            control.Font = semibold ? UiSemibold(size) : Ui(size);
        }

        // Applies the active palette and the shared Lain typography to a form and all its children.
        internal static void Apply(Form f)
        {
            if (f == null) return;

            Palette previous = _previousPalette;
            Palette current = CurrentPalette;

            f.BackColor = current.Background;
            f.ForeColor = current.Text;

            StyleWindow(f);

            foreach (Control c in Options.GetSelfAndChildrenRecursive(f).Reverse())
            {
                if (!(c is Form))
                {
                    c.BackColor = MapThemeColor(c.BackColor, previous, current);
                    c.ForeColor = MapThemeColor(c.ForeColor, previous, current);
                }

                Card card = c as Card;
                if (card != null)
                {
                    card.BackColor = current.Surface;
                    card.BorderColor = current.Border;
                }

                FooterPanel footer = c as FooterPanel;
                if (footer != null) footer.BackColor = current.Surface;

                FluentGrid grid = c as FluentGrid;
                if (grid != null) grid.ApplyColors(null, null);

                DataGridView dataGrid = c as DataGridView;
                if (dataGrid != null) MapGridColors(dataGrid, previous, current);

                IconLabel icon = c as IconLabel;
                if (icon != null && icon.GlyphColor.HasValue)
                {
                    icon.GlyphColor = MapThemeColor(icon.GlyphColor.Value, previous, current);
                    icon.UpdateColor();
                }

                if (c is Label && !(c is IconLabel))
                {
                    string role = Convert.ToString(c.Tag);
                    if (role == Options.ThemeFlag) c.ForeColor = AccentText;
                    else if (role == "secondary" || c.Name == "lblversion" || c.Name == "lblNoMatch") c.ForeColor = TextSecondary;
                    else c.ForeColor = Text;
                    if (c.BackColor.IsEmpty || c.BackColor == SystemColors.Control) c.BackColor = Color.Transparent;
                }

                LinkLabel link = c as LinkLabel;
                if (link != null)
                {
                    link.LinkColor = AccentText;
                    link.ActiveLinkColor = Text;
                    link.VisitedLinkColor = TextSecondary;
                    link.DisabledLinkColor = TextDisabled;
                    link.LinkBehavior = LinkBehavior.HoverUnderline;
                }

                if (c is MoonTree || c is MoonList)
                {
                    c.BackColor = current.Surface;
                    c.ForeColor = current.Text;
                }

                if (c is TextBoxBase && !(c.Parent is InputBox))
                {
                    c.BackColor = current.Control;
                    c.ForeColor = current.Text;
                }
                else if (c is ComboBox)
                {
                    c.BackColor = current.Control;
                    c.ForeColor = current.Text;
                    ((ComboBox)c).FlatStyle = FlatStyle.Flat;
                }
                else if (c is Panel && !(c is Card) && !(c is FooterPanel) && !(c is InputBox) &&
                         (c.BackColor.IsEmpty || c.BackColor == SystemColors.Control))
                {
                    c.BackColor = c.Parent == null ? current.Background : c.Parent.BackColor;
                }

                if (c is ModernButton || c is Label || c is TextBoxBase || c is ComboBox ||
                    c is MoonCheck || c is MoonRadio || c is MoonTree || c is MoonList)
                    ApplyUiFont(c);

                if (c is TextBoxBase && ((TextBoxBase)c).Multiline) ThemeScrollBars(c);
                if (c is DataGridView)
                {
                    foreach (Control child in c.Controls)
                    {
                        if (child is ScrollBar) ThemeScrollBars(child);
                    }
                }
                if (c.ContextMenuStrip != null) StyleMenu(c.ContextMenuStrip);
            }

            foreach (InputBox input in Options.GetSelfAndChildrenRecursive(f).OfType<InputBox>())
                input.ApplyPalette();

            f.Invalidate(true);
        }

        // Theme-aware base styling for dialogs.
        internal static void StyleDialog(Form f)
        {
            f.BackColor = Background;
            f.ForeColor = Text;
            Apply(f);
            f.Invalidate(true);
        }

        internal static void SetEye(IconLabel eye, bool passwordVisible)
        {
            eye.Fallback = passwordVisible ? "\u25CB" : "\u25CF";
            eye.Glyph = passwordVisible ? Glyphs.Hide : Glyphs.View;
            eye.Active = passwordVisible;
        }

        internal static void StyleMenu(ToolStripDropDown menu)
        {
            int dpi = menu.DeviceDpi;
            Func<int, int> scale = v => (int)Math.Round(v * dpi / 96.0);

            bool initialize = !(menu.Renderer is MoonMenuRenderer);
            if (initialize) menu.Renderer = new MoonMenuRenderer();
            menu.Font = Ui(10f);
            menu.BackColor = Menu;
            menu.ForeColor = Text;
            menu.Padding = new Padding(scale(4), scale(4), scale(4), scale(4));

            ToolStripDropDownMenu dropDown = menu as ToolStripDropDownMenu;
            if (dropDown != null)
            {
                dropDown.ShowImageMargin = true;
                dropDown.ShowCheckMargin = false;
                dropDown.ImageScalingSize = new Size(scale(16), scale(16));
            }

            StyleMenuItems(menu);

            if (initialize)
            {
                menu.Opening += (s, e) => FitMenuWidth(menu);
                menu.LayoutCompleted += (s, e) => StretchMenuItems(menu);
            }

            if (initialize) WhenHandleReady(menu, c =>
            {
                try
                {
                    int round = 3;
                    DwmSetWindowAttribute(c.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref round, 4);
                }
                catch { }
            });
        }

        // Applies the menu font, colors and row height to the items; call again after adding items to a styled menu.
        internal static void StyleMenuItems(ToolStripDropDown menu)
        {
            int dpi = menu.DeviceDpi;
            Func<int, int> scale = v => (int)Math.Round(v * dpi / 96.0);

            foreach (ToolStripItem item in menu.Items)
            {
                Color itemColor = MapThemeColor(item.ForeColor, _previousPalette, CurrentPalette);
                bool accentItem = itemColor.ToArgb() == Accent.ToArgb() || itemColor.ToArgb() == AccentText.ToArgb();
                if (!accentItem && _previousPalette != null)
                    accentItem = item.ForeColor.ToArgb() == AccessibleAccentText(Accent, _previousPalette.Control).ToArgb();
                item.ForeColor = itemColor == Danger ? Danger : accentItem ? AccentText : Text;
                item.Font = menu.Font;

                MenuGlyph glyph;
                if (MenuGlyphs.TryGetValue(item, out glyph))
                {
                    int iconSize = (int)Math.Round(16 * dpi / 96.0);
                    Image oldImage = item.Image;
                    item.Image = GlyphImage(glyph.Value, TextSecondary, iconSize);
                    if (oldImage != null) oldImage.Dispose();
                }

                ToolStripMenuItem mi = item as ToolStripMenuItem;
                if (mi == null) continue;

                // Uniform row height for every item.
                mi.Padding = new Padding(scale(4), scale(7), scale(8), scale(7));
                if (mi.HasDropDownItems) StyleMenu(mi.DropDown);
            }
        }

        // Sizes the menu so the renderer's fixed icon/text/shortcut/arrow columns always fit;
        // the dropdown otherwise keeps a stale width after its items were restyled.
        static void FitMenuWidth(ToolStripDropDown menu)
        {
            menu.PerformLayout();
            int need = 0;
            foreach (ToolStripItem item in menu.Items)
            {
                if (!item.Available) continue;
                ToolStripMenuItem mi = item as ToolStripMenuItem;
                int w = mi != null ? Math.Max(MoonMenuRenderer.RequiredWidth(mi), mi.GetPreferredSize(Size.Empty).Width) : 0;
                need = Math.Max(need, w);
            }

            int total = need + menu.Padding.Horizontal + 2;
            if (menu.MinimumSize.Width != total) menu.MinimumSize = new Size(total, 0);
            if (menu.Width < total) menu.Width = total;
            menu.PerformLayout();
        }

        // The dropdown layout sizes items to their own preferred width, so after FitMenuWidth widens the
        // menu the items (and their hover highlight) would stop short of the right edge.
        static void StretchMenuItems(ToolStripDropDown menu)
        {
            foreach (ToolStripItem item in menu.Items)
            {
                if (!item.Available) continue;
                int w = menu.ClientSize.Width - item.Bounds.X - menu.Padding.Right;
                if (w > 0 && item.Width != w) item.Width = w;
            }
        }

        internal static bool IsMenuGlyph(ToolStripItem item)
        {
            MenuGlyph glyph;
            return item != null && MenuGlyphs.TryGetValue(item, out glyph);
        }

        internal static void SetMenuGlyph(ToolStripItem item, string glyph, int dpi)
        {
            if (!HasIcons) return;
            int size = (int)Math.Round(16 * dpi / 96.0);
            item.ImageScaling = ToolStripItemImageScaling.None;
            MenuGlyphs.Remove(item);
            MenuGlyphs.Add(item, new MenuGlyph(glyph ?? string.Empty));
            item.Image = GlyphImage(glyph, TextSecondary, size);
        }

        // Theme-aware, padded tooltips.
        internal static void StyleToolTip(ToolTip tip)
        {
            tip.OwnerDraw = true;
            tip.BackColor = Menu;
            tip.ForeColor = Text;
            tip.InitialDelay = 400;
            tip.ReshowDelay = 100;

            Font font = Ui(9.5f);

            tip.Popup += (s, e) =>
            {
                Size text = Fluent.MeasureText(tip.GetToolTip(e.AssociatedControl), font);
                e.ToolTipSize = new Size(text.Width + 20, text.Height + 12);
            };

            tip.Draw += (s, e) =>
            {
                e.Graphics.Clear(Menu);
                using (Pen p = new Pen(Border))
                {
                    e.Graphics.DrawRectangle(p, 0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }
                Fluent.DrawText(e.Graphics, e.ToolTipText, font, e.Bounds, Text,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
            };
        }
    }

    public enum ButtonVariant
    {
        Primary,
        Secondary,
        Subtle,
        Danger
    }

    // Rounded, animated button with optional icon glyph.
    [DesignerCategory("Code")]
    public sealed class ModernButton : Button
    {
        readonly Timer _animation = new Timer { Interval = 15 };
        float _hover;
        bool _over;
        bool _pressed;

        ButtonVariant _variant = ButtonVariant.Secondary;
        string _glyph = string.Empty;

        public ModernButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Font = Fluent.Ui(10f);
            Padding = new Padding(14, 0, 14, 0);

            _animation.Tick += (s, e) =>
            {
                float target = _over ? 1f : 0f;
                _hover += Math.Sign(target - _hover) * 0.2f;
                if (Math.Abs(target - _hover) < 0.2f)
                {
                    _hover = target;
                    _animation.Stop();
                }
                Invalidate();
            };
        }

        [DefaultValue(ButtonVariant.Secondary)]
        public ButtonVariant Variant
        {
            get { return _variant; }
            set { _variant = value; Invalidate(); }
        }

        [DefaultValue("")]
        public string Glyph
        {
            get { return _glyph; }
            set { _glyph = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue(6)]
        public int Radius { get; set; } = 6;

        protected override void Dispose(bool disposing)
        {
            if (disposing) _animation.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _over = true;
            _animation.Start();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _over = false;
            _pressed = false;
            _animation.Start();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left) { _pressed = true; Invalidate(); }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled) { _over = false; _hover = 0; }
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Invalidate();
        }

        bool ShowGlyph { get { return _glyph.Length > 0 && Fluent.HasIcons; } }

        Font GlyphFont { get { return Fluent.IconFont((float)Math.Round(Font.SizeInPoints * 1.1f, 1)); } }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size text = string.IsNullOrEmpty(Text) ? Size.Empty : Fluent.MeasureText(Text, Font, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            Size glyph = ShowGlyph ? Fluent.MeasureText(_glyph, GlyphFont, Size.Empty, TextFormatFlags.NoPadding) : Size.Empty;
            int gap = (text.Width > 0 && glyph.Width > 0) ? Font.Height / 2 : 0;
            int height = Font.Height + Fluent.Scale(this, 14);

            if (text.Width == 0) return new Size(height, height);
            return new Size(glyph.Width + gap + text.Width + Padding.Horizontal, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color back = Fluent.EffectiveBackColor(Parent);
            g.Clear(back);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color accent = Fluent.Accent;
            Color fill = back, border = Color.Empty, fore = Fluent.Text, glyphColor;
            ButtonVariant variant = _variant;
            string tag = Convert.ToString(Tag);
            if (string.Equals(tag, "primary", StringComparison.OrdinalIgnoreCase)) variant = ButtonVariant.Primary;
            else if (string.Equals(tag, "danger", StringComparison.OrdinalIgnoreCase)) variant = ButtonVariant.Danger;

            switch (variant)
            {
                case ButtonVariant.Primary:
                    fill = Enabled ? Fluent.Blend(accent, Color.White, _hover * 0.12f) : Fluent.Control;
                    if (_pressed) fill = Fluent.Blend(accent, Color.Black, 0.15f);
                    fore = Enabled ? Fluent.OnAccent : Fluent.TextDisabled;
                    break;
                case ButtonVariant.Secondary:
                    fill = Fluent.Blend(Fluent.Control, Fluent.ControlHover, _hover);
                    if (_pressed) fill = Fluent.ControlPressed;
                    border = Fluent.Border;
                    break;
                case ButtonVariant.Subtle:
                    fill = Fluent.Blend(back, Fluent.ControlHover, _hover);
                    if (_pressed) fill = Fluent.Control;
                    break;
                case ButtonVariant.Danger:
                    fill = Fluent.Blend(back, Fluent.Blend(back, Fluent.Danger, 0.22f), _hover);
                    if (_pressed) fill = Fluent.Blend(back, Fluent.Danger, 0.12f);
                    fore = Fluent.Danger;
                    break;
            }

            if (!Enabled) fore = Fluent.TextDisabled;
            glyphColor = fore;
            if (Enabled && variant == ButtonVariant.Subtle && Text.Length > 0) glyphColor = Fluent.TextSecondary;

            int radius = Fluent.Scale(this, Radius);
            RectangleF rect = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (GraphicsPath path = Fluent.RoundedRect(rect, radius))
            {
                if (fill != back)
                {
                    using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, path);
                }
                if (!border.IsEmpty)
                {
                    using (Pen p = new Pen(border)) g.DrawPath(p, path);
                }
                if (Focused && ShowFocusCues)
                {
                    using (GraphicsPath focus = Fluent.RoundedRect(new RectangleF(1.5f, 1.5f, Width - 3.5f, Height - 3.5f), radius - 1))
                    using (Pen p = new Pen(Fluent.Text, 1.5f)) g.DrawPath(p, focus);
                }
            }

            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;
            if (!ShowKeyboardCues) flags |= TextFormatFlags.HidePrefix;

            Size textSize = string.IsNullOrEmpty(Text) ? Size.Empty : Fluent.MeasureText(g, Text, Font, Size.Empty, flags);
            Size glyphSize = ShowGlyph ? Fluent.MeasureText(g, _glyph, GlyphFont, Size.Empty, TextFormatFlags.NoPadding) : Size.Empty;
            int gap = (textSize.Width > 0 && glyphSize.Width > 0) ? Font.Height / 2 : 0;
            int x = (Width - (glyphSize.Width + gap + textSize.Width)) / 2;

            if (glyphSize.Width > 0)
            {
                Fluent.DrawText(g, _glyph, GlyphFont, new Rectangle(x, 1, glyphSize.Width, Height), glyphColor, fill, TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter);
                x += glyphSize.Width + gap;
            }
            if (textSize.Width > 0)
            {
                Fluent.DrawText(g, Text, Font, new Rectangle(x, 0, textSize.Width + 2, Height), fore, fill, flags);
            }
        }
    }

    // Label that displays an icon glyph, with an optional hover/active state for clickable icons.
    [DesignerCategory("Code")]
    public sealed class IconLabel : Label
    {
        string _glyph = string.Empty;
        string _fallback = string.Empty;
        bool _active;
        bool _over;

        public IconLabel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            AutoSize = false;
            TextAlign = ContentAlignment.MiddleCenter;
            ForeColor = Fluent.TextSecondary;
            IconSize = 11f;
        }

        public string Glyph
        {
            get { return _glyph; }
            set { _glyph = value ?? string.Empty; UpdateText(); }
        }

        public string Fallback
        {
            get { return _fallback; }
            set { _fallback = value ?? string.Empty; UpdateText(); }
        }

        float _iconSize;
        public float IconSize
        {
            get { return _iconSize; }
            set { _iconSize = value; UpdateText(); }
        }

        public bool Clickable { get; set; }

        public bool Active
        {
            get { return _active; }
            set { _active = value; UpdateColor(); }
        }

        public Color? GlyphColor { get; set; }

        void UpdateText()
        {
            if (Fluent.HasIcons)
            {
                Font = Fluent.IconFont(_iconSize);
                Text = _glyph;
            }
            else
            {
                Font = new Font(Fluent.UiFont, Math.Max(8f, _iconSize * 0.8f));
                Text = _fallback;
            }
            UpdateColor();
        }

        internal void UpdateColor()
        {
            if (GlyphColor.HasValue) ForeColor = GlyphColor.Value;
            else if (_active) ForeColor = Fluent.AccentText;
            else if (Clickable && _over) ForeColor = Fluent.Text;
            else ForeColor = Fluent.TextSecondary;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _over = true;
            if (Clickable) Cursor = Cursors.Hand;
            UpdateColor();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _over = false;
            UpdateColor();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle r = new Rectangle(Padding.Left, Padding.Top, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical);
            Fluent.DrawText(e.Graphics, Text, Font, r, Enabled ? ForeColor : Fluent.TextDisabled,
                Fluent.AlignmentFlags(TextAlign) | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
        }
    }

    // Rounded input container: hosts a borderless TextBox plus optional leading/trailing icons,
    // and highlights with the accent color when focused.
    [DesignerCategory("Code")]
    public sealed class InputBox : Panel
    {
        Control _leading;
        TextBox _editor;
        readonly List<Control> _trailing = new List<Control>();
        bool _over;

        public InputBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.Opaque, true);
            Cursor = Cursors.IBeam;
            Padding = new Padding(10, 0, 4, 0);
        }

        public Color FillColor { get; set; } = Fluent.Control;

        public int Radius { get; set; } = 6;

        public TextBox Editor { get { return _editor; } }

        public void SetEditor(TextBox editor)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            if (_editor == editor) return;

            _editor = editor;
            editor.BorderStyle = BorderStyle.None;
            editor.BackColor = FillColor;
            editor.ForeColor = Fluent.Text;
            if (!Controls.Contains(editor)) Controls.Add(editor);
            PerformLayout();
            Invalidate();
        }

        internal void ApplyPalette()
        {
            FillColor = Fluent.Control;
            BackColor = FillColor;
            foreach (Control child in Controls)
            {
                if (child.BackColor != FillColor) child.BackColor = FillColor;

                TextBoxBase textBox = child as TextBoxBase;
                if (textBox != null) textBox.ForeColor = Fluent.Text;

                IconLabel icon = child as IconLabel;
                if (icon != null) icon.UpdateColor();
            }
            Invalidate(true);
        }

        public void SetLeading(Control c)
        {
            _leading = c;
            c.BackColor = FillColor;
            if (!Controls.Contains(c)) Controls.Add(c);
            PerformLayout();
        }

        public void AddTrailing(Control c)
        {
            _trailing.Add(c);
            c.BackColor = FillColor;
            if (!Controls.Contains(c)) Controls.Add(c);
            c.VisibleChanged += (s, e) => PerformLayout();
            PerformLayout();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.GotFocus += ChildFocusChanged;
            e.Control.LostFocus += ChildFocusChanged;
            e.Control.MouseEnter += ChildMouseEnter;
            e.Control.MouseMove += ChildMouseMove;
            e.Control.MouseLeave += ChildMouseLeave;
            e.Control.EnabledChanged += ChildEnabledChanged;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            e.Control.GotFocus -= ChildFocusChanged;
            e.Control.LostFocus -= ChildFocusChanged;
            e.Control.MouseEnter -= ChildMouseEnter;
            e.Control.MouseMove -= ChildMouseMove;
            e.Control.MouseLeave -= ChildMouseLeave;
            e.Control.EnabledChanged -= ChildEnabledChanged;
            base.OnControlRemoved(e);
            UpdateHover();
        }

        void ChildFocusChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        void ChildMouseEnter(object sender, EventArgs e)
        {
            UpdateHover();
        }

        void ChildMouseMove(object sender, MouseEventArgs e)
        {
            if (!_over) UpdateHover();
        }

        void ChildMouseLeave(object sender, EventArgs e)
        {
            UpdateHover();
        }

        void ChildEnabledChanged(object sender, EventArgs e)
        {
            UpdateHover();
            Invalidate();
        }

        bool IsPointerInside()
        {
            return Enabled && IsHandleCreated && RectangleToScreen(ClientRectangle).Contains(Cursor.Position);
        }

        void UpdateHover()
        {
            SetHover(IsPointerInside());
        }

        void SetHover(bool value)
        {
            value = value && Enabled;
            if (_over == value) return;
            _over = value;
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            UpdateHover();
            Invalidate();
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            SetHover(true);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_over) UpdateHover();
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            UpdateHover();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_editor != null && _editor.Enabled) _editor.Focus();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (_editor == null) return;

            int left = Padding.Left;
            int right = Width - Padding.Right;
            int iconWidth = Height - Fluent.Scale(this, 6);

            if (_leading != null && _leading.Visible)
            {
                int top = (Height - iconWidth) / 2;
                _leading.SetBounds(Fluent.Scale(this, 4), top, iconWidth, iconWidth);
                left = _leading.Right + Fluent.Scale(this, 2);
            }

            for (int i = _trailing.Count - 1; i >= 0; i--)
            {
                Control c = _trailing[i];
                if (!c.Visible) continue;
                int w = c is Button ? c.Width : iconWidth;
                c.SetBounds(right - w, (Height - iconWidth) / 2, w, iconWidth);
                right = c.Left - Fluent.Scale(this, 2);
            }

            if (_trailing.Any(c => c.Visible)) right -= Fluent.Scale(this, 2);
            else right -= Fluent.Scale(this, 6);

            if (_editor.Multiline)
            {
                int pad = Fluent.Scale(this, 7);
                _editor.SetBounds(left, pad, Math.Max(10, right - left), Math.Max(10, Height - pad * 2));
            }
            else
            {
                int h = _editor.Height;
                _editor.SetBounds(left, (Height - h) / 2, Math.Max(10, right - left), h);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            GraphicsState state = g.Save();
            try
            {
                Color background = Fluent.EffectiveBackColor(Parent);
                using (SolidBrush brush = new SolidBrush(background))
                    g.FillRectangle(brush, ClientRectangle);

                if (Width <= 1 || Height <= 1) return;

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.Default;

                bool focused = ContainsFocus;
                bool enabled = Enabled && (_editor == null || _editor.Enabled);
                float borderWidth = Math.Max(1, Fluent.Scale(this, 1));
                float inset = borderWidth / 2f;
                RectangleF bounds = new RectangleF(inset, inset, Width - borderWidth, Height - borderWidth);
                if (bounds.Width <= 0 || bounds.Height <= 0) return;

                using (GraphicsPath path = Fluent.RoundedRect(bounds, Fluent.Scale(this, Radius)))
                using (SolidBrush fill = new SolidBrush(FillColor))
                {
                    g.FillPath(fill, path);

                    Color border = focused
                        ? Fluent.Blend(Fluent.Border, Fluent.Accent, 0.6f)
                        : (_over && enabled ? Fluent.BorderStrong : Fluent.Border);

                    using (Pen pen = new Pen(border, borderWidth))
                    {
                        pen.LineJoin = LineJoin.Round;
                        g.DrawPath(pen, path);
                    }

                    if (focused)
                    {
                        GraphicsState clipState = g.Save();
                        try
                        {
                            g.SetClip(path, CombineMode.Intersect);
                            int line = Fluent.Scale(this, 2);
                            using (SolidBrush accent = new SolidBrush(Fluent.Accent))
                                g.FillRectangle(accent, 0, Height - line, Width, line);
                        }
                        finally
                        {
                            g.Restore(clipState);
                        }
                    }
                }
            }
            finally
            {
                g.Restore(state);
            }
        }
    }

    // Rounded surface used to group content.
    [DesignerCategory("Code")]
    public sealed class Card : Panel
    {
        public Card()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Fluent.Surface;
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Invalidate();
        }

        public int Radius { get; set; } = 8;

        public Color BorderColor { get; set; } = Fluent.Border;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Fluent.EffectiveBackColor(Parent));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = Fluent.RoundedRect(new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f), Fluent.Scale(this, Radius)))
            using (SolidBrush b = new SolidBrush(BackColor))
            using (Pen p = new Pen(BorderColor))
            {
                g.FillPath(b, path);
                g.DrawPath(p, path);
            }
        }
    }

    // Dialog footer (command bar) with a subtle top divider.
    [DesignerCategory("Code")]
    public sealed class FooterPanel : Panel
    {
        public FooterPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Fluent.Surface;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            using (Pen p = new Pen(Fluent.Border)) e.Graphics.DrawLine(p, 0, 0, Width, 0);
        }
    }

    // Double-buffered list grid with Fluent row visuals: hover tint, rounded accent selection.
    [DesignerCategory("Code")]
    public sealed class FluentGrid : DataGridView
    {
        int _hoverRow = -1;

        public FluentGrid()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
        }

        public void ApplyColors(Font cellFont, Font headerFont)
        {
            BackgroundColor = Fluent.Surface;
            GridColor = Fluent.Border;
            DefaultCellStyle = new DataGridViewCellStyle(DefaultCellStyle)
            {
                BackColor = Fluent.Surface,
                ForeColor = Fluent.Text,
                SelectionBackColor = Fluent.SelectionColor,
                SelectionForeColor = Fluent.Text,
                Font = cellFont ?? DefaultCellStyle.Font
            };
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle(ColumnHeadersDefaultCellStyle)
            {
                BackColor = Fluent.Surface,
                ForeColor = Fluent.TextSecondary,
                SelectionBackColor = Fluent.Surface,
                SelectionForeColor = Fluent.TextSecondary,
                Font = headerFont ?? ColumnHeadersDefaultCellStyle.Font
            };
            RowsDefaultCellStyle = new DataGridViewCellStyle();
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle();
            Invalidate();
        }

        void SetHoverRow(int index)
        {
            if (index == _hoverRow) return;
            int old = _hoverRow;
            _hoverRow = index;
            if (old >= 0 && old < Rows.Count) InvalidateRow(old);
            if (index >= 0 && index < Rows.Count) InvalidateRow(index);
        }

        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellMouseEnter(e);
            SetHoverRow(e.RowIndex);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetHoverRow(-1);
        }

        protected override void OnRowsRemoved(DataGridViewRowsRemovedEventArgs e)
        {
            _hoverRow = -1;
            base.OnRowsRemoved(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnRowPrePaint(DataGridViewRowPrePaintEventArgs e)
        {
            base.OnRowPrePaint(e);
            if (e.Handled) return;

            Rectangle row = new Rectangle(0, e.RowBounds.Y, ClientSize.Width, e.RowBounds.Height);
            using (SolidBrush brush = new SolidBrush(Fluent.Surface))
                e.Graphics.FillRectangle(brush, row);

            bool selected = (e.State & DataGridViewElementStates.Selected) != 0;
            bool hovered = e.RowIndex == _hoverRow;
            if (selected || hovered)
            {
                int inset = Fluent.Scale(this, 4);
                RectangleF bounds = new RectangleF(row.X + inset, row.Y + 2, row.Width - inset * 2 - 1, row.Height - 5);
                Color fill = selected
                    ? Fluent.SelectionColor
                    : Fluent.ControlHover;
                SmoothingMode old = e.Graphics.SmoothingMode;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = Fluent.RoundedRect(bounds, Fluent.Scale(this, 5)))
                using (SolidBrush brush = new SolidBrush(fill))
                    e.Graphics.FillPath(brush, path);
                e.Graphics.SmoothingMode = old;
            }

            e.PaintParts &= ~(DataGridViewPaintParts.Background | DataGridViewPaintParts.SelectionBackground |
                              DataGridViewPaintParts.Focus);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            base.OnCellPainting(e);
            if (e.Handled || (e.PaintParts & DataGridViewPaintParts.ContentForeground) == 0) return;

            string text = Convert.ToString(e.FormattedValue);
            e.Paint(e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.ContentForeground);
            e.Handled = true;
            if (string.IsNullOrEmpty(text)) return;

            DataGridViewCellStyle style = e.CellStyle;
            bool selected = e.RowIndex >= 0 && (e.State & DataGridViewElementStates.Selected) != 0;
            Color color = selected ? style.SelectionForeColor : style.ForeColor;
            Rectangle bounds = e.CellBounds;
            bounds = new Rectangle(bounds.X + style.Padding.Left, bounds.Y + style.Padding.Top,
                bounds.Width - style.Padding.Horizontal - 1, bounds.Height - style.Padding.Vertical - 1);

            TextFormatFlags flags = Fluent.AlignmentFlags(style.Alignment) | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            flags |= style.WrapMode == DataGridViewTriState.True ? TextFormatFlags.WordBreak : TextFormatFlags.SingleLine;
            Fluent.DrawText(e.Graphics, text, style.Font, bounds, color, flags);
        }
    }

    // Label with grayscale antialiased text.
    [DesignerCategory("Code")]
    public class FluentLabel : Label
    {
        public FluentLabel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }

        TextFormatFlags Flags()
        {
            TextFormatFlags flags = Fluent.AlignmentFlags(TextAlign) | TextFormatFlags.WordBreak;
            if (AutoEllipsis) flags |= TextFormatFlags.EndEllipsis;
            if (!UseMnemonic) flags |= TextFormatFlags.NoPrefix;
            else if (!ShowKeyboardCues) flags |= TextFormatFlags.HidePrefix;
            if (RightToLeft == RightToLeft.Yes) flags |= TextFormatFlags.RightToLeft;
            return flags;
        }

        // auto-size with the same measurement used for drawing, so the text never wraps unexpectedly
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!AutoSize || string.IsNullOrEmpty(Text)) return base.GetPreferredSize(proposedSize);
            Size text = Fluent.MeasureText(Text, Font, Size.Empty, Flags() & ~TextFormatFlags.WordBreak);
            Size size = new Size(text.Width + Padding.Horizontal + 1, text.Height + Padding.Vertical);
            Size baseSize = base.GetPreferredSize(proposedSize);
            return new Size(Math.Max(size.Width, baseSize.Width), Math.Max(size.Height, baseSize.Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle r = new Rectangle(Padding.Left, Padding.Top, ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical);
            if (Image != null) DrawImage(e.Graphics, Image, r, ImageAlign);

            Fluent.DrawText(e.Graphics, Text, Font, r, Enabled ? ForeColor : Fluent.TextDisabled, Flags());
        }
    }


    [DesignerCategory("Code")]
    public sealed class DropHintLabel : FluentLabel
    {
        private string _hintText;
        private string _subtitle;

        public string HintText
        {
            get { return _hintText; }
            set
            {
                if (string.Equals(_hintText, value, StringComparison.Ordinal)) return;
                _hintText = value;
                Invalidate();
            }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set
            {
                if (string.Equals(_subtitle, value, StringComparison.Ordinal)) return;
                _subtitle = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(_hintText) || !string.Equals(Text, _hintText, StringComparison.Ordinal))
            {
                base.OnPaint(e);
                return;
            }

            Rectangle bounds = new Rectangle(Padding.Left, Padding.Top,
                ClientSize.Width - Padding.Horizontal, ClientSize.Height - Padding.Vertical);
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            int scaledIconSize = Fluent.Scale(this, 64);
            int gapAfterIcon = Fluent.Scale(this, 14);
            int gapAfterTitle = Fluent.Scale(this, 5);
            int titleHeight = Fluent.Scale(this, 30);
            int subtitleHeight = Fluent.Scale(this, 42);
            int totalHeight = scaledIconSize + gapAfterIcon + titleHeight + gapAfterTitle + subtitleHeight;
            int top = bounds.Top + Math.Max(0, (bounds.Height - totalHeight) / 2);
            int center = bounds.Left + bounds.Width / 2;
            int iconLeft = center - scaledIconSize / 2;
            int iconTop = top;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush iconBackground = new SolidBrush(Fluent.Blend(Fluent.Accent, Fluent.Surface, 0.88f)))
                e.Graphics.FillEllipse(iconBackground, new Rectangle(iconLeft, iconTop, scaledIconSize, scaledIconSize));

            string glyph = Fluent.HasIcons ? Fluent.Glyphs.Download : "↓";
            Font glyphFont = Fluent.HasIcons ? Fluent.IconFont(27f) : Fluent.UiSemibold(27f);
            Rectangle glyphBounds = new Rectangle(iconLeft, iconTop, scaledIconSize, scaledIconSize);
            Fluent.DrawText(e.Graphics, glyph, glyphFont, glyphBounds, Fluent.AccentTextOn(Fluent.Blend(Fluent.Accent, Fluent.Surface, 0.88f)),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            int titleTop = iconTop + scaledIconSize + gapAfterIcon;
            Rectangle titleBounds = new Rectangle(bounds.Left + Fluent.Scale(this, 12), titleTop,
                bounds.Width - Fluent.Scale(this, 24), titleHeight);
            Fluent.DrawText(e.Graphics, Text, Fluent.UiSemibold(14f), titleBounds, Fluent.Text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

            if (!string.IsNullOrWhiteSpace(_subtitle))
            {
                int subtitleTop = titleBounds.Bottom + gapAfterTitle;
                Rectangle subtitleBounds = new Rectangle(bounds.Left + Fluent.Scale(this, 20), subtitleTop,
                    bounds.Width - Fluent.Scale(this, 40), subtitleHeight);
                Fluent.DrawText(e.Graphics, _subtitle, Fluent.Ui(9.5f), subtitleBounds, Fluent.TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            }
        }
    }

    // Distinct type kept so the designer files stay stable; uses native ClearType rendering.
    [DesignerCategory("Code")]
    public sealed class FluentTextBox : TextBox
    {
    }
}
