using System;
using System.Diagnostics.Contracts;
using CipherTool.AST;
using CipherTool.Exceptions;
using CipherTool.Tokenizer;

namespace CipherTool.Interpret
{
    public class Parser
    {
        private IContext Context { get; }

        private TokenStream Tokens { get; }

        public Parser(IContext context, TokenStream tokenStream)
        {
            Contract.Assert(tokenStream != null);
            Context = context;
            Tokens = tokenStream;
        }

        public Node BuildAst()
        {
            Tokens.Reset();
            return Block();
        }

        private Node Block()
        {
            var sentence = Sentence();
            var block = new Block(sentence);
            while (Tokens.Accept(TokenEnum.SentenceEnd))
            {
                block.AddNode(Sentence());
            }
            return block.Sentences.Count == 1 ? block.Sentences[0] : block;
        }

        private Node Sentence()
        {
            switch (Tokens.PeekToken()?.GetTokenEnum())
            {
                case TokenEnum.Variables:
                    Tokens.Read();
                    return new VariablesListStatement();
                case TokenEnum.Help:
                    Tokens.Read();
                    return new HelpStatement();
            }
            // Assignment
            if (Tokens.Peek(TokenEnum.Var) && Tokens.Peek(3, TokenEnum.AssignSymbol))
            {
                Tokens.Read(); // var
                var name = Tokens.ReadText();
                Tokens.Read(); // is
                var data = Expression();
                return new Assignment(name, data);
            }
            // Expression
            var exp = Expression();
            return exp;
        }

        private DataNode Expression()
        {
            return DataExpression();
        }

        private DataNode DataExpression()
        {
            var result = DataTerm();
            while (Tokens.Accept(TokenEnum.ConcatData))
            {
                result = new DataConcator(result, DataTerm());
            }

            return result;
        }

        private DataNode DataTerm()
        {
            DataNode result = PostfixData();
            while (Tokens.Accept(TokenEnum.DuplicateData))
            {
                result = new DataRepeator(result, Tokens.ReadInt());
            }

            return result;
        }

        private DataNode PostfixData()
        {
            DataNode result = PrefixData();
            while (PeekDataOperator())
            {
                var op = DataOperator();
                result = new DataFactor(result, op);
            }
            return result;
        }

        private DataNode PrefixData()
        {
            if (PeekDataOperator())
            {
                var op = DataOperator();
                var data = PrefixData();
                return new DataFactor(data, op);
            }
            else
            {
                return DataPrimary();
            }
        }

        private bool PeekDataOperator()
        {
            return Tokens.Peek(TokenEnum.Encode, TokenEnum.Decode, TokenEnum.Sub, TokenEnum.Print)
                   || Tokens.Peek(TokenType.Hash);
        }

        private DataOperator DataOperator()
        {
            var token = Tokens.Read();
            var @enum = token.GetTokenEnum();
            switch (@enum)
            {
                case TokenEnum.Encode:
                    return new EncodeOperator(Tokens.ReadTokenEnum(TokenType.EncodeFormat));
                case TokenEnum.Decode:
                    return new DecodeOperator(Tokens.ReadTokenEnum(TokenType.DecodeFormat));
                case TokenEnum.Sub:
                    return new SubOperator(Tokens.ReadInt(), Tokens.ReadInt());
                case TokenEnum.Print:
                    return new PrintOperator(Tokens.ReadTokenEnum(TokenType.PrintFormat));
            }

            if (@enum.IsMatchTokenType(TokenType.Hash))
            {
                return new HashOperator(@enum);
            }
            else
            {
                throw new UnexpectedTokenException(token);
            }
        }

        private DataNode DataPrimary()
        {
            var source = Tokens.ReadTokenEnum(TokenType.DataSource);
            return source switch
            {
                TokenEnum.Pipe => new PipeDataPrimary(),
                TokenEnum.Rand => new RandDataPrimary(Tokens.ReadInt()),
                _ => new DataPrimary(source, Tokens.ReadText()),
            };
        }
    }
}
