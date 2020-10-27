using System.Collections.Generic;

namespace CipherTool.Interpret
{
    public interface IContext
    {
        Setting Setting { get; }

        IDictionary<string, Data> Variables { get; }
    }
}
