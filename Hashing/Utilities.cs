using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Hashing
{
    public static class Utilities
    {
        public static SumResult CalculateSums(string file)
        {
            SumResult result = new SumResult();
            
            try
            {
                if (File.Exists(file))
                {
                    result.File = file;

                    using (Stream input = File.OpenRead(file))
                    {
                        using (var md5 = MD5.Create())
                        {
                            result.MD5 = BitConverter.ToString(md5.ComputeHash(input)).Replace("-", "");
                        }

                        using (var sha1 = SHA1.Create())
                        {
                            result.SHA1 = BitConverter.ToString(sha1.ComputeHash(input)).Replace("-", "");
                        }

                        using (var sha256 = SHA256.Create())
                        {
                            result.SHA256 = BitConverter.ToString(sha256.ComputeHash(input)).Replace("-", "");
                        }

                        using (var ripemd160 = RIPEMD160.Create())
                        {
                            result.RIPEMD160 = BitConverter.ToString(ripemd160.ComputeHash(input)).Replace("-", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                MessageBox.Show(ex.Message, "Calculate MD5", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return result;
        }

        public static void CopyToClipboard(string text, bool all = false)
        {
            try
            {
                if (!all)
                {
                    text = text.Replace("MD5:", string.Empty);
                    text = text.Replace("SHA1:", string.Empty);
                    text = text.Replace("SHA256:", string.Empty);
                    text = text.Replace("RIPEMD160:", string.Empty);
                }

                Clipboard.SetText(text.Trim());
            }
            catch { }
        }
    }
}
