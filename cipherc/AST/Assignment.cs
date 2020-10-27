using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Text;

namespace CipherTool.AST
{
    public class Assignment : Node
    {
        public string VarName { get; set; }

        public DataNode Data { get; set; }

        public Assignment(string name, DataNode data)
        {
            VarName = name;
            Data = data;
        }
    }
}
