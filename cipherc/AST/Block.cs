using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CipherTool.AST
{
    public class Block : Node
    {
        public IList<Node> Sentences { get; }

        public Block(Node node)
        {
            Sentences = new List<Node> { node };
        }

        public void AddNode(Node node)
        {
            Sentences.Add(node);
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
            Sentences.ForEach(s => s.Accept(visitor));
        }
    }
}
