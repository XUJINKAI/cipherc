using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace CipherTool.Interpret
{
    public class Setting
    {
        public string EndOfLine { get; set; } = Environment.NewLine;
        public TextWriter OutputStream { get; set; } = Console.Out;
        public TextWriter ErrorStream { get; set; } = Console.Error;
    }
}
