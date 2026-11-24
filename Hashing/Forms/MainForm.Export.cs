using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hashing
{
    public sealed partial class MainForm
    {
        private enum HashExportFormat
        {
            Json,
            Csv,
            Text,
            Md5,
            Sha1,
            Sha256
        }

        private void ShowExportMenu()
        {
            if (_exportMenu == null || btnSaveJson == null)
                return;

            SumResult[] results = SumResult.Sums == null
                ? new SumResult[0]
                : SumResult.Sums.Where(item => item != null && !string.IsNullOrWhiteSpace(item.File)).ToArray();
            bool hasResults = results.Length > 0;
            _exportJsonMenuItem.Enabled = hasResults;
            _exportCsvMenuItem.Enabled = hasResults;
            _exportTxtMenuItem.Enabled = hasResults;
            _exportMd5MenuItem.Enabled = HasCompleteHash(results, item => item.MD5);
            _exportSha1MenuItem.Enabled = HasCompleteHash(results, item => item.SHA1);
            _exportSha256MenuItem.Enabled = HasCompleteHash(results, item => item.SHA256);
            _exportMenu.Show(btnSaveJson, new System.Drawing.Point(0, btnSaveJson.Height));
        }

        private void ExportHashResults(HashExportFormat format)
        {
            SumResult[] results = SumResult.Sums == null
                ? new SumResult[0]
                : SumResult.Sums.Where(item => item != null && !string.IsNullOrWhiteSpace(item.File)).ToArray();

            if (results.Length == 0)
                return;

            string filter;
            string extension;
            switch (format)
            {
                case HashExportFormat.Csv:
                    filter = "CSV file (*.csv)|*.csv";
                    extension = "csv";
                    break;
                case HashExportFormat.Text:
                    filter = "Text file (*.txt)|*.txt";
                    extension = "txt";
                    break;
                case HashExportFormat.Md5:
                    filter = "MD5 checksum (*.md5)|*.md5";
                    extension = "md5";
                    break;
                case HashExportFormat.Sha1:
                    filter = "SHA-1 checksum (*.sha1)|*.sha1";
                    extension = "sha1";
                    break;
                case HashExportFormat.Sha256:
                    filter = "SHA-256 checksum (*.sha256)|*.sha256";
                    extension = "sha256";
                    break;
                default:
                    filter = "JSON file (*.json)|*.json";
                    extension = "json";
                    break;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = ToolText.Get("exportDialogTitle", "Export hash results");
                dialog.Filter = filter;
                dialog.DefaultExt = extension;
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                dialog.FileName = "hashes." + extension;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    string outputPath = Path.GetFullPath(dialog.FileName);
                    if (results.Any(item => IsSourcePath(item.File, outputPath)))
                    {
                        FluentMessageBox.Show(this,
                            ToolText.Get("exportCannotOverwriteSource", "The export destination is one of the files being hashed. Choose a different file."),
                            ToolText.Get("exportDialogTitle", "Export hash results"),
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    switch (format)
                    {
                        case HashExportFormat.Csv:
                            WriteCsv(outputPath, results);
                            break;
                        case HashExportFormat.Text:
                            WriteText(outputPath, results);
                            break;
                        case HashExportFormat.Md5:
                            WriteChecksumManifest(outputPath, results, item => item.MD5, 32);
                            break;
                        case HashExportFormat.Sha1:
                            WriteChecksumManifest(outputPath, results, item => item.SHA1, 40);
                            break;
                        case HashExportFormat.Sha256:
                            WriteChecksumManifest(outputPath, results, item => item.SHA256, 64);
                            break;
                        default:
                            File.WriteAllText(outputPath, JsonConvert.SerializeObject(results, Formatting.Indented,
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }), new UTF8Encoding(false));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    FluentMessageBox.Show(this,
                        FormatFileOperationError(ToolText.Get("operationExport", "export results"), dialog.FileName, ex),
                        ToolText.Get("fileOperationErrorsTitle", "File operation errors"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private static bool HasCompleteHash(SumResult[] results, Func<SumResult, string> selector)
        {
            return results != null && results.Length > 0 &&
                results.All(item => !string.IsNullOrWhiteSpace(selector(item)));
        }

        private static void WriteChecksumManifest(string path, SumResult[] results, Func<SumResult, string> selector, int expectedLength)
        {
            if (results == null || results.Length == 0 || !HasCompleteHash(results, selector))
                throw new InvalidOperationException("The selected checksum was not calculated for every result.");

            string destination = Path.GetFullPath(path);
            string directory = Path.GetDirectoryName(destination);
            if (string.IsNullOrEmpty(directory))
                throw new IOException("The export directory is invalid.");
            Directory.CreateDirectory(directory);

            string temporaryPath = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (StreamWriter writer = new StreamWriter(temporaryPath, false, new UTF8Encoding(false)))
                {
                    writer.WriteLine("# Generated by Hashing");
                    foreach (SumResult result in results)
                    {
                        string hash = selector(result).Trim();
                        if (hash.Length != expectedLength || hash.Any(character => !Uri.IsHexDigit(character)))
                            throw new FormatException("A calculated checksum has an invalid format.");

                        string filePath = Path.GetFullPath(result.File);
                        string manifestPath = GetManifestPath(directory, filePath);
                        bool escaped;
                        string escapedPath = EncodeGnuFileName(manifestPath, out escaped);
                        writer.WriteLine((escaped ? "\\" : string.Empty) + hash.ToLowerInvariant() + " *" + escapedPath);
                    }
                    writer.Flush();
                }

                if (File.Exists(destination))
                    File.Replace(temporaryPath, destination, null);
                else
                    File.Move(temporaryPath, destination);
            }
            finally
            {
                try { if (File.Exists(temporaryPath)) File.Delete(temporaryPath); }
                catch { }
            }
        }

        private static string GetManifestPath(string manifestDirectory, string filePath)
        {
            string basePath = Path.GetFullPath(manifestDirectory);
            string targetPath = Path.GetFullPath(filePath);
            string baseRoot = Path.GetPathRoot(basePath);
            string targetRoot = Path.GetPathRoot(targetPath);
            if (!string.Equals(baseRoot, targetRoot, StringComparison.OrdinalIgnoreCase))
                return targetPath;

            string baseUriPath = basePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal)
                ? basePath
                : basePath + Path.DirectorySeparatorChar;
            string relative = Uri.UnescapeDataString(new Uri(baseUriPath).MakeRelativeUri(new Uri(targetPath)).ToString());
            return relative.Replace('\\', '/');
        }

        private static string EncodeGnuFileName(string fileName, out bool escaped)
        {
            escaped = fileName.IndexOfAny(new[] { '\\', '\t', '\r', '\n' }) >= 0;
            if (!escaped)
                return fileName;

            return fileName.Replace("\\", "\\\\")
                .Replace("\t", "\\t")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }

        private static bool IsSourcePath(string sourcePath, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                return false;

            try
            {
                return string.Equals(Path.GetFullPath(sourcePath), outputPath, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void WriteCsv(string path, SumResult[] results)
        {
            string[] headers =
            {
                "File", "MD5", "SHA1", "SHA256", "SHA384", "SHA512", "CRC32",
                "RIPEMD160", "SHA3-256", "SHA3-384", "SHA3-512"
            };

            using (StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                writer.WriteLine(string.Join(",", headers.Select(EscapeCsv)));
                foreach (SumResult result in results)
                {
                    string[] values =
                    {
                        result.File, result.MD5, result.SHA1, result.SHA256, result.SHA384,
                        result.SHA512, result.CRC32, result.RIPEMD160, result.SHA3_256,
                        result.SHA3_384, result.SHA3_512
                    };
                    writer.WriteLine(string.Join(",", values.Select(EscapeCsv)));
                }
            }
        }

        private static void WriteText(string path, SumResult[] results)
        {
            using (StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                foreach (SumResult result in results)
                {
                    writer.WriteLine("File: " + (result.File ?? string.Empty));
                    WriteHash(writer, "MD5", result.MD5);
                    WriteHash(writer, "SHA1", result.SHA1);
                    WriteHash(writer, "SHA256", result.SHA256);
                    WriteHash(writer, "SHA384", result.SHA384);
                    WriteHash(writer, "SHA512", result.SHA512);
                    WriteHash(writer, "CRC32", result.CRC32);
                    WriteHash(writer, "RIPEMD160", result.RIPEMD160);
                    WriteHash(writer, "SHA3-256", result.SHA3_256);
                    WriteHash(writer, "SHA3-384", result.SHA3_384);
                    WriteHash(writer, "SHA3-512", result.SHA3_512);
                    writer.WriteLine();
                }
            }
        }

        private static void WriteHash(TextWriter writer, string algorithm, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                writer.WriteLine(algorithm + ": " + value);
        }

        private static string EscapeCsv(string value)
        {
            value = value ?? string.Empty;
            if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) < 0)
                return value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
