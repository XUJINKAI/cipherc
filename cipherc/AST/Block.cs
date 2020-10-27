using System;
using System.Collections.Generic;
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
    }
}
