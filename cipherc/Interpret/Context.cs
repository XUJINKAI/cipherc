using System.Collections.Generic;

namespace CipherTool.Interpret
{
    public class Context : IContext
    {
        public Setting Setting { get; }

        public IDictionary<string, Data> Variables { get; }

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
