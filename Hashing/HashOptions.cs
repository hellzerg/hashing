using System;

namespace Hashing
{
    [Serializable]
    public class HashOptions
    {
        public bool MD5 { get; set; }
        public bool SHA1 { get; set; }
        public bool SHA256 { get; set; }
        public bool SHA384 { get; set; }
        public bool SHA512 { get; set; }
        public bool CRC32 { get; set; }
        public bool RIPEMD160 { get; set; }
        public bool SHA3_256 { get; set; }
        public bool SHA3_384 { get; set; }
        public bool SHA3_512 { get; set; }
    }
}
