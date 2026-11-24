using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hashing
{
    public enum RadioStyle
    {
        Radio,
        Swatch,
        Segment
    }

    // Owner-drawn radio: classic dot, color swatch, or segmented pill.
    [DesignerCategory("Code")]
    public sealed class MoonRadio : RadioButton
    {
        bool _over;

        public MoonRadio()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            ForeColor = Fluent.Text;
            Font = Fluent.Ui(10f);
            AutoSize = false;
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

        [DefaultValue(RadioStyle.Radio)]
        public RadioStyle Style { get; set; } = RadioStyle.Radio;

        public Color SwatchColor { get; set; } = Color.Gray;

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs eventargs) { _over = true; Invalidate(); base.OnMouseEnter(eventargs); }
        protected override void OnMouseLeave(EventArgs eventargs) { _over = false; Invalidate(); base.OnMouseLeave(eventargs); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color back = Fluent.EffectiveBackColor(Parent);
            g.Clear(back);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color accent = Fluent.Accent;

            switch (Style)
            {
                case RadioStyle.Segment:
                    {
                        RectangleF r = new RectangleF(1.5f, 1.5f, Width - 3f, Height - 3f);
                        Color fill = Checked ? accent : (_over ? Fluent.ControlHover : back);
                        using (GraphicsPath path = Fluent.RoundedRect(r, Fluent.Scale(this, 5)))
                        using (SolidBrush b = new SolidBrush(fill))
                        {
                            if (fill != back) g.FillPath(b, path);
                        }
                        Fluent.DrawText(g, Text, Font, ClientRectangle, Checked ? Fluent.OnAccent : (_over ? Fluent.Text : Fluent.TextSecondary),
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        break;
                    }
                case RadioStyle.Swatch:
                    {
                        int size = Fluent.Scale(this, 18);
                        RectangleF dot = new RectangleF(Fluent.Scale(this, 3), (Height - size) / 2f, size, size);
                        if (Checked || _over)
                        {
                            float grow = Fluent.Scale(this, 2);
                            RectangleF ring = RectangleF.Inflate(dot, grow, grow);
                            using (Pen p = new Pen(Checked ? Fluent.Text : Fluent.BorderStrong, Fluent.Scale(this, 2))) g.DrawEllipse(p, ring);
                        }
                        using (SolidBrush b = new SolidBrush(SwatchColor)) g.FillEllipse(b, dot);
                        if (Checked)
                        {
                            Color mark = Fluent.OnColor(SwatchColor);
                            using (Pen p = new Pen(mark, Math.Max(Fluent.Scale(this, 1), size / 11f)))
                            {
                                p.StartCap = p.EndCap = LineCap.Round;
                                g.DrawLine(p,
                                    dot.X + size * 0.28f, dot.Y + size * 0.52f,
                                    dot.X + size * 0.44f, dot.Y + size * 0.68f);
                                g.DrawLine(p,
                                    dot.X + size * 0.44f, dot.Y + size * 0.68f,
                                    dot.X + size * 0.74f, dot.Y + size * 0.36f);
                            }
                        }
                        int left = (int)dot.Right + Fluent.Scale(this, 10);
                        Fluent.DrawText(g, Text, Font, new Rectangle(left, 0, Width - left, Height), Checked ? Fluent.Text : Fluent.TextSecondary,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine |
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        break;
                    }
                default:
                    {
                        int size = Fluent.Scale(this, 18);
                        RectangleF outer = new RectangleF(0.5f, (Height - size) / 2f, size, size);
                        if (Checked)
                        {
                            using (SolidBrush b = new SolidBrush(accent)) g.FillEllipse(b, outer);
                            float inner = size * (_over ? 0.5f : 0.42f);
                            using (SolidBrush b = new SolidBrush(Fluent.OnAccent))
                                g.FillEllipse(b, outer.X + (size - inner) / 2f, outer.Y + (size - inner) / 2f, inner, inner);
                        }
                        else
                        {
                            using (SolidBrush b = new SolidBrush(_over ? Fluent.ControlHover : Fluent.Control)) g.FillEllipse(b, outer);
                            using (Pen p = new Pen(Fluent.TextSecondary)) g.DrawEllipse(p, outer);
                        }
                        int left = size + Fluent.Scale(this, 10);
                        Fluent.DrawText(g, Text, Font, new Rectangle(left, 0, Width - left, Height), Fluent.Text,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine |
                            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        break;
                    }
            }

            if (Focused && ShowFocusCues)
            {
                using (Pen p = new Pen(Fluent.TextSecondary) { DashStyle = DashStyle.Dot }) g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }
        }
    }
}
