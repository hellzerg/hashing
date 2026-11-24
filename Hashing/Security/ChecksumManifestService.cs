using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hashing
{
    internal enum ChecksumAlgorithm
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        CRC32,
        RIPEMD160,
        SHA3_256,
        SHA3_384,
        SHA3_512
    }

    internal enum ChecksumManifestFormat
    {
        Gnu,
        Bsd,
        Sfv
    }

    internal enum ChecksumVerificationStatus
    {
        Passed,
        Failed,
        Missing,
        Error
    }

    internal sealed class ChecksumManifestEntry
    {
        internal int LineNumber;
        internal string Path;
        internal ChecksumAlgorithm Algorithm;
        internal string ExpectedHash;
    }

    internal sealed class ChecksumManifestDocument
    {
        internal readonly List<ChecksumManifestEntry> Entries = new List<ChecksumManifestEntry>();
        internal readonly List<string> Warnings = new List<string>();
    }

    internal sealed class ChecksumVerificationResult
    {
        internal ChecksumVerificationStatus Status;
        internal string FilePath;
        internal string Algorithm;
        internal string ExpectedHash;
        internal string ActualHash;
        internal string Error;
    }

    internal sealed class OperationProgress
    {
        internal int Current;
        internal int Total;
        internal string Message;
    }

    internal static class ChecksumManifestService
    {
        private static readonly Regex BsdLine = new Regex(
            @"^(?<algorithm>SHA3[-]?512|SHA3[-]?384|SHA3[-]?256|SHA[-]?512|SHA[-]?384|SHA[-]?256|SHA[-]?1|MD5|RIPEMD[-]?160)\s+\((?<path>.*)\)\s*=\s*(?<hash>[0-9a-fA-F]+)\s*$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private static readonly Regex GnuLine = new Regex(
            @"^(?<hash>[0-9a-fA-F]{8,128}) (?<marker>[ *])(?<path>.+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Regex SfvLine = new Regex(
            @"^(?<path>.+?)[ \t]+(?<hash>[0-9a-fA-F]{8})$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        internal static ChecksumManifestDocument Read(string manifestPath)
        {
            if (string.IsNullOrWhiteSpace(manifestPath))
                throw new ArgumentException("A manifest path is required.", nameof(manifestPath));
            if (!File.Exists(manifestPath))
                throw new FileNotFoundException("The checksum manifest was not found.", manifestPath);

            string fullManifestPath = Path.GetFullPath(manifestPath);
            string baseDirectory = Path.GetDirectoryName(fullManifestPath);
            string extension = Path.GetExtension(fullManifestPath);
            bool isSfv = string.Equals(extension, ".sfv", StringComparison.OrdinalIgnoreCase);
            ChecksumManifestDocument document = new ChecksumManifestDocument();

            int index = 0;
            foreach (string manifestLine in File.ReadLines(fullManifestPath, Encoding.UTF8))
            {
                index++;
                string line = manifestLine;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string trimmed = line.TrimStart();
                if (trimmed.StartsWith("#", StringComparison.Ordinal) || trimmed.StartsWith(";", StringComparison.Ordinal))
                    continue;

                bool escaped = line.StartsWith("\\", StringComparison.Ordinal);
                if (escaped)
                    line = line.Substring(1);

                string fileName = null;
                string hash = null;
                ChecksumAlgorithm algorithm;
                Match match = BsdLine.Match(line);

                if (match.Success)
                {
                    if (!TryParseAlgorithm(match.Groups["algorithm"].Value, out algorithm))
                    {
                        document.Warnings.Add(string.Format(CultureInfo.InvariantCulture, "Line {0}: unsupported algorithm.", index));
                        continue;
                    }

                    fileName = match.Groups["path"].Value;
                    hash = match.Groups["hash"].Value;
                }
                else if (isSfv && (match = SfvLine.Match(line)).Success)
                {
                    algorithm = ChecksumAlgorithm.CRC32;
                    fileName = match.Groups["path"].Value;
                    hash = match.Groups["hash"].Value;
                }
                else if ((match = GnuLine.Match(line)).Success)
                {
                    hash = match.Groups["hash"].Value;
                    fileName = match.Groups["path"].Value;
                    fileName = escaped ? DecodeGnuFileName(fileName) : fileName;
                    if (!TryDetectAlgorithm(hash, extension, out algorithm))
                    {
                        document.Warnings.Add(string.Format(CultureInfo.InvariantCulture, "Line {0}: unsupported or ambiguous checksum length.", index));
                        continue;
                    }
                }
                else
                {
                    document.Warnings.Add(string.Format(CultureInfo.InvariantCulture, "Line {0}: unsupported checksum entry.", index));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(fileName) || !IsValidHash(algorithm, hash))
                {
                    document.Warnings.Add(string.Format(CultureInfo.InvariantCulture, "Line {0}: invalid file name or checksum.", index));
                    continue;
                }

                try
                {
                    string resolvedPath = Path.IsPathRooted(fileName)
                        ? Path.GetFullPath(fileName)
                        : Path.GetFullPath(Path.Combine(baseDirectory, fileName));

                    document.Entries.Add(new ChecksumManifestEntry
                    {
                        LineNumber = index,
                        Path = resolvedPath,
                        Algorithm = algorithm,
                        ExpectedHash = hash.ToUpperInvariant()
                    });
                }
                catch (Exception ex)
                {
                    document.Warnings.Add(string.Format(CultureInfo.InvariantCulture, "Line {0}: invalid path ({1}).", index, ex.Message));
                }
            }

            if (document.Entries.Count == 0)
                throw new FormatException("The selected file contains no supported checksum entries.");

            return document;
        }

        internal static void Create(
            string outputPath,
            ChecksumAlgorithm algorithm,
            ChecksumManifestFormat format,
            IEnumerable<string> filePaths,
            CancellationToken cancellationToken,
            IProgress<OperationProgress> progress)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("An output path is required.", nameof(outputPath));
            if (filePaths == null)
                throw new ArgumentNullException(nameof(filePaths));
            if (format == ChecksumManifestFormat.Sfv && algorithm != ChecksumAlgorithm.CRC32)
                throw new ArgumentException("SFV manifests support CRC32 only.", nameof(format));

            List<string> files = filePaths
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .Select(Path.GetFullPath)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (files.Count == 0)
                throw new InvalidOperationException("There are no calculated files to export.");

            string destination = Path.GetFullPath(outputPath);
            if (files.Contains(destination, StringComparer.OrdinalIgnoreCase))
                throw new IOException(ToolText.Get("manifestCannotReplaceInput", "The checksum manifest cannot replace one of the files being hashed."));

            string directory = Path.GetDirectoryName(destination);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string temporaryPath = destination + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (StreamWriter writer = new StreamWriter(temporaryPath, false, new UTF8Encoding(false)))
                {
                    if (format == ChecksumManifestFormat.Sfv)
                        writer.WriteLine("; Generated by Hashing");
                    else
                        writer.WriteLine("# Generated by Hashing");

                    for (int index = 0; index < files.Count; index++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        string file = files[index];
                        if (!File.Exists(file))
                            throw new FileNotFoundException("A file from the current results no longer exists.", file);

                        string hash = CalculateHash(file, algorithm, cancellationToken);
                        string relativePath = GetManifestPath(directory, file);
                        string line;

                        switch (format)
                        {
                            case ChecksumManifestFormat.Bsd:
                                line = string.Format(CultureInfo.InvariantCulture, "{0} ({1}) = {2}", GetAlgorithmName(algorithm), relativePath, hash);
                                break;
                            case ChecksumManifestFormat.Sfv:
                                line = relativePath + " " + hash;
                                break;
                            default:
                                bool needsEscape;
                                string escapedPath = EncodeGnuFileName(relativePath, out needsEscape);
                                line = (needsEscape ? "\\" : string.Empty) + hash + " *" + escapedPath;
                                break;
                        }

                        writer.WriteLine(line);
                        if (progress != null)
                            progress.Report(new OperationProgress { Current = index + 1, Total = files.Count, Message = Path.GetFileName(file) });
                    }

                    writer.Flush();
                }

                cancellationToken.ThrowIfCancellationRequested();
                ReplaceFile(temporaryPath, destination);
            }
            finally
            {
                TryDelete(temporaryPath);
            }
        }

        internal static List<ChecksumVerificationResult> Verify(
            ChecksumManifestDocument document,
            CancellationToken cancellationToken,
            IProgress<OperationProgress> progress)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            List<ChecksumVerificationResult> results = new List<ChecksumVerificationResult>(document.Entries.Count);
            Dictionary<string, string> hashCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            for (int index = 0; index < document.Entries.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ChecksumManifestEntry entry = document.Entries[index];
                ChecksumVerificationResult result = new ChecksumVerificationResult
                {
                    FilePath = entry.Path,
                    Algorithm = GetAlgorithmName(entry.Algorithm),
                    ExpectedHash = entry.ExpectedHash
                };

                if (!File.Exists(entry.Path))
                {
                    result.Status = ChecksumVerificationStatus.Missing;
                    result.Error = "File not found.";
                }
                else
                {
                    string cacheKey = entry.Algorithm.ToString() + "|" + entry.Path;
                    try
                    {
                        string actual;
                        if (!hashCache.TryGetValue(cacheKey, out actual))
                        {
                            actual = CalculateHash(entry.Path, entry.Algorithm, cancellationToken);
                            hashCache.Add(cacheKey, actual);
                        }

                        result.ActualHash = actual;
                        result.Status = string.Equals(actual, entry.ExpectedHash, StringComparison.OrdinalIgnoreCase)
                            ? ChecksumVerificationStatus.Passed
                            : ChecksumVerificationStatus.Failed;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        result.Status = ChecksumVerificationStatus.Error;
                        result.Error = ex.Message;
                    }
                }

                results.Add(result);
                if (progress != null)
                    progress.Report(new OperationProgress { Current = index + 1, Total = document.Entries.Count, Message = Path.GetFileName(entry.Path) });
            }

            return results;
        }

        internal static string CalculateHash(string file, ChecksumAlgorithm algorithm, CancellationToken cancellationToken)
        {
            HashOptions options = new HashOptions();
            switch (algorithm)
            {
                case ChecksumAlgorithm.MD5: options.MD5 = true; break;
                case ChecksumAlgorithm.SHA1: options.SHA1 = true; break;
                case ChecksumAlgorithm.SHA256: options.SHA256 = true; break;
                case ChecksumAlgorithm.SHA384: options.SHA384 = true; break;
                case ChecksumAlgorithm.SHA512: options.SHA512 = true; break;
                case ChecksumAlgorithm.CRC32: options.CRC32 = true; break;
                case ChecksumAlgorithm.RIPEMD160: options.RIPEMD160 = true; break;
                case ChecksumAlgorithm.SHA3_256: options.SHA3_256 = true; break;
                case ChecksumAlgorithm.SHA3_384: options.SHA3_384 = true; break;
                case ChecksumAlgorithm.SHA3_512: options.SHA3_512 = true; break;
                default: throw new ArgumentOutOfRangeException(nameof(algorithm));
            }

            SumResult result = Utilities.CalculateSums(file, cancellationToken, options);
            string hash;
            switch (algorithm)
            {
                case ChecksumAlgorithm.MD5: hash = result.MD5; break;
                case ChecksumAlgorithm.SHA1: hash = result.SHA1; break;
                case ChecksumAlgorithm.SHA256: hash = result.SHA256; break;
                case ChecksumAlgorithm.SHA384: hash = result.SHA384; break;
                case ChecksumAlgorithm.SHA512: hash = result.SHA512; break;
                case ChecksumAlgorithm.CRC32: hash = result.CRC32; break;
                case ChecksumAlgorithm.RIPEMD160: hash = result.RIPEMD160; break;
                case ChecksumAlgorithm.SHA3_256: hash = result.SHA3_256; break;
                case ChecksumAlgorithm.SHA3_384: hash = result.SHA3_384; break;
                case ChecksumAlgorithm.SHA3_512: hash = result.SHA3_512; break;
                default: throw new ArgumentOutOfRangeException(nameof(algorithm));
            }

            if (algorithm == ChecksumAlgorithm.CRC32 && Options.CurrentOptions.CRC32Decimal)
            {
                uint decimalValue;
                if (!uint.TryParse(hash, NumberStyles.None, CultureInfo.InvariantCulture, out decimalValue))
                    throw new FormatException("The CRC32 result could not be converted to hexadecimal.");
                hash = decimalValue.ToString("X8", CultureInfo.InvariantCulture);
            }

            return hash.ToUpperInvariant();
        }

        internal static string GetAlgorithmName(ChecksumAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case ChecksumAlgorithm.MD5: return "MD5";
                case ChecksumAlgorithm.SHA1: return "SHA1";
                case ChecksumAlgorithm.SHA256: return "SHA256";
                case ChecksumAlgorithm.SHA384: return "SHA384";
                case ChecksumAlgorithm.SHA512: return "SHA512";
                case ChecksumAlgorithm.CRC32: return "CRC32";
                case ChecksumAlgorithm.RIPEMD160: return "RIPEMD160";
                case ChecksumAlgorithm.SHA3_256: return "SHA3-256";
                case ChecksumAlgorithm.SHA3_384: return "SHA3-384";
                case ChecksumAlgorithm.SHA3_512: return "SHA3-512";
                default: return algorithm.ToString();
            }
        }

        private static bool TryParseAlgorithm(string value, out ChecksumAlgorithm algorithm)
        {
            string normalized = (value ?? string.Empty).Replace("-", string.Empty).Replace("_", string.Empty).Trim().ToUpperInvariant();
            switch (normalized)
            {
                case "MD5": algorithm = ChecksumAlgorithm.MD5; return true;
                case "SHA1": algorithm = ChecksumAlgorithm.SHA1; return true;
                case "SHA256": algorithm = ChecksumAlgorithm.SHA256; return true;
                case "SHA384": algorithm = ChecksumAlgorithm.SHA384; return true;
                case "SHA512": algorithm = ChecksumAlgorithm.SHA512; return true;
                case "CRC32": algorithm = ChecksumAlgorithm.CRC32; return true;
                case "RIPEMD160": algorithm = ChecksumAlgorithm.RIPEMD160; return true;
                case "SHA3256": algorithm = ChecksumAlgorithm.SHA3_256; return true;
                case "SHA3384": algorithm = ChecksumAlgorithm.SHA3_384; return true;
                case "SHA3512": algorithm = ChecksumAlgorithm.SHA3_512; return true;
                default: algorithm = default(ChecksumAlgorithm); return false;
            }
        }

        private static bool TryDetectAlgorithm(string hash, string extension, out ChecksumAlgorithm algorithm)
        {
            string ext = (extension ?? string.Empty).ToLowerInvariant();
            switch (ext)
            {
                case ".md5": case ".md5.bsd": algorithm = ChecksumAlgorithm.MD5; break;
                case ".sha1": case ".sha1.bsd": algorithm = ChecksumAlgorithm.SHA1; break;
                case ".sha256": case ".sha256.bsd": algorithm = ChecksumAlgorithm.SHA256; break;
                case ".sha384": case ".sha384.bsd": algorithm = ChecksumAlgorithm.SHA384; break;
                case ".sha512": case ".sha512.bsd": algorithm = ChecksumAlgorithm.SHA512; break;
                case ".sfv": algorithm = ChecksumAlgorithm.CRC32; break;
                default:
                    switch (hash.Length)
                    {
                        case 8: algorithm = ChecksumAlgorithm.CRC32; break;
                        case 32: algorithm = ChecksumAlgorithm.MD5; break;
                        case 40: algorithm = ChecksumAlgorithm.SHA1; break;
                        case 64: algorithm = ChecksumAlgorithm.SHA256; break;
                        case 96: algorithm = ChecksumAlgorithm.SHA384; break;
                        case 128: algorithm = ChecksumAlgorithm.SHA512; break;
                        default: algorithm = default(ChecksumAlgorithm); return false;
                    }
                    break;
            }

            return IsValidHash(algorithm, hash);
        }

        private static bool IsValidHash(ChecksumAlgorithm algorithm, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash) || hash.Any(character => !Uri.IsHexDigit(character)))
                return false;

            int expectedLength;
            switch (algorithm)
            {
                case ChecksumAlgorithm.MD5: expectedLength = 32; break;
                case ChecksumAlgorithm.SHA1: expectedLength = 40; break;
                case ChecksumAlgorithm.SHA256: case ChecksumAlgorithm.SHA3_256: expectedLength = 64; break;
                case ChecksumAlgorithm.SHA384: case ChecksumAlgorithm.SHA3_384: expectedLength = 96; break;
                case ChecksumAlgorithm.SHA512: case ChecksumAlgorithm.SHA3_512: expectedLength = 128; break;
                case ChecksumAlgorithm.CRC32: expectedLength = 8; break;
                case ChecksumAlgorithm.RIPEMD160: expectedLength = 40; break;
                default: return false;
            }

            return hash.Length == expectedLength;
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
            Uri baseUri = new Uri(baseUriPath);
            Uri targetUri = new Uri(targetPath);
            string relative = Uri.UnescapeDataString(baseUri.MakeRelativeUri(targetUri).ToString());
            return relative.Replace('/', Path.DirectorySeparatorChar);
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

        private static string DecodeGnuFileName(string fileName)
        {
            StringBuilder result = new StringBuilder(fileName.Length);
            for (int i = 0; i < fileName.Length; i++)
            {
                char current = fileName[i];
                if (current == '\\' && i + 1 < fileName.Length)
                {
                    char next = fileName[++i];
                    switch (next)
                    {
                        case '\\': result.Append('\\'); break;
                        case 't': result.Append('\t'); break;
                        case 'r': result.Append('\r'); break;
                        case 'n': result.Append('\n'); break;
                        default: result.Append(next); break;
                    }
                }
                else
                {
                    result.Append(current);
                }
            }
            return result.ToString();
        }

        private static void ReplaceFile(string temporaryPath, string destinationPath)
        {
            if (File.Exists(destinationPath))
                File.Replace(temporaryPath, destinationPath, null);
            else
                File.Move(temporaryPath, destinationPath);
        }

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); }
            catch { }
        }
    }
}
