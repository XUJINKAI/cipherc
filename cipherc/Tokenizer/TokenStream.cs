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

        public T ReadEnum<T>() where T : struct, Enum
        {
            var token = Read();
            var t = token.ToEnum<T>();
            if (t.HasValue)
            {
                return t.Value;
            }
            throw new UnexpectedTokenException(token);
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

        // PeekType

        public Token? Peek(int offset = 1)
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

        public bool PeekType(params TokenType[] keywords)
        {
            var token = Peek();
            return token != null && keywords.Any(x => token.IsMatch(x));
        }

        public bool PeekType<T>() where T : struct, Enum
        {
            var token = Peek();
            return token?.ToEnum<T>() != null;
        }

        // Accept

        public bool Accept(TokenType type)
        {
            if (PeekType(type))
            {
                var token = Read();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Expect(TokenType keyword)
        {
            Token token = Read();
            if (token.IsMatch(keyword))
                return true;
            else
                throw new UnexpectedTokenException(token, $"Expected token: {keyword}, got {token.Text}");
        }

    }
}
