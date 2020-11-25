using System;
using System.Diagnostics.Contracts;
using System.IO;
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
            if (Tokens.IsEnd)
                return block.Sentences.Count == 1 ? block.Sentences[0] : block;
            else
                throw new UnexpectedTokenException(Tokens.PeekToken() ?? throw new Exception("[Parser] Tokens.IsEnd == false, but Tokens.PeekToken() == null."));
        }

        private Node Sentence()
        {
            // vars
            if (Tokens.Accept(TokenEnum.Variables))
            {
                return new VariablesListStatement();
            }
            // help
            if (Tokens.Accept(TokenEnum.Help))
            {
                return new HelpStatement();
            }
            // Assignment
            if (Tokens.Peek(TokenEnum.Var) && Tokens.Peek(3, TokenEnum.AssignSymbol))
            {
                Tokens.Read(); // var
                var name = Tokens.ReadText();
                Tokens.Read(); // is
                var data = DataExpression();
                return new Assignment(name, data);
            }
            // Expression
            return DataExpression();
        }

        private DataNode DataExpression()
        {
            var result = DataFactor();
            while (Tokens.Accept(TokenEnum.ConcatData))
            {
                result = new DataConcator(result, DataFactor());
            }
            return result;
        }

        private DataNode DataFactor()
        {
            return PostfixData();
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
            return Tokens.Peek(TokenType.DataFunction) || Tokens.Peek(TokenType.Hash);
        }

        private DataOperator DataOperator()
        {
            var @enum = Tokens.ReadTokenEnum(out var token1);
            switch (@enum)
            {
                case TokenEnum.Encode:
                    return new EncodeOperator(Tokens.ReadTokenEnum(TokenType.EncodeFormat));
                case TokenEnum.Decode:
                    return new DecodeOperator(Tokens.ReadTokenEnum(TokenType.DecodeFormat));
                case TokenEnum.DuplicateData:
                    return new RepeatOperator(Tokens.ReadInt());
                case TokenEnum.Sub:
                    return new SubOperator(Tokens.ReadInt(), Tokens.ReadInt());
                case TokenEnum.Print:
                    return new PrintOperator(Tokens.ReadTokenEnum(TokenType.PrintFormat));
                case TokenEnum.SaveData:
                    return Tokens.ReadTokenEnum(out var token2) switch
                    {
                        TokenEnum.File => new SaveOperator(TokenEnum.File, Tokens.ReadText()),
                        TokenEnum.Var => new SaveOperator(TokenEnum.Var, Tokens.ReadText()),
                        _ => throw new UnexpectedTokenException(token2),
                    };
            }

            if (@enum.IsMatchTokenType(TokenType.Hash))
            {
                return new HashOperator(@enum);
            }
            else
            {
                throw new UnexpectedTokenException(token1);
            }
        }

        private DataNode DataPrimary()
        {
            var peekToken = Tokens.PeekToken();
            if (peekToken == null) throw new ExpectedMoreTokenException();

            if (Context.GuessInputType && peekToken.GetTokenEnum() == TokenEnum.Unknown)
            {
                var input = peekToken.Text;
                if (File.Exists(input))
                {
                    Tokens.Read();
                    return new TextDataPrimary(TokenEnum.File, input);
                }
                if (input.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                    || input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    Tokens.Read();
                    return new TextDataPrimary(TokenEnum.Url, input) { DefaultPrintFormat = TokenEnum.Txt };
                }
                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    Tokens.Read();
                    return new TextDataPrimary(TokenEnum.Hex, input.Substring(2));
                }
                if (input.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                {
                    Tokens.Read();
                    return new TextDataPrimary(TokenEnum.Bin, input.Substring(2));
                }
            }

            var source = Tokens.ReadTokenEnum(TokenType.DataSource);
            return source switch
            {
                TokenEnum.Pipe => Tokens.ReadTokenEnum(out var pipeToken) switch
                {
                    TokenEnum.Txt => new PipeDataPrimary(TokenEnum.Txt),
                    TokenEnum.File => new PipeDataPrimary(TokenEnum.File),
                    _ => throw new UnexpectedTokenException(pipeToken),
                },
                TokenEnum.Rand => new RandDataPrimary(Tokens.ReadInt()),
                _ => new TextDataPrimary(source, Tokens.ReadText()),
            };
        }
    }
}
