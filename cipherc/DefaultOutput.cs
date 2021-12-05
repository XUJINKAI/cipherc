using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cipherc
{
    /// <summary>
    /// CLI运行时的IOutput实现
    /// </summary>
    public class DefaultOutput : SystemConsole, IOutput
    {
        public BinaryWriter? ByteStream { get; set; }

        public IList<DumpForm?> DumpForms { get; }

        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        public DefaultOutput()
        {
            DumpForms = new List<DumpForm?>();
        }

        public void Close()
        {
            if (ByteStream == null) return;

            ByteStream.Close();
            ByteStream = null;
        }
    }
}
