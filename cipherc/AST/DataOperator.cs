using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public abstract class DataOperator : Node
    {

    }

    public class PrintOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.PrintFormat)]
        public TokenEnum PrintFormat { get; }

        public PrintOperator(TokenEnum format)
        {
            PrintFormat = format;
        }
    }

    public class HashOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.Hash)]
        public TokenEnum HashAlgr { get; set; }

        public HashOperator(TokenEnum algr)
        {
            HashAlgr = algr;
        }
    }

    public class EncodeOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.EncodeFormat)]
        public TokenEnum EncodeFormat { get; set; }

        public EncodeOperator(TokenEnum format)
        {
            EncodeFormat = format;
        }
    }

    public class DecodeOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.DecodeFormat)]
        public TokenEnum DecodeFormat { get; set; }

        public DecodeOperator(TokenEnum format)
        {
            DecodeFormat = format;
        }
    }

    public class SubOperator : DataOperator
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public SubOperator(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
