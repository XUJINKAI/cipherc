using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using CipherTool.Exceptions;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace CipherTool.Tokenizer
{
    public class TokenStream
    {
        private int _index;
        private readonly IList<Token> _tokens;
        public int Count => _tokens.Count;
        public bool IsEnd => _index == _tokens.Count - 1;

        public void Reset() => _index = -1;
        private bool IsValidIndex(int index) => index >= 0 && index < _tokens.Count;

        public TokenStream(string[] args)
        {
            Contract.Assert(args != null);
            _tokens = new List<Token>();
            int idx = 0;
            foreach (var arg in args)
            {
                idx++;
                _tokens.Add(new Token(arg, idx));
            }
            Reset();
        }

        // Read()

        public Token Read()
        {
            var idx = _index + 1;
            if (IsValidIndex(idx))
            {
                _index += 1;
                return _tokens[idx];
            }
            else
            {
                throw new ExpectedMoreTokenException();
            }
        }

        public TokenEnum ReadTokenEnum()
        {
            var token = Read();
            var t = token.GetTokenEnum();
            if (t == TokenEnum.Unknown)
                throw new UnexpectedTokenException(token);
            return t;
        }

        public TokenEnum ReadTokenEnum(out Token token)
        {
            token = Read();
            var t = token.GetTokenEnum();
            if (t == TokenEnum.Unknown)
                throw new UnexpectedTokenException(token);
            return t;
        }

        public TokenEnum ReadTokenEnum(TokenType tokenType)
        {
            var token = Read();
            var t = token.GetTokenEnum();
            if (t == TokenEnum.Unknown || !t.IsMatchTokenType(tokenType))
                throw new UnexpectedTokenException(token);
            return t;
        }

        public int ReadInt()
        {
            var token = Read();
            if (int.TryParse(token.Text, out var result))
            {
                return result;
            }
            throw new UnexpectedTokenException(token, "expect int value here");
        }

        public string ReadText()
        {
            var token = Read();
            return token.Text;
        }

        // Peek

        public Token? PeekToken(int offset = 1)
        {
            var idx = _index + offset;
            if (IsValidIndex(idx))
            {
                return _tokens[idx];
            }
            else
            {
                return null;
            }
        }

        public bool Peek(int offset, params TokenEnum[] keywords)
        {
            var @enum = PeekToken(offset)?.GetTokenEnum();
            return @enum != null && keywords.Any(x => x == @enum);
        }

        public bool Peek(params TokenEnum[] keywords) => Peek(1, keywords);

        public bool Peek(int offset, params TokenType[] types)
        {
            var @enum = PeekToken(offset)?.GetTokenEnum();
            return @enum != null && types.Any(@type => @enum.Value.IsMatchTokenType(@type));
        }

        public bool Peek(params TokenType[] types) => Peek(1, types);

        // Accept

        /// <summary>
        /// true if next token is predicate, else false
        /// </summary>
        /// <param name="tokenEnum"></param>
        /// <returns></returns>
        public bool Accept(TokenEnum tokenEnum)
        {
            if (Peek(tokenEnum))
            {
                _ = Read();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
