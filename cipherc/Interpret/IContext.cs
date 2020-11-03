using System.Collections.Generic;
using CipherTool.AST;

namespace CipherTool.Interpret
{
    public interface IContext
    {
        IDictionary<string, Data> Variables { get; }

        bool ThrowOnException { get; }

        void WriteOutputLine(string line);

        void WriteErrorLine(string line);

        PrintOperator GetDefaultPrintOperator();

        string ListVariables();
    }
}
