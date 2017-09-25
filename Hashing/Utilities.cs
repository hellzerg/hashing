using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Diagnostics;

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
                        if (Options.CurrentOptions.HashOptions.MD5)
                        {
                            using (var md5 = MD5.Create())
                            {
                                result.MD5 = BitConverter.ToString(md5.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }

                        if (Options.CurrentOptions.HashOptions.SHA1)
                        {
                            using (var sha1 = SHA1.Create())
                            {
                                result.SHA1 = BitConverter.ToString(sha1.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }

                        if (Options.CurrentOptions.HashOptions.SHA256)
                        {
                            using (var sha256 = SHA256.Create())
                            {
                                result.SHA256 = BitConverter.ToString(sha256.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }

                        if (Options.CurrentOptions.HashOptions.SHA384)
                        {
                            using (var sha384 = SHA384.Create())
                            {
                                result.SHA384 = BitConverter.ToString(sha384.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }

                        if (Options.CurrentOptions.HashOptions.SHA512)
                        {
                            using (var sha512 = SHA512.Create())
                            {
                                result.SHA512 = BitConverter.ToString(sha512.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }

                        if (Options.CurrentOptions.HashOptions.RIPEMD160)
                        {
                            using (var ripemd160 = RIPEMD160.Create())
                            {
                                result.RIPEMD160 = BitConverter.ToString(ripemd160.ComputeHash(input)).Replace("-", string.Empty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                MessageBox.Show(ex.Message, "Hash Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    text = text.Replace("SHA384", string.Empty);
                    text = text.Replace("SHA512", string.Empty);
                    text = text.Replace("RIPEMD160:", string.Empty);
                }

                Clipboard.SetText(text.Trim());
            }
            catch { }
        }

        public static void EnableHighPriority()
        {
            Process p = Process.GetCurrentProcess();
            p.PriorityClass = ProcessPriorityClass.High;
        }

        public static void DisableHighPriority()
        {
            Process p = Process.GetCurrentProcess();
            p.PriorityClass = ProcessPriorityClass.Normal;
        }
    }
}
