using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public abstract class BaseUnit : IUnit
    {
        public IList<Token> Tokens { get; } = new List<Token>();

        public void AppendParseToken(Token token) => Tokens.Add(token);

    }
}
