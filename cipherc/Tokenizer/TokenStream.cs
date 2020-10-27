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
        private readonly IList<Token> Tokens;

        private int Index { get; set; }

        public void Reset() => Index = -1;

        private bool IsValidIndex(int index) => index >= 0 && index < Tokens.Count;

        public TokenStream(string[] args)
        {
            Contract.Assert(args != null);
            Tokens = new List<Token>();
            int idx = 0;
            foreach (var arg in args)
            {
                idx++;
                Tokens.Add(new Token(arg, idx));
            }
            Reset();
        }

        public Token Pop()
        {
            var idx = Index + 1;
            if (IsValidIndex(idx))
            {
                Index += 1;
                return Tokens[idx];
            }
            else
            {
                throw new ExpectedMoreTokenException();
            }
        }

        public bool TryPop([MaybeNullWhen(false)] out Token result)
        {
            var idx = Index + 1;
            if (IsValidIndex(idx))
            {
                Index += 1;
                result = Tokens[idx];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }


        public Token Peek(int offset = 1)
        {
            var idx = Index + offset;
            if (IsValidIndex(idx))
            {
                return Tokens[idx];
            }
            else
            {
                throw new ExpectedMoreTokenException();
            }
        }

        public bool TryPeek([MaybeNullWhen(false)] out Token result, int offset = 1)
        {
            var idx = Index + offset;
            if (IsValidIndex(idx))
            {
                result = Tokens[idx];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public bool Peek(params TokenType[] Keywords)
        {
            var token = Peek();
            return Keywords.Any(x => token.IsMatch(x));
        }

        public bool Expect(TokenType keyword)
        {
            Token token = Pop();
            if (token.IsMatch(keyword))
                return true;
            else
                throw new UnexpectedTokenException(token, $"Expected token: {keyword}, got {token.Text}");
        }

        public bool Accept(TokenType keyword)
        {
            if (Peek(keyword))
            {
                var token = Pop();
                return token.IsMatch(keyword);
            }
            else
            {
                return false;
            }
        }


    }
}
