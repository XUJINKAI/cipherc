using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Parse;

namespace CipherTool.Exceptions
{
    public class UnexpectedTokenException : GeneralException
    {
        public Token Token { get; private set; }

        public UnexpectedTokenException(Token token)
        {
            Token = token;
        }

        public UnexpectedTokenException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
