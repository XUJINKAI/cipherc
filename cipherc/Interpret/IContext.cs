using System.Collections.Generic;

namespace CipherTool.Interpret
{
    public interface IContext
    {
        Setting Setting { get; }

        IDictionary<string, Data> Variables { get; }

        void WriteOutputLine(string line);

        void WriteErrorLine(string line);
    }
}
