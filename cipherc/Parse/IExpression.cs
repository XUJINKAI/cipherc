using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace CipherTool.Parse
{
    public interface IExpression : IUnit, ISentenceRoot
    {
        Data Eval();

        IList<IDataPostfix> Postfixes { get; }

        void ContinueParseExpression(Parser parser, bool allowPostfixes);

        public static IDataPostfix GetDefaultDataPostfix()
        {
            return new DataTransformPostfix();
        }
    }

}
