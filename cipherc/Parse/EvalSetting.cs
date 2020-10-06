using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CipherTool.Parse
{
    public class EvalSetting
    {
        public TextWriter OutputStream { get; set; } = Console.Out;

        public string AppendLine(string line)
        {
            OutputStream.WriteLine(line);
            return line;
        }
    }
}
