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

        public DataExpression? HashData { get; set; }

        public HashExpression()
        {
            HashObj = Activator.CreateInstance<T>();
        }

        public override bool IsDataType => true;

        protected override void SelfParse(TokenStream tokenStream)
        {
            Contract.Assume(tokenStream != null);
            HashData = tokenStream.PopExpression<DataExpression>(this);
        }
        protected override Data? SelfEval()
        {
            Contract.Assume(HashData != null);
            var data = HashData.Eval();

            Contract.Assume(data != null);
            return HashObj.DoHash(data.Value);
        }
    }
}
