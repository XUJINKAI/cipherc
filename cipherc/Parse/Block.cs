using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public class Block
    {
        private readonly List<IExpression> Expressions = new List<IExpression>();

        public void AddExpression(IExpression exp) => Expressions.Add(exp);

        public void Eval(EvalSetting? evalSetting = null)
        {
            evalSetting ??= new EvalSetting();
            foreach (var exp in Expressions)
            {
                exp.Eval(evalSetting);
            }
        }
    }
}
