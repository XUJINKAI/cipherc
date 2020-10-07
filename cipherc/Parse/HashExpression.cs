using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Cipher;

namespace CipherTool.Parse
{
    public class HashExpression<T> : ExpressionBase, IExpression
        where T : IHash
    {
        private IHash HashObj { get; set; }

        public HashExpression()
        {
            HashObj = Activator.CreateInstance<T>();
        }

        public override bool IsDataType => true;

        public override void ContinueParse(Parser parser)
        {
            Contract.Assert(parser != null);

            var dataExp = parser.PopExpression<DataExpression>(this);
            EvalFunc = (s) =>
            {
                var d = dataExp.Eval(s);
                return HashObj.DoHash(d.Value);
            };

            ContinueProcess(parser);
        }

    }
}
