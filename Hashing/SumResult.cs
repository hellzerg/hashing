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

        public SumResult()
        {

        }

        public void ConvertToLowerCasing()
        {
            if (!string.IsNullOrEmpty(MD5)) MD5 = MD5.ToLower();
            if (!string.IsNullOrEmpty(SHA1)) SHA1 = SHA1.ToLower();
            if (!string.IsNullOrEmpty(SHA256)) SHA256 = SHA256.ToLower();
            if (!string.IsNullOrEmpty(SHA384)) SHA384 = SHA384.ToLower();
            if (!string.IsNullOrEmpty(SHA512)) SHA512 = SHA512.ToLower();
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = CRC32.ToLower();
            if (!string.IsNullOrEmpty(RIPEMD160)) RIPEMD160 = RIPEMD160.ToLower();
            if (!string.IsNullOrEmpty(SHA3_256)) SHA3_256 = SHA3_256.ToLower();
            if (!string.IsNullOrEmpty(SHA3_384)) SHA3_384 = SHA3_384.ToLower();
            if (!string.IsNullOrEmpty(SHA3_512)) SHA3_512 = SHA3_512.ToLower();
        }

        public void ConvertToUpperCasing()
        {
            if (!string.IsNullOrEmpty(MD5)) MD5 = MD5.ToUpper();
            if (!string.IsNullOrEmpty(SHA1)) SHA1 = SHA1.ToUpper();
            if (!string.IsNullOrEmpty(SHA256)) SHA256 = SHA256.ToUpper();
            if (!string.IsNullOrEmpty(SHA384)) SHA384 = SHA384.ToUpper();
            if (!string.IsNullOrEmpty(SHA512)) SHA512 = SHA512.ToUpper();
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = CRC32.ToUpper();
            if (!string.IsNullOrEmpty(RIPEMD160)) RIPEMD160 = RIPEMD160.ToUpper();
            if (!string.IsNullOrEmpty(SHA3_256)) SHA3_256 = SHA3_256.ToUpper();
            if (!string.IsNullOrEmpty(SHA3_384)) SHA3_384 = SHA3_384.ToUpper();
            if (!string.IsNullOrEmpty(SHA3_512)) SHA3_512 = SHA3_512.ToUpper();
        }

        public void ConvertCRC32ToDecimal()
        {
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = Convert.ToInt64(CRC32, 16).ToString();
        }

        public void ConvertCRC32ToHexadecimal()
        {
            if (!string.IsNullOrEmpty(CRC32)) CRC32 = string.Format("{0:x}", CRC32);
        }
    }
}
