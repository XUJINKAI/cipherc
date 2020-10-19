using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface IObject : IUnit, ISentenceRoot
    {
        IList<IOperator> Operators { get; }

        Data GetProperty(string Key);
        void SetProperty(string Key, Data Value);
    }
}
