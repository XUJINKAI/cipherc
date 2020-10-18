using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Parse
{
    public interface IUnit
    {
        IList<Token> Tokens { get; }

        void AppendParseToken(Token token);
    }
}
