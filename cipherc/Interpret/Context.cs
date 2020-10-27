using System.Collections.Generic;

namespace CipherTool.Interpret
{
    public class Context : IContext
    {
        public Setting Setting { get; }

        public IDictionary<string, Data> Variables { get; }

        public void WriteOutputLine(string line)
        {
            Setting.OutputStream.Write(line + Setting.EndOfLine);
        }

        public void WriteErrorLine(string line)
        {
            Setting.ErrorStream.Write(line + Setting.EndOfLine);
        }

        public Context() : this(new Setting())
        {

        }

        public Context(Setting setting)
        {
            Setting = setting;
            Variables = new Dictionary<string, Data>();
        }
    }
}
