using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace CipherTool.Parse
{
    public class RandomExpression : ExpressionBase, IExpression
    {
        public override bool IsDataType => true;

        public int Bits { get; private set; }

        protected override void SelfParse(TokenStream tokenStream)
        {
            Contract.Assume(tokenStream != null);
            var token = tokenStream.PopToken();
            Bits = int.Parse(token.Raw);
        }
        protected override Data? SelfEval()
        {
            return Cipher.Random.RandomBytes(Bits);
        }
    }
}
