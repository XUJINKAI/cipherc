using System.Diagnostics.CodeAnalysis;
using CipherTool.AST.Validations;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public abstract class DataNode : Node
    {
        public virtual TokenEnum DefaultPrintFormat { get; set; } = TokenEnum.AutoPrint;
    }

    public class DataConcator : DataNode
    {
        public DataNode Left { get; }
        public DataNode Right { get; }

        public DataConcator(DataNode left, DataNode right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
            Left.Accept(visitor);
            Right.Accept(visitor);
        }
    }

    public class DataFactor : DataNode
    {
        public DataNode Data { get; }

        public DataOperator Operator { get; }

        public override TokenEnum DefaultPrintFormat => Operator switch
        {
            EncodeOperator => TokenEnum.Txt,
            DecodeOperator => TokenEnum.AutoPrint,
            HashOperator => TokenEnum.Hex,
            _ => Data.DefaultPrintFormat,
        };

        public DataFactor(DataNode node, DataOperator op)
        {
            Data = node;
            Operator = op;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
            Data.Accept(visitor);
            Operator.Accept(visitor);
        }
    }

    // DataPrimary

    public class TextDataPrimary : DataNode
    {
        [TokenTypeValidation(TokenType.DataSource)]
        public TokenEnum DataSource { get; }

        public string InputText { get; }

        public TextDataPrimary(TokenEnum source, string input)
        {
            DataSource = source;
            InputText = input;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class RandDataPrimary : DataNode
    {
        public int RandBytes { get; }

        public RandDataPrimary(int nRandBytes)
        {
            RandBytes = nRandBytes;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class PipeDataPrimary : DataNode
    {
        public TokenEnum PipeFormat { get; }

        public PipeDataPrimary(TokenEnum format)
        {
            PipeFormat = format;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }
}
