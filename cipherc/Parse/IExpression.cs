using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CipherTool.Parse
{
    public delegate Data? EvalFuncDelegate(EvalSetting setting);

    public interface IExpression
    {
        /// <summary>
        /// true: Eval() is not null
        /// </summary>
        bool IsDataType { get; }
        IExpression? ParentExpression { get; }

        EvalFuncDelegate? EvalFunc { get; }
        Data? EvalResult { get; }

        Data? Eval(EvalSetting setting);

        /// <summary>
        /// 开始于NextToken(), 结束于CurrentToken()，即结束时不执行MoveNext()
        /// </summary>
        /// <param name="parser"></param>
        void ContinueParse(Parser parser);
        void SetParentExpression(IExpression? parent);
    }

}
