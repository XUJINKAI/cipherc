using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface IDataPostfix : IUnit
    {
        void ContinueParse(Parser parser);

        void Eval(Parser parser, IExpression parent);
    }
}
