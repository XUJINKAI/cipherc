using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace CipherTool.Parse
{
    public class RandomExpression : ExpressionBase, IExpression
    {
        public override bool IsDataType => true;

        public override void ContinueParse(TokenStream parser)
        {
            Contract.Assert(parser != null);

            var token = parser.PopToken();
            int n = int.Parse(token.Raw);
            EvalFunc = () => Cipher.Random.RandomBytes(n);
        }
    }
}
