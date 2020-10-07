using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CipherTool.Parse
{
    public abstract class OutputSetting
    {
        public TextWriter OutputStream { get; set; } = Console.Out;

        public string EndOfLine { get; set; } = Environment.NewLine;

        private bool OutputFirstLine = true;

        public string AppendLine(string line)
        {
            if (OutputFirstLine)
            {
                OutputStream.Write(line);
                OutputFirstLine = false;
            }
            else
            {
                OutputStream.Write(EndOfLine);
                OutputStream.Write(line);
            }
            return line;
        }
    }
}
