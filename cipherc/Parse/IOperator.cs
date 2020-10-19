using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface IOperator : IUnit
    {
        IObject OperateObject { get; }

        bool HasValue { get; }

        Data? Eval(Parser parser);
    }
}
