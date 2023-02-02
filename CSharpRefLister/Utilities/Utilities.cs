using System;
using System.Reflection;

using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRefLister
{
    static class Utilities
    {
        public static string GetApplicationDirectory()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            if (Directory.Exists(dir))
                return dir;
            else
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

    }
}
