using System.Diagnostics.CodeAnalysis;

namespace CipherTool.AST
{
    public abstract class Node
    {
        public abstract void Accept([NotNull] IVisitor visitor);
    }
}
