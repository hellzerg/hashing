using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Hashing
{
    internal static class ToolText
    {
        internal static string Get(string key, string fallback)
        {
            if (Options.TranslationList == null || Options.TranslationList[key] == null)
                return fallback;
            return Convert.ToString(Options.TranslationList[key]);
        }
    }

    internal static class ToolFormChrome
    {
        internal static Panel CreateHeader(string title, string subtitle)
        {
            TableLayoutPanel header = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 76,
                Padding = new Padding(20, 12, 20, 8),
                BackColor = Fluent.Background,
                ColumnCount = 1,
                RowCount = 2
            };
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            header.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            header.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            Label heading = new Label
            {
                Dock = DockStyle.Fill,
                Text = title,
                Font = Fluent.UiSemibold(12f),
                ForeColor = Fluent.Text,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = Padding.Empty,
                AutoEllipsis = true
            };
            Label description = new Label
            {
                Dock = DockStyle.Fill,
                Text = subtitle ?? string.Empty,
                Font = Fluent.Ui(9f),
                ForeColor = Fluent.TextSecondary,
                Tag = "secondary",
                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true,
                Margin = Padding.Empty
            };

            header.Controls.Add(heading, 0, 0);
            header.Controls.Add(description, 0, 1);
            return header;
        }

        internal static Panel CreateBodyHost()
        {
            return new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 4, 18, 14),
                BackColor = Fluent.Background
            };
        }

        internal static Card CreateCard()
        {
            return new Card
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                Radius = 9,
                BackColor = Fluent.Surface,
                BorderColor = Fluent.Border
            };
        }

        internal static FooterPanel CreateFooter(params ModernButton[] buttons)
        {
            FooterPanel footer = new FooterPanel
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                Padding = new Padding(18, 10, 18, 10),
                BackColor = Fluent.Surface
            };

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Fluent.Surface,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            foreach (ModernButton button in buttons ?? new ModernButton[0])
            {
                if (button == null)
                    continue;

                button.Anchor = AnchorStyles.None;
                button.Margin = new Padding(8, 0, 0, 0);
                actions.Controls.Add(button);
            }

            footer.Controls.Add(actions);
            return footer;
        }

        internal static ModernButton CreateButton(string text, string glyph, ButtonVariant variant, DialogResult result, int width)
        {
            return new ModernButton
            {
                Text = text,
                Glyph = glyph,
                Variant = variant,
                DialogResult = result,
                Size = new Size(width, 38),
                Margin = new Padding(8, 0, 0, 0),
                Font = Fluent.UiSemibold(9f),
                Anchor = AnchorStyles.None,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        internal static InputBox CreateTextInput(FluentTextBox editor, bool multiline = false)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor));

            editor.BorderStyle = BorderStyle.None;
            editor.BackColor = Fluent.Control;
            editor.ForeColor = Fluent.Text;
            editor.Font = editor.Font ?? Fluent.Ui(9.5f);
            editor.Multiline = multiline || editor.Multiline;

            InputBox input = new InputBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5),
                FillColor = Fluent.Control,
                Radius = 6
            };
            input.SetEditor(editor);
            return input;
        }

        internal static FluentGrid CreateGrid()
        {
            return new FluentGrid
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 36 },
                BackgroundColor = Fluent.Surface,
                DefaultCellStyle =
                {
                    Font = Fluent.Ui(9f),
                    Padding = new Padding(10, 4, 10, 4),
                    WrapMode = DataGridViewTriState.False,
                    SelectionBackColor = Fluent.SelectionColor,
                    SelectionForeColor = Fluent.Text
                },
                ColumnHeadersDefaultCellStyle =
                {
                    Font = Fluent.UiSemibold(9f),
                    Padding = new Padding(10, 5, 10, 5),
                    WrapMode = DataGridViewTriState.False,
                    BackColor = Fluent.Surface,
                    ForeColor = Fluent.TextSecondary,
                    SelectionBackColor = Fluent.Surface,
                    SelectionForeColor = Fluent.TextSecondary
                }
            };
        }

        internal static void Configure(Form form, Size size, Size minimumSize)
        {
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = size;
            form.MinimumSize = minimumSize;
            form.ShowInTaskbar = false;
            form.ShowIcon = false;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.Font = Fluent.Ui(9.5f);
            form.BackColor = Fluent.Background;
            form.ForeColor = Fluent.Text;
        }
    }

    internal sealed class OperationProgressForm : Form, IProgress<OperationProgress>
    {
        private readonly Label _message;
        private readonly ProgressBar _progress;
        private readonly ModernButton _cancel;
        private readonly System.Threading.CancellationTokenSource _cancellation = new System.Threading.CancellationTokenSource();
        private bool _completed;

        internal OperationProgressForm(string title)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            ToolFormChrome.Configure(this, new Size(560, 290), new Size(560, 290));

            _message = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                Text = ToolText.Get("operationInProgress", "Working..."),
                Tag = "secondary",
                TextAlign = ContentAlignment.MiddleLeft
            };
            _progress = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 16,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 24,
                Margin = new Padding(0, 8, 0, 0)
            };

            TableLayoutPanel progressLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 78,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Fluent.Surface,
                Margin = Padding.Empty
            };
            progressLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            progressLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22));
            progressLayout.Controls.Add(_message, 0, 0);
            progressLayout.Controls.Add(_progress, 0, 1);
            Card body = ToolFormChrome.CreateCard();
            body.Controls.Add(progressLayout);
            _cancel = ToolFormChrome.CreateButton(
                ToolText.Get("btnCancelHashing", "Cancel"), Fluent.Glyphs.Cancel,
                ButtonVariant.Secondary, DialogResult.None, 112);
            _cancel.Click += Cancel_Click;

            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(_cancel));
            Controls.Add(ToolFormChrome.CreateHeader(title, string.Empty));
            CancelButton = _cancel;
            Options.ApplyTheme(this);
        }

        internal System.Threading.CancellationToken Token { get { return _cancellation.Token; } }

        public void Report(OperationProgress value)
        {
            if (IsDisposed || value == null)
                return;

            if (InvokeRequired)
            {
                try { BeginInvoke((MethodInvoker)delegate { Report(value); }); }
                catch (InvalidOperationException) { }
                return;
            }

            _message.Text = string.IsNullOrWhiteSpace(value.Message)
                ? ToolText.Get("operationInProgress", "Working...")
                : value.Message;

            if (value.Total > 0)
            {
                _progress.Style = ProgressBarStyle.Continuous;
                _progress.Minimum = 0;
                _progress.Maximum = Math.Max(1, value.Total);
                _progress.Value = Math.Min(_progress.Maximum, Math.Max(0, value.Current));
            }
            else
            {
                _progress.Style = ProgressBarStyle.Marquee;
            }
        }

        internal void Complete()
        {
            if (IsDisposed)
                return;

            _completed = true;
            DialogResult = _cancellation.IsCancellationRequested ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            if (_completed || _cancellation.IsCancellationRequested)
                return;

            _cancellation.Cancel();
            _cancel.Enabled = false;
            _message.Text = ToolText.Get("operationCancelling", "Cancelling...");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_completed && !_cancellation.IsCancellationRequested)
            {
                e.Cancel = true;
                _cancellation.Cancel();
                _cancel.Enabled = false;
                _message.Text = ToolText.Get("operationCancelling", "Cancelling...");
            }
            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _cancellation.Dispose();
            base.Dispose(disposing);
        }
    }

    internal sealed class ManifestVerificationResultsForm : Form
    {
        private readonly FluentGrid _grid;
        private readonly Label _summary;
        private readonly FluentTextBox _warnings;
        private readonly List<ChecksumVerificationResult> _results;

        internal ManifestVerificationResultsForm(IList<ChecksumVerificationResult> results, IList<string> warnings)
        {
            _results = results == null ? new List<ChecksumVerificationResult>() : results.ToList();
            Text = ToolText.Get("manifestResultsTitle", "Checksum verification results");
            ToolFormChrome.Configure(this, new Size(1120, 680), new Size(850, 500));

            _summary = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Padding = new Padding(4, 0, 4, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = Fluent.UiSemibold(10f)
            };

            _grid = ToolFormChrome.CreateGrid();
            AddColumn("status", ToolText.Get("manifestStatus", "Status"), 110);
            AddColumn("algorithm", ToolText.Get("manifestAlgorithm", "Algorithm"), 120);
            AddColumn("file", ToolText.Get("manifestFile", "File"), 320, true);
            AddColumn("expected", ToolText.Get("manifestExpected", "Expected"), 240);
            AddColumn("actual", ToolText.Get("manifestActual", "Actual"), 240, true);

            _warnings = new FluentTextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = Fluent.Ui(8.5f),
                ForeColor = Fluent.TextSecondary,
                Text = warnings == null ? string.Empty : string.Join(Environment.NewLine, warnings)
            };
            InputBox warningsInput = ToolFormChrome.CreateTextInput(_warnings, true);
            warningsInput.Margin = new Padding(4);

            TableLayoutPanel content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Fluent.Surface,
                Padding = Padding.Empty
            };
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.RowStyles.Add(new RowStyle(SizeType.Absolute, warnings != null && warnings.Count > 0 ? 82 : 0));
            _summary.Dock = DockStyle.Fill;
            _summary.Padding = new Padding(8, 0, 8, 0);
            _grid.Dock = DockStyle.Fill;
            warningsInput.Dock = DockStyle.Fill;
            content.Controls.Add(_summary, 0, 0);
            content.Controls.Add(_grid, 0, 1);
            content.Controls.Add(warningsInput, 0, 2);

            Card card = ToolFormChrome.CreateCard();
            card.Controls.Add(content);
            Panel body = ToolFormChrome.CreateBodyHost();
            body.Controls.Add(card);

            ModernButton close = ToolFormChrome.CreateButton(
                ToolText.Get("button7", "OK"), Fluent.Glyphs.Checkmark,
                ButtonVariant.Primary, DialogResult.OK, 112);
            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(close));
            Controls.Add(ToolFormChrome.CreateHeader(Text,
                ToolText.Get("manifestResultsSubtitle", "Review which files match the expected checksums.")));
            AcceptButton = close;

            foreach (ChecksumVerificationResult result in _results)
            {
                int row = _grid.Rows.Add(
                    GetStatusText(result.Status),
                    result.Algorithm,
                    result.FilePath,
                    result.ExpectedHash,
                    result.ActualHash ?? result.Error);
                DataGridViewRow gridRow = _grid.Rows[row];
                gridRow.Tag = result;
                gridRow.Cells[0].Style.ForeColor = GetStatusColor(result.Status);
                gridRow.Cells[0].Style.Font = Fluent.UiSemibold(9f);
            }

            int passed = _results.Count(item => item.Status == ChecksumVerificationStatus.Passed);
            int failed = _results.Count(item => item.Status == ChecksumVerificationStatus.Failed);
            int missing = _results.Count(item => item.Status == ChecksumVerificationStatus.Missing);
            int errors = _results.Count(item => item.Status == ChecksumVerificationStatus.Error);
            _summary.Text = string.Format(
                "{0}: {1}    {2}: {3}    {4}: {5}    {6}: {7}",
                ToolText.Get("manifestPassed", "Passed"), passed,
                ToolText.Get("manifestFailed", "Failed"), failed,
                ToolText.Get("manifestMissing", "Missing"), missing,
                ToolText.Get("manifestErrors", "Errors"), errors);

            Options.ApplyTheme(this);
            if (_grid.Rows.Count > 0)
                _grid.Rows[0].Selected = true;
        }

        private void AddColumn(string name, string header, int width, bool fill = false)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Width = width,
                AutoSizeMode = fill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }

        private string GetStatusText(ChecksumVerificationStatus status)
        {
            switch (status)
            {
                case ChecksumVerificationStatus.Passed: return ToolText.Get("manifestPassed", "Passed");
                case ChecksumVerificationStatus.Failed: return ToolText.Get("manifestFailed", "Failed");
                case ChecksumVerificationStatus.Missing: return ToolText.Get("manifestMissing", "Missing");
                default: return ToolText.Get("manifestError", "Error");
            }
        }

        private Color GetStatusColor(ChecksumVerificationStatus status)
        {
            switch (status)
            {
                case ChecksumVerificationStatus.Passed: return Fluent.Success;
                case ChecksumVerificationStatus.Failed: return Fluent.Danger;
                case ChecksumVerificationStatus.Missing: return Fluent.Warning;
                default: return Fluent.TextSecondary;
            }
        }
    }

    internal sealed class AuthenticodeResultsForm : Form
    {
        private readonly FluentGrid _grid;
        private readonly FluentTextBox _details;

        internal AuthenticodeResultsForm(IList<AuthenticodeInspectionResult> results)
        {
            Text = ToolText.Get("authenticodeResultsTitle", "Authenticode inspection");
            ToolFormChrome.Configure(this, new Size(1080, 680), new Size(820, 500));

            _grid = ToolFormChrome.CreateGrid();
            AddColumn("status", ToolText.Get("authenticodeStatus", "Status"), 210);
            AddColumn("file", ToolText.Get("manifestFile", "File"), 420, true);
            AddColumn("publisher", ToolText.Get("authenticodePublisher", "Signer"), 280);

            _details = new FluentTextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = Fluent.Mono(9f),
                ForeColor = Fluent.Text
            };
            InputBox detailsInput = ToolFormChrome.CreateTextInput(_details, true);
            detailsInput.Margin = new Padding(8);

            TableLayoutPanel content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Fluent.Surface,
                Padding = new Padding(2)
            };
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
            content.Controls.Add(_grid, 0, 0);
            content.Controls.Add(detailsInput, 0, 1);
            _grid.SelectionChanged += Grid_SelectionChanged;

            Card card = ToolFormChrome.CreateCard();
            card.Controls.Add(content);
            Panel body = ToolFormChrome.CreateBodyHost();
            body.Controls.Add(card);

            ModernButton close = ToolFormChrome.CreateButton(
                ToolText.Get("button7", "OK"), Fluent.Glyphs.Checkmark,
                ButtonVariant.Primary, DialogResult.OK, 112);
            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(close));
            Controls.Add(ToolFormChrome.CreateHeader(Text,
                ToolText.Get("authenticodeResultsSubtitle", "Review signature trust and verified signer details.")));
            AcceptButton = close;

            foreach (AuthenticodeInspectionResult result in results ?? new List<AuthenticodeInspectionResult>())
            {
                int row = _grid.Rows.Add(result.GetStatusText(), result.FilePath, result.Publisher);
                _grid.Rows[row].Tag = result;
                _grid.Rows[row].Cells[0].Style.ForeColor = result.Trusted ? Fluent.Success : Fluent.Danger;
                _grid.Rows[row].Cells[0].Style.Font = Fluent.UiSemibold(9f);
            }

            Options.ApplyTheme(this);
            if (_grid.Rows.Count > 0)
                _grid.Rows[0].Selected = true;
        }

        private void AddColumn(string name, string header, int width, bool fill = false)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                Width = width,
                AutoSizeMode = fill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0)
                return;
            AuthenticodeInspectionResult result = _grid.SelectedRows[0].Tag as AuthenticodeInspectionResult;
            if (result != null)
                _details.Text = result.GetDetails();
        }
    }

    internal sealed class CertificateInspectionResultsForm : Form
    {
        private readonly ListBox _certificates;
        private readonly FluentTextBox _details;
        private readonly IList<CertificateInspectionResult> _results;

        internal CertificateInspectionResultsForm(IList<CertificateInspectionResult> results)
        {
            _results = results ?? new List<CertificateInspectionResult>();
            Text = ToolText.Get("certificateInspectorTitle", "Certificate inspector");
            ToolFormChrome.Configure(this, new Size(1020, 660), new Size(800, 500));

            _certificates = new ListBox
            {
                Dock = DockStyle.Fill,
                IntegralHeight = false,
                BorderStyle = BorderStyle.None,
                BackColor = Fluent.Control,
                ForeColor = Fluent.Text,
                Font = Fluent.Ui(9f)
            };
            _details = new FluentTextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = Fluent.Mono(9f),
                ForeColor = Fluent.Text
            };
            InputBox detailsInput = ToolFormChrome.CreateTextInput(_details, true);
            detailsInput.Margin = new Padding(8);
            _certificates.SelectedIndexChanged += Certificates_SelectedIndexChanged;

            TableLayoutPanel content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Fluent.Surface,
                Padding = new Padding(2)
            };
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
            content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
            content.Controls.Add(_certificates, 0, 0);
            content.Controls.Add(detailsInput, 1, 0);

            Card card = ToolFormChrome.CreateCard();
            card.Controls.Add(content);
            Panel body = ToolFormChrome.CreateBodyHost();
            body.Controls.Add(card);

            ModernButton close = ToolFormChrome.CreateButton(
                ToolText.Get("button7", "OK"), Fluent.Glyphs.Checkmark,
                ButtonVariant.Primary, DialogResult.OK, 112);
            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(close));
            Controls.Add(ToolFormChrome.CreateHeader(Text,
                ToolText.Get("certificateInspectorSubtitle", "Inspect certificate identity, validity, usage and key details.")));
            AcceptButton = close;

            foreach (CertificateInspectionResult result in _results)
            {
                string label = string.IsNullOrWhiteSpace(result.Error)
                    ? result.Subject
                    : Path.GetFileName(result.SourcePath) + " — " + ToolText.Get("certificateLoadError", "Load error");
                _certificates.Items.Add(label ?? result.SourcePath);
            }

            Options.ApplyTheme(this);
            if (_certificates.Items.Count > 0)
                _certificates.SelectedIndex = 0;
        }

        private void Certificates_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = _certificates.SelectedIndex;
            if (index >= 0 && index < _results.Count)
                _details.Text = _results[index].GetDetails();
        }
    }

    internal sealed class PasswordPromptForm : Form
    {
        private readonly FluentTextBox _password;
        internal string Password { get { return _password.Text; } }

        internal PasswordPromptForm()
        {
            Text = ToolText.Get("certificatePasswordTitle", "Certificate password");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            ToolFormChrome.Configure(this, new Size(560, 330), new Size(560, 330));

            Label label = new Label
            {
                Dock = DockStyle.Top,
                Height = 32,
                Text = ToolText.Get("certificatePasswordPrompt", "Enter the PFX/P12 password:"),
                Tag = "secondary",
                TextAlign = ContentAlignment.MiddleLeft
            };
            _password = new FluentTextBox
            {
                UseSystemPasswordChar = true,
                Font = Fluent.Ui(10f)
            };
            InputBox passwordInput = ToolFormChrome.CreateTextInput(_password);
            passwordInput.Dock = DockStyle.Fill;
            passwordInput.Margin = new Padding(0, 4, 0, 4);

            TableLayoutPanel fields = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 78,
                ColumnCount = 1,
                RowCount = 2,
                Padding = Padding.Empty,
                BackColor = Fluent.Surface
            };
            fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            label.Margin = new Padding(0);
            fields.Controls.Add(label, 0, 0);
            fields.Controls.Add(passwordInput, 0, 1);
            Card card = ToolFormChrome.CreateCard();
            card.Controls.Add(fields);
            Panel body = ToolFormChrome.CreateBodyHost();
            body.Controls.Add(card);

            ModernButton ok = ToolFormChrome.CreateButton(
                ToolText.Get("button7", "OK"), Fluent.Glyphs.Checkmark,
                ButtonVariant.Primary, DialogResult.OK, 104);
            ModernButton cancel = ToolFormChrome.CreateButton(
                ToolText.Get("btnCancelHashing", "Cancel"), Fluent.Glyphs.Cancel,
                ButtonVariant.Secondary, DialogResult.Cancel, 112);
            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(ok, cancel));
            Controls.Add(ToolFormChrome.CreateHeader(Text,
                ToolText.Get("certificatePasswordSubtitle", "The private key remains in memory only while the certificate is inspected.")));
            AcceptButton = ok;
            CancelButton = cancel;
            Options.ApplyTheme(this);
        }
    }

    internal sealed class CertificateGeneratorForm : Form
    {
        private readonly FluentTextBox _commonName;
        private readonly FluentTextBox _alternativeNames;
        private readonly MoonSelect _purpose;
        private readonly MoonSelect _keySize;
        private readonly FluentTextBox _validity;
        private readonly FluentTextBox _password;
        private readonly FluentTextBox _confirmPassword;
        private readonly Label _status;

        internal CertificateGeneratorForm()
        {
            Text = ToolText.Get("certificateGeneratorTitle", "Certificate generator");
            ToolFormChrome.Configure(this, new Size(760, 760), new Size(650, 650));

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(12, 10, 12, 10),
                AutoScroll = true,
                BackColor = Fluent.Surface
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 92));
            for (int row = 2; row < 7; row++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));

            _commonName = new FluentTextBox { Font = Fluent.Ui(9.5f) };
            _alternativeNames = new FluentTextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, Font = Fluent.Ui(9.5f) };
            InputBox commonNameInput = ToolFormChrome.CreateTextInput(_commonName);
            InputBox alternativeNamesInput = ToolFormChrome.CreateTextInput(_alternativeNames, true);
            _purpose = new MoonSelect { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5) };
            _purpose.Items.AddRange(new object[]
            {
                ToolText.Get("certificatePurposeServer", "TLS server authentication"),
                ToolText.Get("certificatePurposeClient", "TLS client authentication"),
                ToolText.Get("certificatePurposeBoth", "TLS server + client authentication"),
                ToolText.Get("certificatePurposeCodeSigning", "Code signing"),
                ToolText.Get("certificatePurposeGeneral", "General purpose")
            });
            _purpose.SelectedIndex = 0;

            _keySize = new MoonSelect { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 0, 5) };
            _keySize.Items.AddRange(new object[] { "2048", "3072", "4096" });
            _keySize.SelectedIndex = 1;
            _validity = new FluentTextBox
            {
                MaxLength = 4,
                Text = "365",
                Font = Fluent.Ui(9.5f)
            };
            InputBox validityInput = ToolFormChrome.CreateTextInput(_validity);
            _validity.KeyPress += Validity_KeyPress;
            _password = new FluentTextBox { UseSystemPasswordChar = true, Font = Fluent.Ui(9.5f) };
            _confirmPassword = new FluentTextBox { UseSystemPasswordChar = true, Font = Fluent.Ui(9.5f) };
            InputBox passwordInput = ToolFormChrome.CreateTextInput(_password);
            InputBox confirmPasswordInput = ToolFormChrome.CreateTextInput(_confirmPassword);
            _status = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = "secondary",
                AutoEllipsis = true,
                Padding = new Padding(0, 2, 0, 2),
                Text = ToolText.Get("certificateSelfSignedNote", "Self-signed, non-CA certificate. It is not trusted automatically.")
            };

            AddRow(layout, 0, ToolText.Get("certificateCommonName", "Common name (CN)"), commonNameInput);
            AddRow(layout, 1, ToolText.Get("certificateSan", "DNS / IP SANs (one per line)"), alternativeNamesInput);
            AddRow(layout, 2, ToolText.Get("certificatePurpose", "Purpose"), _purpose);
            AddRow(layout, 3, ToolText.Get("certificateKeySize", "RSA key size"), _keySize);
            AddRow(layout, 4, ToolText.Get("certificateValidity", "Validity (days)"), validityInput);
            AddRow(layout, 5, ToolText.Get("certificatePfxPassword", "PFX password (12+ chars)"), passwordInput);
            AddRow(layout, 6, ToolText.Get("certificateConfirmPassword", "Confirm password"), confirmPasswordInput);
            AddRow(layout, 7, ToolText.Get("certificateNote", "Certificate type"), _status);

            Card card = ToolFormChrome.CreateCard();
            card.Controls.Add(layout);
            Panel body = ToolFormChrome.CreateBodyHost();
            body.Controls.Add(card);

            ModernButton generate = ToolFormChrome.CreateButton(
                ToolText.Get("certificateGenerateExport", "Generate and export PFX + CER"), Fluent.Glyphs.Key,
                ButtonVariant.Primary, DialogResult.None, 300);
            ModernButton cancel = ToolFormChrome.CreateButton(
                ToolText.Get("btnCancelHashing", "Cancel"), Fluent.Glyphs.Cancel,
                ButtonVariant.Secondary, DialogResult.Cancel, 112);
            generate.Click += Generate_Click;

            Controls.Add(body);
            Controls.Add(ToolFormChrome.CreateFooter(generate, cancel));
            Controls.Add(ToolFormChrome.CreateHeader(Text,
                ToolText.Get("certificateGeneratorSubtitle", "Create a self-signed certificate for testing and private environments.")));
            CancelButton = cancel;
            Options.ApplyTheme(this);
        }

        private static void AddRow(TableLayoutPanel layout, int row, string caption, Control input)
        {
            Label label = new Label
            {
                Text = caption,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Tag = "secondary",
                Padding = new Padding(0, 0, 12, 0)
            };
            input.Margin = new Padding(0, 5, 0, 5);
            layout.Controls.Add(label, 0, row);
            layout.Controls.Add(input, 1, row);
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            string password = _password.Text;
            if (password.Length < 12)
            {
                FluentMessageBox.Show(this, ToolText.Get("certificatePasswordLength", "Use a PFX password containing at least 12 characters."), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _password.Focus();
                return;
            }
            if (!string.Equals(password, _confirmPassword.Text, StringComparison.Ordinal))
            {
                FluentMessageBox.Show(this, ToolText.Get("certificatePasswordMismatch", "The passwords do not match."), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _confirmPassword.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(_commonName.Text))
            {
                FluentMessageBox.Show(this, ToolText.Get("certificateCommonNameRequired", "Enter a common name."), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _commonName.Focus();
                return;
            }

            int validityDays;
            if (!int.TryParse(_validity.Text, System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture, out validityDays) ||
                validityDays < 1 || validityDays > 3650)
            {
                FluentMessageBox.Show(this,
                    ToolText.Get("certificateValidityRange", "Enter a validity period between 1 and 3650 days."),
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _validity.Focus();
                _validity.SelectAll();
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = ToolText.Get("certificateSaveTitle", "Save private-key certificate");
                dialog.Filter = "PKCS #12 certificate (*.pfx)|*.pfx";
                dialog.DefaultExt = "pfx";
                dialog.AddExtension = true;
                dialog.FileName = "certificate.pfx";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                string cerPath = System.IO.Path.ChangeExtension(dialog.FileName, ".cer");
                if ((System.IO.File.Exists(dialog.FileName) || System.IO.File.Exists(cerPath)) &&
                    FluentMessageBox.Show(this,
                        ToolText.Get("certificateOverwrite", "The PFX or CER file already exists. Replace both files?"),
                        Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                CertificatePurpose purpose = GetPurpose(_purpose.SelectedIndex);
                CertificateGenerationOptions options = new CertificateGenerationOptions
                {
                    CommonName = _commonName.Text.Trim(),
                    SubjectAlternativeNames = _alternativeNames.Text.Split(new[] { '\r', '\n', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(value => value.Trim()).Where(value => value.Length > 0).ToArray(),
                    Purpose = purpose,
                    KeySize = int.Parse(Convert.ToString(_keySize.SelectedItem), System.Globalization.CultureInfo.InvariantCulture),
                    ValidityDays = validityDays
                };

                try
                {
                    using (X509Certificate2 certificate = CertificateService.CreateSelfSigned(options))
                    {
                        CertificateService.ExportPfxAndCer(certificate, dialog.FileName, password);
                        FluentMessageBox.Show(this,
                            ToolText.Get("certificateGenerated", "Certificate generated successfully.") + Environment.NewLine +
                            "PFX: " + dialog.FileName + Environment.NewLine +
                            "CER: " + cerPath + Environment.NewLine +
                            "SHA-1 thumbprint: " + certificate.Thumbprint + Environment.NewLine + Environment.NewLine +
                            ToolText.Get("certificateSelfSignedWarning", "This is self-signed and will not be trusted by browsers or Windows until explicitly trusted."),
                            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    _password.Clear();
                    _confirmPassword.Clear();
                }
                catch (Exception ex)
                {
                    FluentMessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void Validity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '0' || e.KeyChar > '9'))
                e.Handled = true;
        }

        private static CertificatePurpose GetPurpose(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: return CertificatePurpose.TlsServer;
                case 1: return CertificatePurpose.TlsClient;
                case 2: return CertificatePurpose.TlsServerAndClient;
                case 3: return CertificatePurpose.CodeSigning;
                default: return CertificatePurpose.GeneralPurpose;
            }
        }
    }
}
