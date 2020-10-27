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
            return DataFactor();
        }

        private Node Block()
        {
            var sentence = Sentence();
            var block = new Block(sentence);
            while (Tokens.Accept(TokenType.SentenceEnd))
            {
                throw new NotImplementedException();
            }

            return block.Sentences.Count == 1 ? block.Sentences[0] : block;
        }

        private Node Sentence()
        {
            throw new NotImplementedException();
        }

        private Node DataFactor()
        {
            var token = Tokens.Pop();
            var source = token.GetTokenType();
            switch (source)
            {
                case TokenType.Txt:
                case TokenType.Hex:
                case TokenType.Base64:
                case TokenType.File:
                case TokenType.Var:
                case TokenType.Rand:
                    return new InputSourceDataPrimary((InputSource)source, Tokens.Pop().Text);
                case TokenType.Pipe:
                    return new PipeSourceDataPrimary();
                default:
                    throw new UnexpectedTokenException(token);
            }
        }
    }
}
