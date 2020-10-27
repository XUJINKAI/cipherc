using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

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
