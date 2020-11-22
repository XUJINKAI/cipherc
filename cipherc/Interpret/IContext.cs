using System.Collections.Generic;
using CipherTool.AST;

namespace CipherTool.Interpret
{
    public interface IContext
    {
        IDictionary<string, Data> Variables { get; }

        public string EndOfLine { get; }

        bool GuessInputType { get; }

        bool ThrowOnException { get; }

        void WriteOutputLine(string line);

        void WriteErrorLine(string line);
    }
}
