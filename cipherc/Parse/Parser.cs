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
    public class Parser
    {
        public ParseSetting Setting { get; private set; }

        private readonly IList<Token> Tokens;

        private int _index = 0;
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

        private bool TryMoveNext()
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

        private Parser(string[] args, ParseSetting setting)
        {
            Tokens = new List<Token>();
            foreach (var arg in args)
            {
                Tokens.Add(new Token(arg));
            }
            Setting = setting;
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

        private Block StartParse()
        {
            Index = 0;
            var block = new Block();
            do
            {
                var token = Tokens[Index];
                var exp = token.MakeExpression(null);
                exp.ContinueParse(this);
                block.AddExpression(exp);
            } while (TryMoveNext());
            return block;
        }

        public IExpression PopExpression<T>(IExpression? parent) where T : IExpression
        {
            var token = PopToken();
            var exp = token.MakeExpression(parent);
            exp.ContinueParse(this);
            return exp;
        }

        public static Block Parse(string[] args, ParseSetting? setting = null)
        {
            Contract.Assert(args != null);
            Contract.Assert(args.Length > 0);
            return new Parser(args, setting ?? new ParseSetting()).StartParse();
        }
    }
}
