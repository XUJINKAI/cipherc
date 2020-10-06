using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public abstract class ExpressionBase : IExpression
    {
        public IExpression? ParentExpression { get; private set; }

        public EvalFuncDelegate? EvalFunc { get; protected set; }

        public Data? EvalResult { get; private set; }

        private readonly List<IExpression> SubExpressions = new List<IExpression>();

        public virtual Data? Eval(EvalSetting setting)
        {
            Contract.Assert(EvalFunc != null);
            EvalResult = EvalFunc.Invoke(setting);
            foreach (var sub in SubExpressions)
            {
                sub.Eval(setting);
            }
            return EvalResult;
        }

        public abstract void ContinueParse(Parser parser);

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
    }
}
