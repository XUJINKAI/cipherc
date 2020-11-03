using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.Exceptions
{
    public class UnexpectedTokenException : GeneralException
    {
        public const string FragmentMessage = "Unexpected token";

        private readonly string _message;

        public override string Message
            => $"{FragmentMessage}: {Token.Text}{(string.IsNullOrEmpty(_message) ? "" : $", {_message}")}";

        public Token Token { get; private set; }

        public UnexpectedTokenException(Token token)
        {
            Contract.Assume(token != null);
            _message = "";
            Token = token;
        }

        public UnexpectedTokenException(Token token, string message)
        {
            _message = message;
            Token = token;
        }
    }
}
