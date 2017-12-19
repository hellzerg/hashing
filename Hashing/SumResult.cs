using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SumResult()
        {
            MD5 = string.Empty;
            SHA1 = string.Empty;
            SHA256 = string.Empty;
            SHA384 = string.Empty;
            SHA512 = string.Empty;
            CRC32 = string.Empty;
            RIPEMD160 = string.Empty;
        }

        public void ConvertToLowerCasing()
        {
            MD5 = MD5.ToLower();
            SHA1 = SHA1.ToLower();
            SHA256 = SHA256.ToLower();
            SHA384 = SHA384.ToLower();
            SHA512 = SHA512.ToLower();
            CRC32 = CRC32.ToLower();
            RIPEMD160 = RIPEMD160.ToLower();
        }

        public void ConvertToUpperCasing()
        {
            MD5 = MD5.ToUpper();
            SHA1 = SHA1.ToUpper();
            SHA256 = SHA256.ToUpper();
            SHA384 = SHA384.ToUpper();
            SHA512 = SHA512.ToUpper();
            CRC32 = CRC32.ToUpper();
            RIPEMD160 = RIPEMD160.ToUpper();
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
