using System.Collections.Generic;
using CipherTool.AST;

namespace CipherTool.Interpret
{
    public interface IContext
    {
        IDictionary<string, Data> Variables { get; }

        void WriteOutputLine(string line);

        void WriteErrorLine(string line);

        public PrintOperator GetDefaultPrintOperator();
    }
}
