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

        public override void ContinueParse(TokenStream parser)
        {
            Contract.Assert(parser != null);

            var dataExp = parser.PopExpression<DataExpression>(this);
            EvalFunc = () =>
            {
                var d = dataExp.Eval();
                return HashObj.DoHash(d.Value);
            };

            ContinueProcess(parser);
        }

    }
}
