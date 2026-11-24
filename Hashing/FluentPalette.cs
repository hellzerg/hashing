using System;
using System.Drawing;

namespace Hashing
{
    internal sealed class FluentPalette
    {
        internal static FluentPalette Current = Create(Theme.Amethyst, AppearanceMode.Dark);

        internal readonly bool IsLight;
        internal readonly Color Background;
        internal readonly Color Surface;
        internal readonly Color SurfaceAlt;
        internal readonly Color SurfaceHover;
        internal readonly Color Border;
        internal readonly Color Text;
        internal readonly Color SecondaryText;
        internal readonly Color MutedText;
        internal readonly Color Accent;
        internal readonly Color AccentHover;
        internal readonly Color AccentText;
        internal readonly Color Selection;

        private FluentPalette(bool isLight, Color accent, Color accentHover)
        {
            IsLight = isLight;
            Accent = accent;
            AccentHover = accentHover;
            Background = isLight ? Color.FromArgb(245, 245, 245) : Color.FromArgb(30, 30, 30);
            Surface = isLight ? Color.White : Color.FromArgb(39, 39, 39);
            SurfaceAlt = isLight ? Color.FromArgb(250, 250, 250) : Color.FromArgb(35, 35, 35);
            SurfaceHover = isLight ? Color.FromArgb(239, 239, 239) : Color.FromArgb(54, 54, 54);
            Border = isLight ? Color.FromArgb(222, 222, 222) : Color.FromArgb(65, 65, 65);
            Text = isLight ? Color.FromArgb(28, 28, 28) : Color.FromArgb(245, 245, 245);
            SecondaryText = isLight ? Color.FromArgb(92, 92, 92) : Color.FromArgb(190, 190, 190);
            MutedText = isLight ? Color.FromArgb(112, 112, 112) : Color.FromArgb(145, 145, 145);
            AccentText = GetLuminance(accent) > 0.42 ? Color.FromArgb(24, 24, 24) : Color.White;
            Selection = isLight ? Blend(accent, Color.White, 0.86f) : Blend(accent, Background, 0.72f);
        }

        internal static FluentPalette Create(Theme theme, AppearanceMode appearance)
        {
            Color accent;
            Color hover;

            switch (theme)
            {
                case Theme.Amber:
                    accent = Color.FromArgb(195, 146, 0);
                    hover = Color.FromArgb(171, 128, 0);
                    break;
                case Theme.Jade:
                    accent = Color.FromArgb(70, 175, 105);
                    hover = Color.FromArgb(61, 153, 92);
                    break;
                case Theme.Ruby:
                    accent = Color.FromArgb(205, 22, 39);
                    hover = Color.FromArgb(155, 17, 30);
                    break;
                case Theme.Silver:
                    accent = Color.FromArgb(128, 128, 128);
                    hover = Color.FromArgb(96, 96, 96);
                    break;
                case Theme.Azurite:
                    accent = Color.FromArgb(0, 127, 255);
                    hover = Color.FromArgb(0, 111, 223);
                    break;
                default:
                    accent = Color.FromArgb(153, 102, 204);
                    hover = Color.FromArgb(134, 89, 179);
                    break;
            }

            bool light = appearance == AppearanceMode.Light ||
                         (appearance == AppearanceMode.System && Options.IsSystemLightTheme());

            return new FluentPalette(light, accent, hover);
        }

        private static Color Blend(Color foreground, Color background, float amount)
        {
            return Color.FromArgb(
                (int)(foreground.R * (1f - amount) + background.R * amount),
                (int)(foreground.G * (1f - amount) + background.G * amount),
                (int)(foreground.B * (1f - amount) + background.B * amount));
        }

        private static double GetLuminance(Color color)
        {
            return (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) / 255d;
        }
    }
}
