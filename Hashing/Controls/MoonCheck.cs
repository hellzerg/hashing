using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hashing
{
    public enum CheckStyle
    {
        Checkbox,
        Switch
    }

    // Owner-drawn checkbox / toggle switch.
    [DesignerCategory("Code")]
    public sealed class MoonCheck : CheckBox
    {
        readonly Timer _animation = new Timer { Interval = 15 };
        float _progress;
        bool _over;

        public MoonCheck()
        {
            ParentChanged += (s, e) => Invalidate();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            ForeColor = Fluent.Text;
            Font = Fluent.Ui(10f);
            AutoSize = false;

            _animation.Tick += (s, e) =>
            {
                float target = Checked ? 1f : 0f;
                _progress += Math.Sign(target - _progress) * 0.2f;
                if (Math.Abs(target - _progress) < 0.2f)
                {
                    _progress = target;
                    _animation.Stop();
                }
                Invalidate();
            };
        }

        [DefaultValue(CheckStyle.Checkbox)]
        public CheckStyle Style { get; set; } = CheckStyle.Checkbox;

        protected override void Dispose(bool disposing)
        {
            if (disposing) _animation.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            if (IsHandleCreated && Visible) _animation.Start();
            else { _progress = Checked ? 1f : 0f; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs eventargs) { _over = true; Invalidate(); base.OnMouseEnter(eventargs); }
        protected override void OnMouseLeave(EventArgs eventargs) { _over = false; Invalidate(); base.OnMouseLeave(eventargs); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Fluent.EffectiveBackColor(Parent));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color accent = Fluent.Accent;
            Color text = Enabled ? Fluent.Text : Fluent.TextDisabled;
            int textLeft;

            if (Style == CheckStyle.Switch)
            {
                int h = Fluent.Scale(this, 20);
                int w = Fluent.Scale(this, 40);
                RectangleF track = new RectangleF(0.5f, (Height - h) / 2f, w, h);
                Color offFill = _over && Enabled ? Fluent.ControlHover : Fluent.Control;
                Color fill = Fluent.Blend(offFill, Enabled ? accent : Fluent.BorderStrong, _progress);

                using (GraphicsPath path = Fluent.RoundedRect(track, h / 2f))
                {
                    using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, path);
                    if (_progress < 1f)
                    {
                        using (Pen p = new Pen(Color.FromArgb((int)(255 * (1 - _progress)), Fluent.TextSecondary))) g.DrawPath(p, path);
                    }
                }

                float knob = h - Fluent.Scale(this, 8) + (_over && Enabled ? 2 : 0);
                float minX = track.X + (h - knob) / 2f;
                float maxX = track.Right - (h + knob) / 2f;
                float x = minX + (maxX - minX) * _progress;
                Color knobColor = Fluent.Blend(Fluent.TextSecondary, Fluent.OnAccent, _progress);
                if (!Enabled) knobColor = Fluent.TextDisabled;
                using (SolidBrush b = new SolidBrush(knobColor))
                {
                    g.FillEllipse(b, x, track.Y + (h - knob) / 2f, knob, knob);
                }
                textLeft = w + Fluent.Scale(this, 12);
            }
            else
            {
                int size = Fluent.Scale(this, 18);
                RectangleF box = new RectangleF(0.5f, (Height - size) / 2f, size, size);
                using (GraphicsPath path = Fluent.RoundedRect(box, Fluent.Scale(this, 4)))
                {
                    Color off = _over && Enabled ? Fluent.ControlHover : Fluent.Control;
                    using (SolidBrush b = new SolidBrush(Fluent.Blend(off, Enabled ? accent : Fluent.BorderStrong, _progress))) g.FillPath(b, path);
                    if (_progress < 1f)
                    {
                        using (Pen p = new Pen(Color.FromArgb((int)(255 * (1 - _progress)), Fluent.TextSecondary))) g.DrawPath(p, path);
                    }
                }

                if (_progress > 0f)
                {
                    using (Pen p = new Pen(Color.FromArgb((int)(255 * _progress), Fluent.OnAccent), Math.Max(1.5f, size / 10f)))
                    {
                        p.StartCap = p.EndCap = LineCap.Round;
                        p.LineJoin = LineJoin.Round;
                        g.DrawLine(p,
                            box.X + size * 0.25f, box.Y + size * 0.52f,
                            box.X + size * 0.43f, box.Y + size * 0.70f);
                        g.DrawLine(p,
                            box.X + size * 0.43f, box.Y + size * 0.70f,
                            box.X + size * 0.76f, box.Y + size * 0.33f);
                    }
                }
                textLeft = size + Fluent.Scale(this, 10);
            }

            Fluent.DrawText(g, Text, Font, new Rectangle(textLeft, 0, Width - textLeft, Height), text,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            if (Focused && ShowFocusCues)
            {
                using (Pen p = new Pen(Fluent.TextSecondary) { DashStyle = DashStyle.Dot })
                {
                    g.DrawRectangle(p, textLeft - 2, 2, Math.Min(Width - textLeft, Fluent.MeasureText(Text, Font).Width + 2), Height - 5);
                }
            }
        }
    }
}
