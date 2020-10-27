using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.AST
{
    public class Expression : Node
    {
        public Node DataExpression { get; set; }
    }

    /// <summary>
    /// DataExpression -> DataTerm { concat DataTerm }
    /// </summary>
    public class DataExpression : Node
    {
        public IList<Node> DataTerms { get; } = new List<Node>();
    }

    /// <summary>
    /// DataTerm -> DataFactor { times <N> }
    /// </summary>
    public class DataTerm : Node
    {
        public DataFactor DataFactor { get; set; }
        public IList<int> Times { get; } = new List<int>();
    }

    public class DataFactor : Node
    {
        public PostfixData PostfixData { get; set; }
    }

    public class PostfixData : Node
    {

    }

    public class PrefixData : Node
    {
        public DataPrimary DataPrimary { get; set; }

        public IList<DataOperator> Prefixes { get; } = new List<DataOperator>();
    }

    /// <summary>
    /// DataPrimary -> InputSourceDataPrimary | PipeSourceDataPrimary
    /// </summary>
    public abstract class DataPrimary : Node
    {
    }

    public class InputSourceDataPrimary : DataPrimary
    {
        public InputSource InputSource { get; set; }

        public string InputString { get; set; }

        public InputSourceDataPrimary(InputSource source, string input)
        {
            InputSource = source;
            InputString = input;
        }
    }

    public class PipeSourceDataPrimary : DataPrimary
    {

    }
}
