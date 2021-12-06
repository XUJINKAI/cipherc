using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Handler
{
    public static class TEXT
    {
        public static string FILE(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
