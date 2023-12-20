using System;
using System.Collections.Generic;

namespace Hashing
{
    public class SumResult
    {
        public static List<SumResult> Sums = new List<SumResult>();

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

        public SumResult() { }

        private void ConvertHashes(Func<string, string> converter)
        {
            MD5 = converter(MD5);
            SHA1 = converter(SHA1);
            SHA256 = converter(SHA256);
            SHA384 = converter(SHA384);
            SHA512 = converter(SHA512);
            CRC32 = converter(CRC32);
            RIPEMD160 = converter(RIPEMD160);
            SHA3_256 = converter(SHA3_256);
            SHA3_384 = converter(SHA3_384);
            SHA3_512 = converter(SHA3_512);
        }

        public void ConvertToLowerCasing() => ConvertHashes(s => s?.ToLower());

        public void ConvertToUpperCasing() => ConvertHashes(s => s?.ToUpper());

        public void ConvertCRC32ToDecimal() => CRC32 = !string.IsNullOrEmpty(CRC32) ? Convert.ToInt64(CRC32, 16).ToString() : CRC32;

        public void ConvertCRC32ToHexadecimal() => CRC32 = !string.IsNullOrEmpty(CRC32) ? string.Format("{0:x}", Convert.ToInt64(CRC32)) : CRC32;
    }
}
