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

        public EvalFuncDelegate? EvalFunc { get; protected set; }

        public Data? EvalResult { get; private set; }

        private readonly List<IExpression> SubExpressions = new List<IExpression>();

        public virtual Data? Eval(EvalSetting setting)
        {
            Contract.Assume(setting != null);
            Contract.Assert(EvalFunc != null);
            EvalResult = EvalFunc.Invoke(setting);
            foreach (var sub in SubExpressions)
            {
                sub.Eval(setting);
            }
            if (IsDataType
                && (ParentExpression == null || !ParentExpression.IsDataType)
                && SubExpressions.Count == 0)
            {
                Contract.Assume(EvalResult != null);
                setting.AppendLine($"Output(HEX): {EvalResult.Value.ToHexString()}");
                setting.AppendLine($"Output(B64): {EvalResult.Value.ToBase64String()}");
            }
            return EvalResult;
        }

        public void SetParentExpression(IExpression? parent)
        {
            ParentExpression = ParentExpression == null
                ? parent
                : throw new GeneralException("Property is already set.");
        }

        protected void AddSubExpression(IExpression expression)
        {
            SubExpressions.Add(expression);
        }

        public abstract void ContinueParse(Parser parser);

        protected void ContinueProcess(Parser parser)
        {
            Contract.Assert(parser != null);

            if (IsDataType)
            {
                if (ParentExpression == null || !ParentExpression.IsDataType)
                {
                    while (parser.HasNextToken()
                        && parser.NextToken().ExpressionType == typeof(TransformExpression))
                    {
                        var token = parser.PopToken();
                        var exp = token.MakeExpression(this);
                        exp.ContinueParse(parser);
                        AddSubExpression(exp);
                    }
                }
            }
        }

    }
}
