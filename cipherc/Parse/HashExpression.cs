using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Cipher;

namespace CipherTool.Parse
{
    public class HashExpression<T> : BaseExpression, IExpression
        where T : IHash
    {
        private IHash HashObj { get; set; }

        public IExpression? HashData { get; set; }

        public HashExpression()
        {
            HashObj = Activator.CreateInstance<T>();
        }

        protected override void SelfParse(Parser parser)
        {
            Contract.Assume(parser != null);
            HashData = parser.PopExpression();
        }
        protected override Data SelfEval(Parser parser)
        {
            Contract.Assume(HashData != null);
            var data = HashData.Eval(parser);
            return HashObj.DoHash(data);
        }
    }
}
