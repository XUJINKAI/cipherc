using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CipherTool.Parse
{
    public class Parser
    {
        public ParseSetting Setting { get; }

        public IReadOnlyList<string> InputArgs { get; }

        public TokenStream TokenStream { get; }

        private List<IExpression> Expressions { get; }

        private Parser(string[] args, ParseSetting parseSetting)
        {
            Setting = parseSetting;
            InputArgs = new List<string>(args);
            TokenStream = new TokenStream(args);
            Expressions = new List<IExpression>();
            ParseTokensToExpressions();
        }

        private void ParseTokensToExpressions()
        {
            TokenStream.Reset();
            Expressions.Clear();
            do
            {
                var token = TokenStream.PopToken();
                var exp = token.MakeExpression(null);
                exp.Parse(TokenStream);
                Expressions.Add(exp);
            } while (TokenStream.TryMoveNext());
        }

        public void Evaluate() => Expressions.ForEach(exp => exp.Eval());

        public static Parser Eval(string[] args, ParseSetting? setting = null)
        {
            Contract.Assert(args != null);
            Contract.Assert(args.Length > 0);

            setting ??= new ParseSetting();
            var parser = new Parser(args, setting);
            parser.Evaluate();
            return parser;
        }
    }
}
