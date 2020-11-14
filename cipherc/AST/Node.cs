using System.Diagnostics.Contracts;

namespace CipherTool.AST
{
    public abstract class Node
    {
        public virtual void Accept(IVisitor visitor)
        {
            Contract.Assume(visitor != null);
            visitor.Visit(this);
        }
    }
}
