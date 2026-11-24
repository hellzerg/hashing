using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Hashing
{
    internal static class EmbeddedAssembly
    {
        private static readonly Dictionary<string, Assembly> dic = new Dictionary<string, Assembly>();

        public static void Load(string embeddedResource, string fileName)
        {
            byte[] ba = null;
            Assembly asm = null;
            var curAsm = Assembly.GetExecutingAssembly();

            using (var stm = curAsm.GetManifestResourceStream(embeddedResource))
            {
                if (stm == null)
                    throw new Exception(embeddedResource + " is not found in Embedded Resources.");

                ba = new byte[(int)stm.Length];
                stm.Read(ba, 0, (int)stm.Length);
                try
                {
                    asm = Assembly.Load(ba);

                    dic.Add(asm.FullName, asm);
                    return;
                }
                catch { }
            }

            var fileOk = false;
            var tempFile = "";

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                var fileHash = BitConverter.ToString(sha1.ComputeHash(ba)).Replace("-", string.Empty);
                ;

                tempFile = Path.GetTempPath() + fileName;

                if (File.Exists(tempFile))
                {
                    var bb = File.ReadAllBytes(tempFile);
                    var fileHash2 = BitConverter.ToString(sha1.ComputeHash(bb)).Replace("-", string.Empty);

                    if (fileHash == fileHash2)
                    {
                        fileOk = true;
                    }
                }
                else
                {
                    fileOk = false;
                }
            }

            if (!fileOk)
            {
                File.WriteAllBytes(tempFile, ba);
            }

            asm = Assembly.LoadFile(tempFile);

            dic.Add(asm.FullName, asm);
        }

        public static Assembly Get(string assemblyFullName)
        {
            Assembly assembly;
            return dic.TryGetValue(assemblyFullName, out assembly) ? assembly : null;
        }
    }
}
