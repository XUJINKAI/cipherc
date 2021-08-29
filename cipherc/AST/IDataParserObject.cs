using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.AST
{
    public interface IDataParserObject
    {
        void LoadData(Data data);

        string ToDisplayString();
    }
}
