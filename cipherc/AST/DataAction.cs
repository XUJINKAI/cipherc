using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.AST
{
    public class PrintSupportedHashAction : Node
    {
        public DataNode Data { get; }

        public PrintSupportedHashAction(DataNode node)
        {
            Data = node;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
            Data.Accept(visitor);
        }
    }
}
