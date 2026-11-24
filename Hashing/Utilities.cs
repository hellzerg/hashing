using Force.Crc32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hashing
{
    internal static class Utilities
    {
        private const int BufferSize = 64 * 1024;
        [ThreadStatic]
        private static byte[] _hashBuffer;
        [ThreadStatic]
        private static byte[] _comparisonLeftBuffer;
        [ThreadStatic]
        private static byte[] _comparisonRightBuffer;

        internal static SumResult CalculateSums(string file, CancellationToken cancellationToken)
        {
            return CalculateSums(file, cancellationToken, Options.CurrentOptions.HashOptions);
        }

        internal static SumResult CalculateSums(string file, CancellationToken cancellationToken, HashOptions options)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException("A file path is required.", nameof(file));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            bool lowerCasing = Options.CurrentOptions.LowerCasing;
            bool crc32Decimal = Options.CurrentOptions.CRC32Decimal;
            SumResult result = new SumResult { File = file };
            List<HashAlgorithm> algorithms = new List<HashAlgorithm>(10);

            HashAlgorithm md5 = null;
            HashAlgorithm sha1 = null;
            HashAlgorithm sha256 = null;
            HashAlgorithm sha384 = null;
            HashAlgorithm sha512 = null;
            HashAlgorithm crc32 = null;
            HashAlgorithm ripemd160 = null;
            HashAlgorithm sha3_256 = null;
            HashAlgorithm sha3_384 = null;
            HashAlgorithm sha3_512 = null;

            try
            {
                if (options.MD5) { md5 = new MD5CryptoServiceProvider(); algorithms.Add(md5); }
                if (options.SHA1) { sha1 = new SHA1CryptoServiceProvider(); algorithms.Add(sha1); }
                if (options.SHA256) { sha256 = new SHA256CryptoServiceProvider(); algorithms.Add(sha256); }
                if (options.SHA384) { sha384 = new SHA384CryptoServiceProvider(); algorithms.Add(sha384); }
                if (options.SHA512) { sha512 = new SHA512CryptoServiceProvider(); algorithms.Add(sha512); }
                if (options.CRC32) { crc32 = new Crc32Algorithm(); algorithms.Add(crc32); }
                if (options.RIPEMD160) { ripemd160 = RIPEMD160.Create(); algorithms.Add(ripemd160); }
                if (options.SHA3_256) { sha3_256 = Sha3.Sha3256(); algorithms.Add(sha3_256); }
                if (options.SHA3_384) { sha3_384 = Sha3.Sha3384(); algorithms.Add(sha3_384); }
                if (options.SHA3_512) { sha3_512 = Sha3.Sha3512(); algorithms.Add(sha3_512); }

                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan))
                {
                    if (algorithms.Count > 0)
                    {
                        byte[] buffer = _hashBuffer ?? (_hashBuffer = new byte[BufferSize]);
                        int read;

                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            for (int i = 0; i < algorithms.Count; i++)
                                algorithms[i].TransformBlock(buffer, 0, read, buffer, 0);
                        }

                        cancellationToken.ThrowIfCancellationRequested();
                        byte[] empty = Array.Empty<byte>();
                        for (int i = 0; i < algorithms.Count; i++)
                            algorithms[i].TransformFinalBlock(empty, 0, 0);
                    }
                }

                if (md5 != null) result.MD5 = FormatHash(md5.Hash);
                if (sha1 != null) result.SHA1 = FormatHash(sha1.Hash);
                if (sha256 != null) result.SHA256 = FormatHash(sha256.Hash);
                if (sha384 != null) result.SHA384 = FormatHash(sha384.Hash);
                if (sha512 != null) result.SHA512 = FormatHash(sha512.Hash);
                if (crc32 != null)
                {
                    result.CRC32 = FormatHash(crc32.Hash);
                    if (crc32Decimal)
                        result.CRC32 = Convert.ToUInt32(result.CRC32, 16).ToString(CultureInfo.InvariantCulture);
                }
                if (ripemd160 != null) result.RIPEMD160 = FormatHash(ripemd160.Hash);
                if (sha3_256 != null) result.SHA3_256 = FormatHash(sha3_256.Hash);
                if (sha3_384 != null) result.SHA3_384 = FormatHash(sha3_384.Hash);
                if (sha3_512 != null) result.SHA3_512 = FormatHash(sha3_512.Hash);

                if (lowerCasing)
                    result.ConvertToLowerCasing();
                else
                    result.ConvertToUpperCasing();

                return result;
            }
            finally
            {
                for (int i = 0; i < algorithms.Count; i++)
                    algorithms[i].Dispose();
            }
        }

        private const string HexCharacters = "0123456789ABCDEF";

        private static string FormatHash(byte[] hash)
        {
            char[] characters = new char[hash.Length * 2];
            for (int i = 0, j = 0; i < hash.Length; i++)
            {
                byte value = hash[i];
                characters[j++] = HexCharacters[value >> 4];
                characters[j++] = HexCharacters[value & 0x0F];
            }
            return new string(characters);
        }

        internal static bool TryAreFilesIdentical(SumResult left, SumResult right, CancellationToken cancellationToken, out bool identical, out string error)
        {
            identical = false;
            error = null;
            if (left == null || right == null || string.IsNullOrWhiteSpace(left.File) || string.IsNullOrWhiteSpace(right.File))
                return true;

            try
            {
                string leftPath = Path.GetFullPath(left.File);
                string rightPath = Path.GetFullPath(right.File);

                if (string.Equals(leftPath, rightPath, StringComparison.Ordinal))
                {
                    using (FileStream stream = new FileStream(leftPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan))
                        identical = true;
                    return true;
                }

                using (FileStream leftStream = new FileStream(leftPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan))
                using (FileStream rightStream = new FileStream(rightPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan))
                {
                    if (leftStream.Length != rightStream.Length)
                        return true;

                    byte[] leftBuffer = _comparisonLeftBuffer ?? (_comparisonLeftBuffer = new byte[BufferSize]);
                    byte[] rightBuffer = _comparisonRightBuffer ?? (_comparisonRightBuffer = new byte[BufferSize]);

                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        int leftRead = ReadBlock(leftStream, leftBuffer, leftBuffer.Length);
                        int rightRead = ReadBlock(rightStream, rightBuffer, rightBuffer.Length);
                        if (leftRead != rightRead)
                            return true;
                        if (leftRead == 0)
                        {
                            identical = true;
                            return true;
                        }

                        for (int i = 0; i < leftRead; i++)
                        {
                            if (leftBuffer[i] != rightBuffer[i])
                                return true;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static int ReadBlock(Stream stream, byte[] buffer, int count)
        {
            int total = 0;
            while (total < count)
            {
                int read = stream.Read(buffer, total, count - total);
                if (read == 0)
                    break;
                total += read;
            }
            return total;
        }

        internal static List<List<SumResult>> FindDuplicateGroups(IEnumerable<SumResult> items, Func<SumResult, string> selector)
        {
            Dictionary<string, List<SumResult>> lookup = new Dictionary<string, List<SumResult>>(StringComparer.OrdinalIgnoreCase);
            List<List<SumResult>> orderedGroups = new List<List<SumResult>>();

            foreach (SumResult item in items)
            {
                if (item == null) continue;

                string hash = selector(item);
                if (string.IsNullOrEmpty(hash)) continue;

                List<SumResult> group;
                if (!lookup.TryGetValue(hash, out group))
                {
                    group = new List<SumResult>();
                    lookup.Add(hash, group);
                    orderedGroups.Add(group);
                }
                group.Add(item);
            }

            List<List<SumResult>> duplicates = new List<List<SumResult>>();
            foreach (List<SumResult> group in orderedGroups)
            {
                if (group.Count > 1)
                    duplicates.Add(group);
            }

            return duplicates;
        }

        internal static void CopyToClipboard(string text, bool all = false)
        {
            try
            {
                if (!all)
                {
                    text = text.Replace("MD5:", string.Empty);
                    text = text.Replace("SHA1:", string.Empty);
                    text = text.Replace("SHA256:", string.Empty);
                    text = text.Replace("SHA384:", string.Empty);
                    text = text.Replace("SHA512:", string.Empty);
                    text = text.Replace("CRC32:", string.Empty);
                    text = text.Replace("RIPEMD160:", string.Empty);
                    text = text.Replace("SHA3-256:", string.Empty);
                    text = text.Replace("SHA3-384:", string.Empty);
                    text = text.Replace("SHA3-512:", string.Empty);
                }

                Clipboard.SetText(text.Trim());
            }
            catch { }
        }

        internal static bool TrySetProcessPriority(ProcessPriorityClass priority, out string error)
        {
            try
            {
                using (Process process = Process.GetCurrentProcess())
                    process.PriorityClass = priority;
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        internal static void SearchVirusTotal(string hash)
        {
            try
            {
                Process.Start(string.Format("https://www.virustotal.com/#/file/{0}/detection", hash.ToLowerInvariant()));
            }
            catch { }
        }

        internal static void SearchHash(string hash, bool ddg)
        {
            hash = hash.Replace("MD5:", string.Empty);
            hash = hash.Replace("SHA1:", string.Empty);
            hash = hash.Replace("SHA256:", string.Empty);
            hash = hash.Replace("SHA384:", string.Empty);
            hash = hash.Replace("SHA512:", string.Empty);
            hash = hash.Replace("CRC32:", string.Empty);
            hash = hash.Replace("RIPEMD160:", string.Empty);
            hash = hash.Replace("SHA3-256:", string.Empty);
            hash = hash.Replace("SHA3-384:", string.Empty);
            hash = hash.Replace("SHA3-512:", string.Empty);

            try
            {
                if (ddg) Process.Start(string.Format("https://duckduckgo.com/?q=\"{0}\"", hash.Trim()));
                if (!ddg) Process.Start(string.Format("https://www.google.com/search?q=\"{0}\"", hash.Trim()));
            }
            catch { }
        }

        internal static bool TryFindFile(string file, out Exception error)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(file))
                    throw new ArgumentException("A file path is required.", nameof(file));

                File.GetAttributes(file);
                Process.Start(new ProcessStartInfo("explorer.exe", "/select, \"" + file + "\"")
                {
                    UseShellExecute = true
                });
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }
    }
}
