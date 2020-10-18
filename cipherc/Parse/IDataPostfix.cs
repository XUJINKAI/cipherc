using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface IDataPostfix : IUnit
    {
        void ContinueParse(Parser parser);

        void Eval(IExpression parent);
    }
}
