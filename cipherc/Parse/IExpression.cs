using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace CipherTool.Parse
{
    public interface IExpression : IUnit, ISentenceRoot
    {
        IList<IDataPostfix> Postfixes { get; }

        void ContinueParseExpression(Parser parser, bool allowPostfixes);

        Data Eval(Parser parser);
    }
}
