using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Hashing
{
    internal static class FluentMessageBox
    {
        internal static DialogResult Show(string message)
        {
            return Show(null, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, false);
        }

        internal static DialogResult Show(IWin32Window owner, string message)
        {
            return Show(owner, message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, false);
        }

        internal static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, message, caption, buttons, icon, false);
        }

        internal static DialogResult Show(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, message, caption, buttons, icon, false);
        }

        internal static DialogResult ShowTopMost(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, message, caption, buttons, icon, true);
        }

        internal static DialogResult ShowTopMost(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, message, caption, buttons, icon, true);
        }

        static DialogResult Show(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool forceTopMost)
        {
            using (FluentMessageDialog dialog = new FluentMessageDialog(message, caption, buttons, icon, forceTopMost || (owner as Form)?.TopMost == true))
            {
                return owner == null ? dialog.ShowDialog() : dialog.ShowDialog(owner);
            }
        }
    }

    internal sealed class FluentMessageDialog : Form
    {
        sealed class ButtonDefinition
        {
            internal readonly string Text;
            internal readonly DialogResult Result;
            internal readonly bool Primary;

            internal ButtonDefinition(string text, DialogResult result, bool primary)
            {
                Text = text;
                Result = result;
                Primary = primary;
            }
        }

        readonly string _message;
        readonly string _caption;
        readonly MessageBoxButtons _buttons;
        readonly bool _hasIcon;
        readonly Card _card;
        readonly FluentLabel _heading;
        readonly FluentLabel _messageLabel;
        readonly Panel _messageViewport;
        readonly MessageIconControl _iconControl;
        readonly List<ModernButton> _actionButtons = new List<ModernButton>(3);
        DialogResult _closeResult = DialogResult.Cancel;

        internal FluentMessageDialog(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool topMost)
        {
            _message = message ?? string.Empty;
            _caption = caption ?? string.Empty;
            _buttons = buttons;
            _hasIcon = icon != MessageBoxIcon.None;

            Text = _caption.Length == 0 ? "Hashing" : _caption;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            ShowIcon = false;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            AutoScaleMode = AutoScaleMode.Dpi;
            KeyPreview = true;
            TopMost = topMost;
            Font = Fluent.Ui(10f);
            BackColor = Fluent.Background;
            ForeColor = Fluent.Text;

            _card = new Card { Name = "messageCard", Radius = 10 };
            _heading = new FluentLabel
            {
                Name = "messageHeading",
                AutoSize = false,
                Text = _caption,
                Font = Fluent.UiSemibold(11.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = Options.ThemeFlag,
                Visible = _caption.Length > 0
            };
            _messageViewport = new Panel
            {
                Name = "messageViewport",
                AutoScroll = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent
            };
            _messageLabel = new FluentLabel
            {
                Name = "messageText",
                AutoSize = false,
                Text = _message,
                Font = Fluent.Ui(10f),
                ForeColor = Fluent.Text,
                TextAlign = ContentAlignment.TopLeft,
                UseMnemonic = false
            };
            _messageViewport.Controls.Add(_messageLabel);
            _iconControl = new MessageIconControl(icon) { Visible = _hasIcon };

            _card.Controls.Add(_heading);
            _card.Controls.Add(_messageViewport);
            if (_hasIcon) _card.Controls.Add(_iconControl);
            Controls.Add(_card);

            BuildButtons();
            Options.ApplyTheme(this);
        }

        void BuildButtons()
        {
            List<ButtonDefinition> definitions = GetButtonDefinitions(_buttons);
            foreach (ButtonDefinition definition in definitions)
            {
                ModernButton button = new ModernButton
                {
                    Text = definition.Text,
                    Variant = definition.Primary ? ButtonVariant.Primary : ButtonVariant.Secondary,
                    Radius = 6,
                    TabStop = true
                };
                button.Click += (sender, args) =>
                {
                    DialogResult = definition.Result;
                    Close();
                };
                _actionButtons.Add(button);
                _card.Controls.Add(button);
                if (definition.Primary) AcceptButton = button;
                if (definition.Result == DialogResult.Cancel) CancelButton = button;
            }

            if (AcceptButton == null && _actionButtons.Count > 0)
                AcceptButton = _actionButtons[0];

            if (_buttons == MessageBoxButtons.YesNo)
                _closeResult = DialogResult.No;
            else if (_buttons == MessageBoxButtons.AbortRetryIgnore)
                _closeResult = DialogResult.Abort;
            else if (_buttons == MessageBoxButtons.RetryCancel || _buttons == MessageBoxButtons.OKCancel || _buttons == MessageBoxButtons.YesNoCancel)
                _closeResult = DialogResult.Cancel;
            else
                _closeResult = DialogResult.OK;
        }

        List<ButtonDefinition> GetButtonDefinitions(MessageBoxButtons buttons)
        {
            string ok = ButtonText(DialogResult.OK);
            string cancel = ButtonText(DialogResult.Cancel);
            string yes = ButtonText(DialogResult.Yes);
            string no = ButtonText(DialogResult.No);
            string retry = ButtonText(DialogResult.Retry);
            string abort = ButtonText(DialogResult.Abort);
            string ignore = ButtonText(DialogResult.Ignore);

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    return new List<ButtonDefinition> { new ButtonDefinition(ok, DialogResult.OK, true) };
                case MessageBoxButtons.OKCancel:
                    return new List<ButtonDefinition> { new ButtonDefinition(ok, DialogResult.OK, true), new ButtonDefinition(cancel, DialogResult.Cancel, false) };
                case MessageBoxButtons.YesNo:
                    return new List<ButtonDefinition> { new ButtonDefinition(yes, DialogResult.Yes, true), new ButtonDefinition(no, DialogResult.No, false) };
                case MessageBoxButtons.YesNoCancel:
                    return new List<ButtonDefinition> { new ButtonDefinition(yes, DialogResult.Yes, true), new ButtonDefinition(no, DialogResult.No, false), new ButtonDefinition(cancel, DialogResult.Cancel, false) };
                case MessageBoxButtons.RetryCancel:
                    return new List<ButtonDefinition> { new ButtonDefinition(retry, DialogResult.Retry, true), new ButtonDefinition(cancel, DialogResult.Cancel, false) };
                case MessageBoxButtons.AbortRetryIgnore:
                    return new List<ButtonDefinition> { new ButtonDefinition(abort, DialogResult.Abort, true), new ButtonDefinition(retry, DialogResult.Retry, false), new ButtonDefinition(ignore, DialogResult.Ignore, false) };
                default:
                    throw new ArgumentOutOfRangeException("buttons");
            }
        }

        static string ButtonText(DialogResult result)
        {
            LanguageCode language = Options.CurrentOptions == null ? LanguageCode.EN : Options.CurrentOptions.LanguageCode;
            switch (language)
            {
                case LanguageCode.EL:
                    switch (result)
                    {
                        case DialogResult.OK: return "Εντάξει";
                        case DialogResult.Cancel: return "Άκυρο";
                        case DialogResult.Yes: return "Ναι";
                        case DialogResult.No: return "Όχι";
                        case DialogResult.Retry: return "Επανάληψη";
                        case DialogResult.Abort: return "Ματαίωση";
                        case DialogResult.Ignore: return "Παράβλεψη";
                    }
                    break;
                case LanguageCode.CN:
                    switch (result)
                    {
                        case DialogResult.OK: return "确定";
                        case DialogResult.Cancel: return "取消";
                        case DialogResult.Yes: return "是";
                        case DialogResult.No: return "否";
                        case DialogResult.Retry: return "重试";
                        case DialogResult.Abort: return "中止";
                        case DialogResult.Ignore: return "忽略";
                    }
                    break;
                case LanguageCode.DE:
                    switch (result)
                    {
                        case DialogResult.OK: return "OK";
                        case DialogResult.Cancel: return "Abbrechen";
                        case DialogResult.Yes: return "Ja";
                        case DialogResult.No: return "Nein";
                        case DialogResult.Retry: return "Wiederholen";
                        case DialogResult.Abort: return "Abbrechen";
                        case DialogResult.Ignore: return "Ignorieren";
                    }
                    break;
            }

            switch (result)
            {
                case DialogResult.OK: return "OK";
                case DialogResult.Cancel: return "Cancel";
                case DialogResult.Yes: return "Yes";
                case DialogResult.No: return "No";
                case DialogResult.Retry: return "Retry";
                case DialogResult.Abort: return "Abort";
                case DialogResult.Ignore: return "Ignore";
                default: return result.ToString();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LayoutDialog();
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            LayoutDialog();
        }

        void LayoutDialog()
        {
            int scale = DeviceDpi;
            Func<int, int> s = value => (int)Math.Round(value * scale / 96.0);
            int width = s(620);
            int margin = s(18);
            int cardWidth = width - margin * 2;
            int iconSize = s(40);
            int iconRowHeight = s(48);
            int contentLeft = s(_hasIcon ? 72 : 20);
            int contentWidth = cardWidth - contentLeft - s(20);
            int top = s(_caption.Length > 0 ? 48 : 22);
            Rectangle workArea = Screen.FromControl(this).WorkingArea;
            int maxMessageHeight = Math.Max(s(100), Math.Min(s(430), workArea.Height - s(172)));
            TextFormatFlags measureFlags = TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix;
            Size measured = Fluent.MeasureText(_message, _messageLabel.Font, new Size(contentWidth, int.MaxValue), measureFlags);
            int textHeight = Math.Max(s(24), measured.Height + s(4));
            int contentHeight = Math.Max(textHeight, _hasIcon ? iconRowHeight : s(24));
            bool needsVerticalScroll = contentHeight > maxMessageHeight;
            int scrollWidth = Math.Max(s(17), SystemInformation.VerticalScrollBarWidth);
            int messageWidth = needsVerticalScroll ? Math.Max(s(1), contentWidth - scrollWidth) : contentWidth;
            if (needsVerticalScroll)
            {
                measured = Fluent.MeasureText(_message, _messageLabel.Font, new Size(messageWidth, int.MaxValue), measureFlags);
                textHeight = Math.Max(s(24), measured.Height + s(4));
                contentHeight = Math.Max(textHeight, _hasIcon ? iconRowHeight : s(24));
            }

            int viewportHeight = Math.Min(contentHeight, maxMessageHeight);
            int buttonHeight = s(32);
            int buttonTopGap = s(12);
            int cardBottomPadding = s(14);
            int cardHeight = top + viewportHeight + buttonTopGap + buttonHeight + cardBottomPadding;

            ClientSize = new Size(width, margin + cardHeight + margin);

            _card.SetBounds(margin, margin, cardWidth, cardHeight);
            if (_heading.Visible)
                _heading.SetBounds(s(20), s(12), cardWidth - s(40), s(28));

            int bodyTop = top;
            _messageViewport.SetBounds(contentLeft, bodyTop, contentWidth, viewportHeight);
            _messageViewport.AutoScrollMinSize = new Size(messageWidth, contentHeight);
            int messageTop = Math.Max(0, (contentHeight - textHeight) / 2);
            _messageLabel.SetBounds(0, messageTop, messageWidth, textHeight);

            if (_hasIcon)
                _iconControl.SetBounds(s(20), bodyTop + Math.Max(0, (viewportHeight - iconSize) / 2), iconSize, iconSize);

            int buttonGap = s(8);
            int x = cardWidth - s(20);
            int y = cardHeight - cardBottomPadding - buttonHeight;
            for (int i = 0; i < _actionButtons.Count; i++)
            {
                ModernButton button = _actionButtons[i];
                button.Padding = new Padding(s(10), 0, s(10), 0);
                Size textSize = Fluent.MeasureText(button.Text, button.Font, Size.Empty, TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
                int buttonWidth = Math.Max(s(80), textSize.Width + s(20));
                x -= buttonWidth;
                button.SetBounds(x, y, buttonWidth, buttonHeight);
                button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                x -= buttonGap;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = _closeResult;
                Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.None)
                DialogResult = _closeResult;
            base.OnFormClosing(e);
        }
    }

    internal sealed class MessageIconControl : Control
    {
        readonly MessageBoxIcon _icon;
        readonly string _symbol;

        internal MessageIconControl(MessageBoxIcon icon)
        {
            _icon = icon;
            _symbol = GetSymbol(icon);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        static string GetSymbol(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Error) return "×";
            if (icon == MessageBoxIcon.Warning) return "!";
            if (icon == MessageBoxIcon.Question) return "?";
            if (icon == MessageBoxIcon.Information) return "i";
            return string.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Color color = Fluent.AccentText;
            if (_icon == MessageBoxIcon.Error || _icon == MessageBoxIcon.Hand || _icon == MessageBoxIcon.Stop)
                color = Fluent.Danger;
            else if (_icon == MessageBoxIcon.Warning || _icon == MessageBoxIcon.Exclamation)
                color = Fluent.Warning;

            int size = Math.Min(Width, Height) - Fluent.Scale(this, 4);
            Rectangle bounds = new Rectangle((Width - size) / 2, (Height - size) / 2, size, size);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush fill = new SolidBrush(Color.FromArgb(28, color)))
                e.Graphics.FillEllipse(fill, bounds);
            using (Pen border = new Pen(Color.FromArgb(110, color), Fluent.Scale(this, 1)))
                e.Graphics.DrawEllipse(border, bounds);

            Font font = Fluent.UiSemibold(20f);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
            Fluent.DrawText(e.Graphics, _symbol, font, ClientRectangle, color, flags);
        }
    }
}
