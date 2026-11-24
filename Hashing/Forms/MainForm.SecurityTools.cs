using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class MainForm
    {
        private ModernButton _btnTools;
        private ContextMenuStrip _toolsMenu;
        private ToolStripMenuItem _createManifestMenuItem;
        private ToolStripMenuItem _verifyManifestMenuItem;
        private ToolStripMenuItem _authenticodeMenuItem;
        private ToolStripMenuItem _certificateInspectorMenuItem;
        private ToolStripMenuItem _certificateGeneratorMenuItem;
        private ContextMenuStrip _exportMenu;
        private ToolStripMenuItem _exportJsonMenuItem;
        private ToolStripMenuItem _exportCsvMenuItem;
        private ToolStripMenuItem _exportTxtMenuItem;
        private ToolStripMenuItem _exportMd5MenuItem;
        private ToolStripMenuItem _exportSha1MenuItem;
        private ToolStripMenuItem _exportSha256MenuItem;

        private sealed class ToolOperationOutcome<T>
        {
            internal T Value;
            internal Exception Error;
            internal bool Cancelled;
        }

        private void InitializeSecurityToolsMenu()
        {
            _toolsMenu = new ContextMenuStrip();
            _createManifestMenuItem = new ToolStripMenuItem(ToolText.Get("toolCreateManifest", "Create checksum manifest..."));
            _verifyManifestMenuItem = new ToolStripMenuItem(ToolText.Get("toolVerifyManifest", "Verify checksum manifest..."));
            _authenticodeMenuItem = new ToolStripMenuItem(ToolText.Get("toolAuthenticode", "Inspect Authenticode signatures..."));
            _certificateInspectorMenuItem = new ToolStripMenuItem(ToolText.Get("toolCertificateInspector", "Certificate inspector..."));
            _certificateGeneratorMenuItem = new ToolStripMenuItem(ToolText.Get("toolCertificateGenerator", "Certificate generator..."));

            _createManifestMenuItem.Click += CreateChecksumManifest_Click;
            _verifyManifestMenuItem.Click += VerifyChecksumManifest_Click;
            _authenticodeMenuItem.Click += InspectAuthenticode_Click;
            _certificateInspectorMenuItem.Click += InspectCertificates_Click;
            _certificateGeneratorMenuItem.Click += GenerateCertificate_Click;

            _toolsMenu.Items.AddRange(new ToolStripItem[]
            {
                _createManifestMenuItem,
                _verifyManifestMenuItem,
                new ToolStripSeparator(),
                _authenticodeMenuItem,
                new ToolStripSeparator(),
                _certificateInspectorMenuItem,
                _certificateGeneratorMenuItem
            });
            Fluent.SetMenuGlyph(_createManifestMenuItem, Fluent.Glyphs.Download, _toolsMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_verifyManifestMenuItem, Fluent.Glyphs.Checkmark, _toolsMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_authenticodeMenuItem, Fluent.Glyphs.Shield, _toolsMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_certificateInspectorMenuItem, Fluent.Glyphs.Search, _toolsMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_certificateGeneratorMenuItem, Fluent.Glyphs.Key, _toolsMenu.DeviceDpi);
            Fluent.StyleMenu(_toolsMenu);

            _exportMenu = new ContextMenuStrip();
            _exportJsonMenuItem = new ToolStripMenuItem(ToolText.Get("exportJson", "JSON"));
            _exportCsvMenuItem = new ToolStripMenuItem(ToolText.Get("exportCsv", "CSV"));
            _exportTxtMenuItem = new ToolStripMenuItem(ToolText.Get("exportTxt", "Text file"));
            _exportMd5MenuItem = new ToolStripMenuItem(ToolText.Get("exportMd5", "MD5 checksum (.md5)"));
            _exportSha1MenuItem = new ToolStripMenuItem(ToolText.Get("exportSha1", "SHA-1 checksum (.sha1)"));
            _exportSha256MenuItem = new ToolStripMenuItem(ToolText.Get("exportSha256", "SHA-256 checksum (.sha256)"));
            _exportJsonMenuItem.Click += delegate { ExportHashResults(HashExportFormat.Json); };
            _exportCsvMenuItem.Click += delegate { ExportHashResults(HashExportFormat.Csv); };
            _exportTxtMenuItem.Click += delegate { ExportHashResults(HashExportFormat.Text); };
            _exportMd5MenuItem.Click += delegate { ExportHashResults(HashExportFormat.Md5); };
            _exportSha1MenuItem.Click += delegate { ExportHashResults(HashExportFormat.Sha1); };
            _exportSha256MenuItem.Click += delegate { ExportHashResults(HashExportFormat.Sha256); };
            _exportMenu.Items.AddRange(new ToolStripItem[]
            {
                _exportJsonMenuItem,
                _exportCsvMenuItem,
                _exportTxtMenuItem,
                new ToolStripSeparator(),
                _exportMd5MenuItem,
                _exportSha1MenuItem,
                _exportSha256MenuItem
            });
            Fluent.SetMenuGlyph(_exportJsonMenuItem, Fluent.Glyphs.Tag, _exportMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_exportCsvMenuItem, Fluent.Glyphs.Copy, _exportMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_exportTxtMenuItem, Fluent.Glyphs.Info, _exportMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_exportMd5MenuItem, Fluent.Glyphs.Checkmark, _exportMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_exportSha1MenuItem, Fluent.Glyphs.Shield, _exportMenu.DeviceDpi);
            Fluent.SetMenuGlyph(_exportSha256MenuItem, Fluent.Glyphs.Key, _exportMenu.DeviceDpi);
            Fluent.StyleMenu(_exportMenu);

            FormClosed += delegate
            {
                if (_toolsMenu != null)
                {
                    _toolsMenu.Dispose();
                    _toolsMenu = null;
                }
                if (_exportMenu != null)
                {
                    _exportMenu.Dispose();
                    _exportMenu = null;
                }
            };
        }

        private void btnTools_Click(object sender, EventArgs e)
        {
            if (_toolsMenu == null || _btnTools == null)
                return;

            _createManifestMenuItem.Enabled = SumResult.Sums != null && SumResult.Sums.Count > 0;
            _toolsMenu.Show(_btnTools, new Point(0, _btnTools.Height));
        }

        private void TranslateSecurityTools()
        {
            if (_btnTools != null)
                _btnTools.Text = ToolText.Get("btnTools", "Tools");
            if (btnSaveJson != null)
                btnSaveJson.Text = ToolText.Get("btnSaveJson", "Export");
            if (_exportJsonMenuItem != null)
            {
                _exportJsonMenuItem.Text = ToolText.Get("exportJson", "JSON");
                _exportCsvMenuItem.Text = ToolText.Get("exportCsv", "CSV");
                _exportTxtMenuItem.Text = ToolText.Get("exportTxt", "Text file");
                _exportMd5MenuItem.Text = ToolText.Get("exportMd5", "MD5 checksum (.md5)");
                _exportSha1MenuItem.Text = ToolText.Get("exportSha1", "SHA-1 checksum (.sha1)");
                _exportSha256MenuItem.Text = ToolText.Get("exportSha256", "SHA-256 checksum (.sha256)");
                Fluent.StyleMenu(_exportMenu);
            }
            if (_createManifestMenuItem == null)
                return;

            _createManifestMenuItem.Text = ToolText.Get("toolCreateManifest", "Create checksum manifest...");
            _verifyManifestMenuItem.Text = ToolText.Get("toolVerifyManifest", "Verify checksum manifest...");
            _authenticodeMenuItem.Text = ToolText.Get("toolAuthenticode", "Inspect Authenticode signatures...");
            _certificateInspectorMenuItem.Text = ToolText.Get("toolCertificateInspector", "Certificate inspector...");
            _certificateGeneratorMenuItem.Text = ToolText.Get("toolCertificateGenerator", "Certificate generator...");
            Fluent.StyleMenu(_toolsMenu);
        }

        private async void CreateChecksumManifest_Click(object sender, EventArgs e)
        {
            string[] files = SumResult.Sums
                .Where(item => item != null && !string.IsNullOrWhiteSpace(item.File))
                .Select(item => item.File)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (files.Length == 0)
            {
                FluentMessageBox.Show(this, ToolText.Get("manifestNoResults", "Calculate files before creating a checksum manifest."), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = ToolText.Get("manifestCreateTitle", "Create checksum manifest");
                dialog.Filter =
                    "GNU MD5 (*.md5)|*.md5|BSD MD5 (*.md5.bsd)|*.md5.bsd|" +
                    "GNU SHA-1 (*.sha1)|*.sha1|BSD SHA-1 (*.sha1.bsd)|*.sha1.bsd|" +
                    "GNU SHA-256 (*.sha256)|*.sha256|BSD SHA-256 (*.sha256.bsd)|*.sha256.bsd|" +
                    "GNU SHA-384 (*.sha384)|*.sha384|BSD SHA-384 (*.sha384.bsd)|*.sha384.bsd|" +
                    "GNU SHA-512 (*.sha512)|*.sha512|BSD SHA-512 (*.sha512.bsd)|*.sha512.bsd|" +
                    "CRC32 SFV (*.sfv)|*.sfv|" +
                    "BSD RIPEMD-160 (*.ripemd160.bsd)|*.ripemd160.bsd|" +
                    "BSD SHA3-256 (*.sha3-256.bsd)|*.sha3-256.bsd|" +
                    "BSD SHA3-384 (*.sha3-384.bsd)|*.sha3-384.bsd|" +
                    "BSD SHA3-512 (*.sha3-512.bsd)|*.sha3-512.bsd";
                dialog.FilterIndex = 5;
                dialog.DefaultExt = "sha256";
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                dialog.FileName = "checksums";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                ChecksumAlgorithm algorithm;
                ChecksumManifestFormat format;
                if (!TryGetManifestExportSelection(dialog.FilterIndex, out algorithm, out format))
                    return;

                string outputPath = dialog.FileName;
                ToolOperationOutcome<bool> outcome = await RunToolOperationAsync(
                    ToolText.Get("manifestCreating", "Creating checksum manifest..."),
                    (token, progress) =>
                    {
                        ChecksumManifestService.Create(outputPath, algorithm, format, files, token, progress);
                        return true;
                    });

                if (outcome.Error != null)
                {
                    FluentMessageBox.Show(this, outcome.Error.Message, ToolText.Get("fileOperationErrorsTitle", "File operation errors"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (outcome.Cancelled || !outcome.Value)
                    return;

                FluentMessageBox.Show(this,
                    ToolText.Get("manifestCreated", "Checksum manifest created successfully.") + Environment.NewLine + outputPath,
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static bool TryGetManifestExportSelection(int filterIndex, out ChecksumAlgorithm algorithm, out ChecksumManifestFormat format)
        {
            format = ChecksumManifestFormat.Gnu;
            switch (filterIndex)
            {
                case 1: algorithm = ChecksumAlgorithm.MD5; return true;
                case 2: algorithm = ChecksumAlgorithm.MD5; format = ChecksumManifestFormat.Bsd; return true;
                case 3: algorithm = ChecksumAlgorithm.SHA1; return true;
                case 4: algorithm = ChecksumAlgorithm.SHA1; format = ChecksumManifestFormat.Bsd; return true;
                case 5: algorithm = ChecksumAlgorithm.SHA256; return true;
                case 6: algorithm = ChecksumAlgorithm.SHA256; format = ChecksumManifestFormat.Bsd; return true;
                case 7: algorithm = ChecksumAlgorithm.SHA384; return true;
                case 8: algorithm = ChecksumAlgorithm.SHA384; format = ChecksumManifestFormat.Bsd; return true;
                case 9: algorithm = ChecksumAlgorithm.SHA512; return true;
                case 10: algorithm = ChecksumAlgorithm.SHA512; format = ChecksumManifestFormat.Bsd; return true;
                case 11: algorithm = ChecksumAlgorithm.CRC32; format = ChecksumManifestFormat.Sfv; return true;
                case 12: algorithm = ChecksumAlgorithm.RIPEMD160; format = ChecksumManifestFormat.Bsd; return true;
                case 13: algorithm = ChecksumAlgorithm.SHA3_256; format = ChecksumManifestFormat.Bsd; return true;
                case 14: algorithm = ChecksumAlgorithm.SHA3_384; format = ChecksumManifestFormat.Bsd; return true;
                case 15: algorithm = ChecksumAlgorithm.SHA3_512; format = ChecksumManifestFormat.Bsd; return true;
                default: algorithm = default(ChecksumAlgorithm); return false;
            }
        }

        private async void VerifyChecksumManifest_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = ToolText.Get("manifestVerifyTitle", "Verify checksum manifest");
                dialog.Filter = "Checksum manifests (*.md5;*.sha1;*.sha256;*.sha384;*.sha512;*.sfv;*.bsd)|*.md5;*.sha1;*.sha256;*.sha384;*.sha512;*.sfv;*.bsd;*.md5.bsd;*.sha1.bsd;*.sha256.bsd;*.sha384.bsd;*.sha512.bsd;*.ripemd160.bsd;*.sha3-256.bsd;*.sha3-384.bsd;*.sha3-512.bsd|All files (*.*)|*.*";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                ChecksumManifestDocument document;
                try
                {
                    document = ChecksumManifestService.Read(dialog.FileName);
                }
                catch (Exception ex)
                {
                    FluentMessageBox.Show(this, ex.Message, ToolText.Get("manifestVerifyTitle", "Verify checksum manifest"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ToolOperationOutcome<List<ChecksumVerificationResult>> outcome = await RunToolOperationAsync(
                    ToolText.Get("manifestVerifying", "Verifying checksum manifest..."),
                    (token, progress) => ChecksumManifestService.Verify(document, token, progress));

                if (outcome.Error != null)
                {
                    FluentMessageBox.Show(this, outcome.Error.Message, ToolText.Get("manifestVerifyTitle", "Verify checksum manifest"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (outcome.Cancelled || outcome.Value == null)
                    return;

                using (ManifestVerificationResultsForm form = new ManifestVerificationResultsForm(outcome.Value, document.Warnings))
                    form.ShowDialog(this);
            }
        }

        private async void InspectAuthenticode_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = ToolText.Get("authenticodeTitle", "Authenticode signature inspector");
                dialog.Filter = "Signed files (*.exe;*.dll;*.sys;*.ocx;*.msi;*.cat)|*.exe;*.dll;*.sys;*.ocx;*.msi;*.cat|All files (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                string[] files = dialog.FileNames;
                ToolOperationOutcome<List<AuthenticodeInspectionResult>> outcome = await RunToolOperationAsync(
                    ToolText.Get("authenticodeVerifying", "Inspecting Authenticode signatures..."),
                    (token, progress) =>
                    {
                        List<AuthenticodeInspectionResult> results = new List<AuthenticodeInspectionResult>(files.Length);
                        for (int i = 0; i < files.Length; i++)
                        {
                            token.ThrowIfCancellationRequested();
                            try
                            {
                                results.Add(AuthenticodeService.Inspect(files[i]));
                            }
                            catch (Exception ex)
                            {
                                results.Add(new AuthenticodeInspectionResult
                                {
                                    FilePath = files[i],
                                    Error = ex.Message,
                                    Publisher = string.Empty
                                });
                            }
                            progress.Report(new OperationProgress { Current = i + 1, Total = files.Length, Message = Path.GetFileName(files[i]) });
                        }
                        return results;
                    });

                if (outcome.Error != null)
                {
                    FluentMessageBox.Show(this, outcome.Error.Message, ToolText.Get("authenticodeTitle", "Authenticode signature inspector"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (outcome.Cancelled || outcome.Value == null)
                    return;

                using (AuthenticodeResultsForm form = new AuthenticodeResultsForm(outcome.Value))
                    form.ShowDialog(this);
            }
        }

        private async void InspectCertificates_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = ToolText.Get("certificateInspectorTitle", "Certificate inspector");
                dialog.Filter = "Certificates (*.cer;*.crt;*.der;*.pem;*.pfx;*.p12;*.p7b)|*.cer;*.crt;*.der;*.pem;*.pfx;*.p12;*.p7b|All files (*.*)|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                string password = null;
                if (dialog.FileNames.Any(file =>
                    string.Equals(Path.GetExtension(file), ".pfx", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(Path.GetExtension(file), ".p12", StringComparison.OrdinalIgnoreCase)))
                {
                    using (PasswordPromptForm prompt = new PasswordPromptForm())
                    {
                        if (prompt.ShowDialog(this) != DialogResult.OK)
                            return;
                        password = prompt.Password;
                    }
                }

                string[] files = dialog.FileNames;
                ToolOperationOutcome<List<CertificateInspectionResult>> outcome = await RunToolOperationAsync(
                    ToolText.Get("certificateInspecting", "Inspecting certificates..."),
                    (token, progress) =>
                    {
                        List<CertificateInspectionResult> results = new List<CertificateInspectionResult>();
                        for (int i = 0; i < files.Length; i++)
                        {
                            token.ThrowIfCancellationRequested();
                            results.AddRange(CertificateService.InspectFiles(new[] { files[i] }, password));
                            progress.Report(new OperationProgress { Current = i + 1, Total = files.Length, Message = Path.GetFileName(files[i]) });
                        }
                        return results;
                    });

                if (outcome.Error != null)
                {
                    FluentMessageBox.Show(this, outcome.Error.Message, ToolText.Get("certificateInspectorTitle", "Certificate inspector"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (outcome.Cancelled || outcome.Value == null)
                    return;

                using (CertificateInspectionResultsForm form = new CertificateInspectionResultsForm(outcome.Value))
                    form.ShowDialog(this);
            }
        }

        private void GenerateCertificate_Click(object sender, EventArgs e)
        {
            using (CertificateGeneratorForm form = new CertificateGeneratorForm())
                form.ShowDialog(this);
        }

        private Task<ToolOperationOutcome<T>> RunToolOperationAsync<T>(
            string title,
            Func<CancellationToken, IProgress<OperationProgress>, T> operation)
        {
            ToolOperationOutcome<T> outcome = new ToolOperationOutcome<T>();
            using (OperationProgressForm progress = new OperationProgressForm(title))
            {
                progress.Shown += async (sender, args) =>
                {
                    try
                    {
                        outcome.Value = await Task.Run(() => operation(progress.Token, progress), progress.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        outcome.Cancelled = true;
                    }
                    catch (Exception ex)
                    {
                        outcome.Error = ex;
                    }
                    finally
                    {
                        progress.Complete();
                    }
                };
                progress.ShowDialog(this);
            }
            return Task.FromResult(outcome);
        }
    }
}
