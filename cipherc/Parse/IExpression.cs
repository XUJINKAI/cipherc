using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CipherTool.Parse
{
    public interface IExpression
    {
        /// <summary>
        /// true: Eval() is not null
        /// </summary>
        bool IsDataType { get; }
        IExpression? ParentExpression { get; }

        void SetParentExpression(IExpression? parent);

        /// <summary>
        /// 开始于NextToken(), 结束于CurrentToken()，即结束时不执行MoveNext()
        /// </summary>
        /// <param name="tokenStream"></param>
        void Parse(TokenStream tokenStream);

        Data? Eval();
    }

}
