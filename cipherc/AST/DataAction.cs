using System;
using System.Collections.Generic;
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
    }
}
