using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CipherTool.AST
{
    public class HelpStatement : Node
    {
        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class VariablesListStatement : Node
    {
        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }
}
