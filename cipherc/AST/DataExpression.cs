using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CipherTool.Exceptions;
using CipherTool.Interpret;

namespace CipherTool.AST
{
    public abstract class DataNode : Node
    {

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

        public DataFactor(DataNode node, DataOperator op)
        {
            Data = node;
            Operator = op;
        }
    }

    /// <summary>
    /// DataPrimary -> [ txt | hex | base64 | file | var ] InputText | rand <NUM> | pipe
    /// </summary>
    public class DataPrimary : DataNode
    {
        public DataSource DataSource { get; }

        public string InputText { get; }

        public DataPrimary(DataSource source, string input)
        {
            DataSource = source;
            InputText = input;
        }
    }

    public class RandDataPrimary : DataNode
    {
        public int RandBytes { get; }

        public RandDataPrimary(int nRandBytes)
        {
            RandBytes = nRandBytes;
        }
    }

    public class PipeDataPrimary : DataNode
    {

    }
}
