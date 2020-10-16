using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public abstract class ExpressionBase : IExpression
    {
        public abstract bool IsDataType { get; }

        public IExpression? ParentExpression { get; private set; }

        protected List<IExpression> SubExpressions { get; } = new List<IExpression>();
        protected List<IExpression> DataOperateExpressions { get; } = new List<IExpression>();

        private Data? EvalCache { get; set; }

        public void CheckRules()
        {
            if (IsDataType)
            {
                Contract.Assert(SubExpressions.Count == 0);

                if (ParentExpression == null)
                {
                    Contract.Assert(DataOperateExpressions.Count != 0);
                }
                else
                {
                    Contract.Assert(DataOperateExpressions.Count == 0);
                }
            }
            else
            {
                Contract.Assert(ParentExpression == null);
                Contract.Assert(SubExpressions.Count != 0);
                Contract.Assert(DataOperateExpressions.Count == 0);
            }
        }

        public void SetParentExpression(IExpression? parent)
        {
            Contract.Assert(ParentExpression == null);
            ParentExpression = parent;
        }

        public void Parse(TokenStream tokenStream)
        {
            Contract.Assert(tokenStream != null);

            SelfParse(tokenStream);

            if (IsDataType)
            {
                if (ParentExpression == null || !ParentExpression.IsDataType)
                {
                    while (tokenStream.HasNextToken()
                        && tokenStream.NextToken().ExpressionType == typeof(TransformExpression))
                    {
                        var token = tokenStream.PopToken();
                        var exp = token.MakeExpression(this);
                        exp.Parse(tokenStream);
                        DataOperateExpressions.Add(exp);
                    }
                }
            }
        }

        protected abstract void SelfParse(TokenStream tokenStream);

        public Data? Eval()
        {
            if (EvalCache != null)
            {
                return EvalCache;

            }
            EvalCache = SelfEval();

            foreach (var sub in DataOperateExpressions)
            {
                sub.Eval();
            }
            if (IsDataType
                && (ParentExpression == null || !ParentExpression.IsDataType)
                && DataOperateExpressions.Count == 0)
            {
                Contract.Assume(EvalCache != null);
                Log.OutputDataLine($"Output(HEX): {EvalCache.Value.ToHexString()}");
                Log.OutputDataLine($"Output(B64): {EvalCache.Value.ToBase64String()}");
            }
            return EvalCache;
        }

        protected abstract Data? SelfEval();
    }
}
