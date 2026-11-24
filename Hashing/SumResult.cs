using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hashing
{
    public sealed class SumResult
    {
        public static readonly List<SumResult> Sums = new List<SumResult>();

        public string File { get; set; }
        public string MD5 { get; set; }
        public string SHA1 { get; set; }
        public string SHA256 { get; set; }
        public string SHA384 { get; set; }
        public string SHA512 { get; set; }
        public string CRC32 { get; set; }
        public string RIPEMD160 { get; set; }
        public string SHA3_256 { get; set; }
        public string SHA3_384 { get; set; }
        public string SHA3_512 { get; set; }

        public SumResult()
        {

        }

        public void ConvertToLowerCasing()
        {
            if (!string.IsNullOrEmpty(MD5)) MD5 = MD5.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA1)) SHA1 = SHA1.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA256)) SHA256 = SHA256.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA384)) SHA384 = SHA384.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA512)) SHA512 = SHA512.ToLowerInvariant();
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = CRC32.ToLowerInvariant();
            if (!string.IsNullOrEmpty(RIPEMD160)) RIPEMD160 = RIPEMD160.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA3_256)) SHA3_256 = SHA3_256.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA3_384)) SHA3_384 = SHA3_384.ToLowerInvariant();
            if (!string.IsNullOrEmpty(SHA3_512)) SHA3_512 = SHA3_512.ToLowerInvariant();
        }

        public void ConvertToUpperCasing()
        {
            if (!string.IsNullOrEmpty(MD5)) MD5 = MD5.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA1)) SHA1 = SHA1.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA256)) SHA256 = SHA256.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA384)) SHA384 = SHA384.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA512)) SHA512 = SHA512.ToUpperInvariant();
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = CRC32.ToUpperInvariant();
            if (!string.IsNullOrEmpty(RIPEMD160)) RIPEMD160 = RIPEMD160.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA3_256)) SHA3_256 = SHA3_256.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA3_384)) SHA3_384 = SHA3_384.ToUpperInvariant();
            if (!string.IsNullOrEmpty(SHA3_512)) SHA3_512 = SHA3_512.ToUpperInvariant();
        }

        public void ConvertCRC32ToDecimal()
        {
            if (!string.IsNullOrEmpty(CRC32))
                CRC32 = Convert.ToUInt32(CRC32, 16).ToString(CultureInfo.InvariantCulture);
        }

        public void ConvertCRC32ToHexadecimal()
        {
            uint value;
            if (uint.TryParse(CRC32, NumberStyles.None, CultureInfo.InvariantCulture, out value))
                CRC32 = value.ToString("X8", CultureInfo.InvariantCulture);
        }
    }
}
