using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Hashing
{
    // Flat, rounded-highlight renderer for context menus with a fixed icon column,
    // vertically centered icons/text and right-aligned shortcuts.
    internal sealed class MoonMenuRenderer : ToolStripProfessionalRenderer
    {
        internal MoonMenuRenderer() : base(new MoonColors())
        {
            RoundedEdges = false;
        }

        static int Scale(ToolStripItem item, int value)
        {
            int dpi = item.Owner != null ? item.Owner.DeviceDpi : 96;
            return (int)Math.Round(value * dpi / 96.0);
        }

        // Left edge of the icon column, relative to the item.
        internal static int IconLeft(ToolStripItem item) { return Scale(item, 10); }

        internal static int IconSize(ToolStripItem item) { return Scale(item, 16); }

        // Left edge of the text column; identical for every item so labels line up.
        internal static int TextLeft(ToolStripItem item)
        {
            return IconLeft(item) + IconSize(item) + Scale(item, 12);
        }

        static int ArrowWidth(ToolStripItem item) { return Scale(item, 22); }

        // Width an item needs with the fixed icon/text/shortcut/arrow columns used by this renderer.
        internal static int RequiredWidth(ToolStripMenuItem item)
        {
            Font font = item.Font;
            int width = TextLeft(item) + Fluent.MeasureText(item.Text ?? string.Empty, font, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width;
            if (!string.IsNullOrEmpty(item.ShortcutKeyDisplayString))
            {
                width += Scale(item, 24) + Fluent.MeasureText(item.ShortcutKeyDisplayString, font, Size.Empty, TextFormatFlags.NoPadding).Width;
            }
            width += Scale(item, 14);
            if (HasSubmenu(item)) width += ArrowWidth(item);
            return width;
        }

        static bool HasSubmenu(ToolStripItem item)
        {
            ToolStripMenuItem mi = item as ToolStripMenuItem;
            return mi != null && mi.HasDropDownItems;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.Clear(Fluent.Menu);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            using (Pen p = new Pen(Fluent.Border))
            {
                e.Graphics.DrawRectangle(p, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected || !e.Item.Enabled) return;

            RectangleF r = new RectangleF(1.5f, 1.5f, e.Item.Width - 3f, e.Item.Height - 3f);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = Fluent.RoundedRect(r, Scale(e.Item, 4)))
            using (SolidBrush b = new SolidBrush(Fluent.ControlHover))
            {
                e.Graphics.FillPath(b, path);
            }
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            Image img = e.Image ?? e.Item.Image;
            if (img == null) return;

            int size = Math.Min(IconSize(e.Item), Math.Min(img.Width, e.Item.Height));
            Rectangle r = new Rectangle(IconLeft(e.Item) + (IconSize(e.Item) - size) / 2, (e.Item.Height - size) / 2, size, size);

            if (e.Item.Enabled)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                if (Fluent.IsMenuGlyph(e.Item))
                {
                    e.Graphics.DrawImage(img, r);
                }
                else
                {
                    Color tint = Fluent.TextSecondary;
                    ColorMatrix matrix = new ColorMatrix(new float[][]
                    {
                        new float[] { tint.R / 255f, 0f, 0f, 0f, 0f },
                        new float[] { 0f, tint.G / 255f, 0f, 0f, 0f },
                        new float[] { 0f, 0f, tint.B / 255f, 0f, 0f },
                        new float[] { 0f, 0f, 0f, 1f, 0f },
                        new float[] { 0f, 0f, 0f, 0f, 1f }
                    });
                    using (ImageAttributes attributes = new ImageAttributes())
                    {
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        e.Graphics.DrawImage(img, r, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
            }
            else
            {
                ControlPaint.DrawImageDisabled(e.Graphics, img, r.X, r.Y, Fluent.Menu);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            ToolStripMenuItem item = e.Item as ToolStripMenuItem;
            if (item == null || item.IsOnDropDown == false)
            {
                base.OnRenderItemText(e);
                return;
            }

            bool isShortcut = !string.IsNullOrEmpty(item.ShortcutKeyDisplayString) && e.Text == item.ShortcutKeyDisplayString;

            Color color;
            if (!item.Enabled) color = Fluent.TextDisabled;
            else if (isShortcut) color = Fluent.TextSecondary;
            else if (e.Item.Selected && item.ForeColor.ToArgb() == Fluent.AccentText.ToArgb()) color = Fluent.AccentHoverText;
            else color = item.ForeColor;

            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;

            int right = item.Width - Scale(item, 12) - (HasSubmenu(item) ? ArrowWidth(item) : 0);
            Rectangle bounds;

            if (isShortcut)
            {
                bounds = new Rectangle(TextLeft(item), 0, Math.Max(0, right - TextLeft(item)), item.Height);
                flags |= TextFormatFlags.Right;
            }
            else
            {
                int shortcut = string.IsNullOrEmpty(item.ShortcutKeyDisplayString) ? 0
                    : Fluent.MeasureText(item.ShortcutKeyDisplayString, e.TextFont, Size.Empty, TextFormatFlags.NoPadding).Width + Scale(item, 24);
                bounds = new Rectangle(TextLeft(item), 0, Math.Max(0, right - shortcut - TextLeft(item)), item.Height);
                flags |= TextFormatFlags.Left;
            }

            Fluent.DrawText(e.Graphics, e.Text, e.TextFont, bounds, color, flags);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int y = e.Item.Height / 2;
            int inset = Scale(e.Item, 8);
            using (Pen p = new Pen(Fluent.Border))
            {
                e.Graphics.DrawLine(p, inset, y, e.Item.Width - inset, y);
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            ToolStripItem item = e.Item;
            if (item == null || !item.IsOnDropDown || !Fluent.HasIcons)
            {
                e.ArrowColor = Fluent.TextSecondary;
                base.OnRenderArrow(e);
                return;
            }

            Rectangle r = new Rectangle(item.Width - ArrowWidth(item) - Scale(item, 4), 0, ArrowWidth(item), item.Height);
            Fluent.DrawText(e.Graphics, "\uE76C", Fluent.IconFont(7.5f), r,
                item.Enabled ? Fluent.TextSecondary : Fluent.TextDisabled,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }
    }

    internal sealed class MoonColors : ProfessionalColorTable
    {
        public override Color SeparatorLight { get { return Fluent.Border; } }
        public override Color SeparatorDark { get { return Fluent.Border; } }
        public override Color ToolStripDropDownBackground { get { return Fluent.Menu; } }
        public override Color ImageMarginGradientBegin { get { return Fluent.Menu; } }
        public override Color ImageMarginGradientMiddle { get { return Fluent.Menu; } }
        public override Color ImageMarginGradientEnd { get { return Fluent.Menu; } }
        public override Color ToolStripBorder { get { return Fluent.Border; } }
        public override Color MenuBorder { get { return Fluent.Border; } }
        public override Color MenuItemSelected { get { return Fluent.ControlHover; } }
        public override Color MenuItemSelectedGradientBegin { get { return Fluent.ControlHover; } }
        public override Color MenuItemSelectedGradientEnd { get { return Fluent.ControlHover; } }
        public override Color MenuItemBorder { get { return Color.Transparent; } }
    }
}
