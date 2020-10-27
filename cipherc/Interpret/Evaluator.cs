using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using CipherTool.AST;
using CipherTool.Exceptions;

namespace CipherTool.Interpret
{
    public class Evaluator : IVisitor
    {
        public IContext Context { get; }

        public Evaluator(IContext context)
        {
            Context = context;
        }

        public void Visit(Node node)
        {
            Contract.Assume(node != null);
            switch (node)
            {
                case Block block:
                    foreach (var sentence in block.Sentences)
                    {
                        Evaluate(sentence);
                    }
                    return;
                case DataExpression dataExpression:
                    var data = Evaluate(dataExpression);
                    Context.Setting.OutputDataLine(data.ToHexString());
                    return;
                case InputSourceDataPrimary inputSourceDataPrimary:
                    Context.Setting.OutputDataLine(Evaluate(inputSourceDataPrimary).ToHexString());
                    return;
                case Assignment assignment:
                    Context.Variables.Add(assignment.VarName, Evaluate(assignment.DataExpression));
                    return;
                default:
                    throw new UnknownNodeTypeException(node.GetType());
            }
        }

        private Data Evaluate(Node node)
        {
            switch (node)
            {
                case InputSourceDataPrimary inputSourceDataPrimary:
                    return Evaluate(inputSourceDataPrimary);
                default:
                    throw new Exception();
            }
        }

        private Data Evaluate(InputSourceDataPrimary node)
        {
            return (node.InputSource) switch
            {
                (InputSource.Txt) => node.InputString,
                (InputSource.Hex) => Data.FromHexString(node.InputString),
                (InputSource.Base64) => Data.FromBase64String(node.InputString),
                (InputSource.File) => File.ReadAllBytes(node.InputString),
                (InputSource.Rand) => Cipher.Random.RandomBytes(int.Parse(node.InputString)),
                (InputSource.Var) => throw new NotImplementedException(),
                _ => throw new GeneralException(),
            };
        }
    }
}
