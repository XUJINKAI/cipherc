using System.Diagnostics.CodeAnalysis;
using CipherTool.AST.Validations;
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

        public bool PrintReadable { get; }

        public PrintOperator(TokenEnum format, bool readable)
        {
            PrintFormat = format;
            PrintReadable = readable;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class SaveOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.DataDestination)]
        public TokenEnum DestType { get; }

        public string DestText { get; }

        public SaveOperator(TokenEnum type, string text)
        {
            DestType = type;
            DestText = text;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class EncodeOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.EncodeFormat)]
        public TokenEnum EncodeFormat { get; set; }

        public EncodeOperator(TokenEnum format)
        {
            EncodeFormat = format;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class DecodeOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.DecodeFormat)]
        public TokenEnum DecodeFormat { get; set; }

        public DecodeOperator(TokenEnum format)
        {
            DecodeFormat = format;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class HashOperator : DataOperator
    {
        [TokenTypeValidation(TokenType.Hash)]
        public TokenEnum HashAlgr { get; set; }

        public HashOperator(TokenEnum algr)
        {
            HashAlgr = algr;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class RepeatOperator : DataOperator
    {
        public int Times { get; set; }

        public RepeatOperator(int n)
        {
            Times = n;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
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

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }
}
