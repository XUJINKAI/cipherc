using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace CipherTool.Parse
{
    public abstract class BaseObject : BaseUnit, IObject
    {
        public IList<IOperator> Operators { get; } = new List<IOperator>();

        public Data GetProperty(string Key)
        {
            var p = GetType().GetProperty(Key) ?? throw new Exception();
            var o = p.GetValue(this) ?? throw new Exception();
            return (Data)o;
        }

        public void SetProperty(string Key, Data Value)
        {
            var p = GetType().GetProperty(Key) ?? throw new Exception();
            p.SetValue(this, Value);
        }

        public void ContinueParseWholeSentence(Parser parser)
        {
            Contract.Assert(parser != null);

            SelfParse(parser);

            // TODO

            AfterParse(parser);
        }

        public void Execute(Parser parser)
        {
            foreach (var op in Operators)
            {
                op.Eval(parser);
            }
        }

        protected virtual void SelfParse(Parser parser) { }

        protected virtual void AfterParse(Parser parser) { }
    }
}
