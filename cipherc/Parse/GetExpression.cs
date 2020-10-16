using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public class GetExpression : ExpressionBase, IExpression
    {
        public override bool IsDataType => false;

        protected override void SelfParse(TokenStream tokenStream) => throw new NotImplementedException();
        protected override Data? SelfEval() => throw new NotImplementedException();
    }
}
