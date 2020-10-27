using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Interpret;

namespace CipherTool.AST
{
    public interface IExpression<T>
    {
        T Eval(IContext context);
    }
}
