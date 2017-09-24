using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool RIPEMD160 { get; set; }
    }
}
