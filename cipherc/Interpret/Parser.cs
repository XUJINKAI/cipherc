using System;
using System.Diagnostics.Contracts;
using CipherTool.AST;
using CipherTool.Exceptions;
using CipherTool.Tokenizer;

namespace CipherTool.Interpret
{
    public class Parser
    {
        private TokenStream Tokens { get; }

        public Parser(TokenStream tokenStream)
        {
            Contract.Assert(tokenStream != null);
            Tokens = tokenStream;
        }

        public Node BuildAst()
        {
            Tokens.Reset();
            return DataExpression();
        }

        private Node Block()
        {
            var sentence = Sentence();
            var block = new Block(sentence);
            while (Tokens.Accept(TokenType.SentenceEnd))
            {
                block.AddNode(Sentence());
            }

            return block.Sentences.Count == 1 ? block.Sentences[0] : block;
        }

        private Node Sentence()
        {
            return Expression();
        }

        private Node Expression()
        {
            return DataExpression();
        }

        private DataNode DataExpression()
        {
            var result = DataTerm();
            while (Tokens.Accept(TokenType.ConcatData))
            {
                result = new DataConcator(result, DataTerm());
            }

            return result;
        }

        private DataNode DataTerm()
        {
            DataNode result = PostfixData();
            while (Tokens.Accept(TokenType.RepeatData))
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
            return Tokens.PeekType(TokenType.Encode, TokenType.Decode, TokenType.Sub, TokenType.Print)
                   || Tokens.PeekType<HashAlgr>();
        }

        private DataOperator DataOperator()
        {
            var token = Tokens.Read();
            var type = token.GetTokenType();
            switch (type)
            {
                case TokenType.Encode:
                    return new EncodeOperator(Tokens.ReadEnum<EncodeFormat>());
                case TokenType.Decode:
                    return new DecodeOperator(Tokens.ReadEnum<DecodeFormat>());
                case TokenType.Sub:
                    return new SubOperator(Tokens.ReadInt(), Tokens.ReadInt());
                case TokenType.Print:
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

        private HashOperator HashOperator()
        {
            return new HashOperator(Tokens.ReadEnum<HashAlgr>());
        }

        private DataNode DataPrimary()
        {
            var source = Tokens.ReadEnum<DataSource>();
            var text = "";
            if (source != DataSource.Pipe)
            {
                text = Tokens.ReadText();
            }
            return new DataPrimary(source, text);
        }
    }
}
