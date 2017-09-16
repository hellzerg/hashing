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
        public string RIPEMD160 { get; set; }
    }
}
