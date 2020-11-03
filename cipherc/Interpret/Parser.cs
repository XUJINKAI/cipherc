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
            switch (Tokens.Peek()?.GetTokenType())
            {
                case TokenEnum.Variables:
                    Tokens.Read();
                    return new VariablesListStatement();
                case TokenEnum.Help:
                    Tokens.Read();
                    return new HelpStatement();
            }
            var exp = Expression();
            return exp switch
            {
                DataFactor factor when factor.Operator is PrintOperator => exp,
                _ => new DataFactor(exp, Context.GetDefaultPrintOperator()),
            };
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
            return Tokens.PeekType(TokenEnum.Encode, TokenEnum.Decode, TokenEnum.Sub, TokenEnum.Print)
                   || Tokens.PeekType<HashAlgr>();
        }

        private DataOperator DataOperator()
        {
            var token = Tokens.Read();
            var type = token.GetTokenType();
            switch (type)
            {
                case TokenEnum.Encode:
                    return new EncodeOperator(Tokens.ReadEnum<EncodeFormat>());
                case TokenEnum.Decode:
                    return new DecodeOperator(Tokens.ReadEnum<DecodeFormat>());
                case TokenEnum.Sub:
                    return new SubOperator(Tokens.ReadInt(), Tokens.ReadInt());
                case TokenEnum.Print:
                    return new PrintOperator(Tokens.ReadEnum<PrintFormat>());
            }

            var algr = type.CastToEnum<HashAlgr>();
            if (algr.HasValue)
            {
                return new HashOperator(algr.Value);
            }
            else
            {
                throw new UnexpectedTokenException(token);
            }
        }

        private DataNode DataPrimary()
        {
            var source = Tokens.ReadEnum<DataSource>();
            switch (source)
            {
                case DataSource.Pipe:
                    return new PipeDataPrimary();
                case DataSource.Rand:
                    return new RandDataPrimary(Tokens.ReadInt());
                default:
                    return new DataPrimary(source, Tokens.ReadText());
            }
        }
    }
}
