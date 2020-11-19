using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using CipherTool.AST.Validations;
using CipherTool.Exceptions;
using CipherTool.Interpret;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public abstract class DataNode : Node
    {
        public virtual TokenEnum DefaultPrintFormat { get; } = TokenEnum.AutoPrint;
    }

    /// <summary>
    /// DataConcator -> DataRepeator { concat DataRepeator }
    /// </summary>
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

    /// <summary>
    /// DataRepeator -> DataFactor { times <N> }
    /// </summary>
    public class DataRepeator : DataNode
    {
        public DataNode DataFactor { get; set; }

        public int RepeatTimes { get; }

        public DataRepeator(DataNode node, int repeatTimes)
        {
            DataFactor = node;
            RepeatTimes = repeatTimes;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
            DataFactor.Accept(visitor);
        }
    }

    /// <summary>
    /// DataFactor -> PostfixData
    /// PostfixData -> PrefixData { DataOperator | print PrintFormat }
    /// PrefixData ->  { DataOperator } DataPrimary
    /// </summary>
    public class DataFactor : DataNode
    {
        public DataNode Data { get; }

        public DataOperator Operator { get; }

        public override TokenEnum DefaultPrintFormat => Operator switch
        {
            EncodeOperator => TokenEnum.Txt,
            DecodeOperator => TokenEnum.AutoPrint,
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

    /// <summary>
    /// DataPrimary -> [ txt | hex | base64 | file | var ] InputText | rand <NUM> | pipe
    /// </summary>
    public class DataPrimary : DataNode
    {
        [TokenTypeValidation(TokenType.DataSource)]
        public TokenEnum DataSource { get; }

        public string InputText { get; }

        public DataPrimary(TokenEnum source, string input)
        {
            DataSource = source;
            InputText = input;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RandDataPrimary : DataNode
    {
        public int RandBytes { get; }

        public RandDataPrimary(int nRandBytes)
        {
            RandBytes = nRandBytes;
        }

        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class PipeDataPrimary : DataNode
    {
        public override void Accept([NotNull] IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
