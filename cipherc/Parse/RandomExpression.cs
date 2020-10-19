using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace CipherTool.Parse
{
    public class RandomExpression : BaseExpression, IExpression
    {
        public int Bits { get; private set; }

        protected override void SelfParse(Parser parser)
        {
            Contract.Assume(parser != null);
            var s = parser.PopString();
            Bits = int.Parse(s);
        }
        protected override Data SelfEval(Parser parser)
        {
            return Cipher.Random.RandomBytes(Bits);
        }
    }
}
