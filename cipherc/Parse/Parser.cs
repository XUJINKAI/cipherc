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

        private Parser(string[] args, ParseSetting setting)
        {
            Tokens = new List<Token>();
            foreach (var arg in args)
            {
                Tokens.Add(new Token(arg));
            }
            Setting = setting;
        }

        public Token CurrentToken()
        {
            return Tokens[Index];
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
        /// MoveNext(1) => Next
        /// </summary>
        /// <param name="offset"></param>
        public void MoveNext(int offset = 1)
        {
            Index += offset;
        }

        /// <summary>
        /// MoveNext() and CurrentToken()
        /// </summary>
        /// <returns></returns>
        public Token MoveNextTheToken()
        {
            Index++;
            return Tokens[Index];
        }

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

        private void Reset()
        {
            Index = 0;
        }

        private Block RunParse()
        {
            Reset();
            var block = new Block();
            do
            {
                var token = CurrentToken();
                var exp = token.MakeExpression(null);
                exp.ContinueParse(this);
                block.AddExpression(exp);
            } while (TryMoveNext());
            return block;
        }

        public static Block Parse(string[] args, ParseSetting? setting = null)
        {
            Contract.Assert(args != null);
            Contract.Assert(args.Length > 0);
            return new Parser(args, setting ?? new ParseSetting()).RunParse();
        }
    }
}
