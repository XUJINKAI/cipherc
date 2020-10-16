using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CipherTool.Parse;
using System.Linq;
using CipherTool.Exceptions;
using System.Diagnostics.Contracts;

namespace CipherTool.Parse
{
    public class TokenStream
    {
        private readonly IList<Token> Tokens;

        private int _index = -1;
        private int Index
        {
            get => _index; set
            {
                if (Tokens.Count != 0 && value >= Tokens.Count)
                {
                    throw new ExpectedMoreTokenException();
                }
                _index = value;
            }
        }

        public TokenStream(string[] args)
        {
            Contract.Assert(args != null);
            Tokens = new List<Token>();
            foreach (var arg in args)
            {
                Tokens.Add(new Token(arg));
            }
        }

        public void Reset() => Index = -1;

        public bool TryMoveNext()
        {
            if (Index + 1 >= Tokens.Count)
            {
                return false;
            }
            else
            {
                Index += 1;
                return true;
            }
        }

        public bool HasNextToken(int offset = 1)
        {
            return Index + offset < Tokens.Count;
        }

        /// <summary>
        /// NextToken(0) == CurrentToken()
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public Token NextToken(int offset = 1)
        {
            if (Index + offset < Tokens.Count)
            {
                return Tokens[Index + offset];
            }
            else
            {
                throw new ExpectedMoreTokenException();
            }
        }

        /// <summary>
        /// MoveNext then getToken
        /// </summary>
        /// <returns></returns>
        public Token PopToken()
        {
            Index++;
            return Tokens[Index];
        }

        public T PopExpression<T>(IExpression? parent) where T : IExpression
        {
            var token = PopToken();
            var exp = token.MakeExpression(parent);
            exp.Parse(this);
            if (exp is T t)
            {
                return t;
            }
            throw new UnexpectedTokenException(token);
        }
    }
}
