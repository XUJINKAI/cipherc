using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CipherTool.AST;
using CipherTool.Cipher;
using CipherTool.Cli;
using CipherTool.Exceptions;
using CipherTool.Tokenizer;
using CipherTool.Utils;

namespace CipherTool.Interpret
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class Evaluator : IVisitor
    {
        public IContext Context { get; }

        public Evaluator(IContext context)
        {
            Context = context;
        }

        public static void WriteVariablesList(IContext context)
        {
            Contract.Assume(context != null);
            var Variables = context.Variables;
            var EndOfLine = context.EndOfLine;

            if (Variables.Count == 0)
            {
                return;
            }

            var result = string.Join(EndOfLine, Variables.Select(item => $"{item.Key} = {item.Value.ToHexString()}"));
            context.WriteOutputLine(result);
        }

        public void Visit(Node node)
        {
            Contract.Assume(node != null);
            switch (node)
            {
                case Block block:
                    foreach (var sentence in block.Sentences)
                    {
                        Visit(sentence);
                    }
                    return;
                case HelpStatement:
                    Context.WriteOutputLine(HelpMenu.GetHelpText());
                    return;
                case VariablesListStatement:
                    WriteVariablesList(Context);
                    return;
                case DataNode dataNode:
                    Evaluate(dataNode);
                    return;
                case Assignment assignment:
                    Context.Variables[assignment.VarName] = Evaluate(assignment.Data);
                    return;
                default:
                    throw new UnknownNodeTypeException(node.GetType());
            }
        }

        private Data Evaluate(DataNode node)
        {
            return node switch
            {
                DataConcator dataConcator => Evaluate(dataConcator.Left).Concat(Evaluate(dataConcator.Right)),
                DataRepeator dataRepeator => Evaluate(dataRepeator.DataFactor).Repeat(dataRepeator.RepeatTimes),
                DataFactor dataFactor => Evaluate(dataFactor),
                DataPrimary dataPrimary => Evaluate(dataPrimary),
                RandDataPrimary randDataPrimary => Evaluate(randDataPrimary),
                PipeDataPrimary pipeDataPrimary => Evaluate(pipeDataPrimary),
                _ => throw new Exception(),
            };
        }

        private Data Evaluate(DataFactor factor)
        {
            var data = Evaluate(factor.Data);
            switch (factor.Operator)
            {
                case HashOperator hashOperator:
                    return hashOperator.HashAlgr switch
                    {
                        TokenEnum.Sm3 => Hash.SM3(data),
                        TokenEnum.Md5 => Hash.MD5(data),
                        TokenEnum.Sha1 => Hash.SHA1(data),
                        TokenEnum.Sha256 => Hash.SHA256(data),
                        TokenEnum.Sha384 => Hash.SHA384(data),
                        TokenEnum.Sha512 => Hash.SHA512(data),
                        TokenEnum.Sha3 => Hash.SHA3(data),
                        _ => throw new NotImplementedException(),
                    };
                case EncodeOperator encodeOperator:
                    return encodeOperator.EncodeFormat switch
                    {
                        TokenEnum.Base64 => data.ToBase64String(),
                        TokenEnum.Hex => data.ToHexString(),
                        TokenEnum.Url => Converter.UrlEncode(data),
                        _ => throw new NotImplementedException(),
                    };
                case DecodeOperator decodeOperator:
                    return decodeOperator.DecodeFormat switch
                    {
                        TokenEnum.Base64 => Data.FromBase64String(data.ToAsciiString()),
                        TokenEnum.Hex => Data.FromHexString(data.ToAsciiString()),
                        TokenEnum.Url => Converter.UrlDecode(data.ToAsciiString()),
                        _ => throw new NotImplementedException(),
                    };
                case SubOperator subOperator:
                    return data.Sub(subOperator.Start, subOperator.Length);
                case PrintOperator printOperator:
                    switch (printOperator.PrintFormat)
                    {
                        case TokenEnum.Txt:
                            Context.WriteOutputLine(data.ToUtf8String());
                            break;
                        case TokenEnum.Hex:
                            Context.WriteOutputLine(data.ToHexString());
                            break;
                        case TokenEnum.Bin:
                            Context.WriteOutputLine(data.ToBinaryString());
                            break;
                        case TokenEnum.Base64:
                            Context.WriteOutputLine(data.ToBase64String());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return data;
                default:
                    throw new NotImplementedException();
            }
        }

        private Data Evaluate(DataPrimary node)
        {
            return (node.DataSource) switch
            {
                (TokenEnum.Txt) => Data.FromUtf8String(node.InputText),
                (TokenEnum.Hex) => Data.FromHexString(node.InputText),
                (TokenEnum.Bin) => Data.FromBinaryString(node.InputText),
                (TokenEnum.Base64) => Data.FromBase64String(node.InputText),
                (TokenEnum.File) => File.ReadAllBytes(node.InputText),
                (TokenEnum.Var) => Context.Variables[node.InputText],
                _ => throw new Exception(),
            };
        }

        private Data Evaluate(RandDataPrimary node)
        {
            return Cipher.Random.RandomBytes(node.RandBytes);
        }

        private Data Evaluate(PipeDataPrimary _)
        {
            return ConsoleHelper.GetPipeAllTextIn();
        }

    }
}
