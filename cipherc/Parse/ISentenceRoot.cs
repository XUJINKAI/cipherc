using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface ISentenceRoot : IUnit
    {
        void ContinueParseWholeSentence(Parser parser);

        void Execute();
    }
}
