using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CipherTool.AST;

namespace CipherTool.Interpret
{
    public class Context : IContext
    {
        public string EndOfLine { get; set; } = Environment.NewLine;
        public TextWriter OutputStream { get; set; } = Console.Out;
        public TextWriter ErrorStream { get; set; } = Console.Error;
        public PrintFormat DefaultPrintFormat { get; set; } = PrintFormat.Hex;

        public IDictionary<string, Data> Variables { get; } = new Dictionary<string, Data>();

        public bool ThrowOnException { get; set; } = false;

        public void WriteOutputLine(string line) => OutputStream.Write(line + EndOfLine);

        public void WriteErrorLine(string line) => ErrorStream.Write(line + EndOfLine);

        public PrintOperator GetDefaultPrintOperator() => new PrintOperator(DefaultPrintFormat);
    }
}
