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

        private void WriteFormatData(TokenEnum format, Data data)
        {
            switch (format)
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
                case TokenEnum.AutoPrint:
                    const int TXT_TH = 80;
                    const int LENGTH_TH = 8;
                    const int HEX_TH = TXT_TH / 2;
                    const int BIN_TH = TXT_TH / 8;
                    const int B64_TH = TXT_TH / 4 * 3;
                    const int HASH_TH = 8;

                    if (data.Length > LENGTH_TH)
                        Context.WriteOutputLine("LENGTH: " + data.Length.ToString());

                    Context.WriteOutputLine("HEX:    " + (data.Length > HEX_TH ? data.Sub(0, HEX_TH).ToHexString() + "..." : data.ToHexString()));

                    if (data.Length < BIN_TH)
                        Context.WriteOutputLine("BIN:    " + data.ToBinaryString());

                    if (data.Length < TXT_TH)
                    {
                        if (data.IsPrintable())
                            Context.WriteOutputLine("ASCII:  " + data.ToAsciiString());
                        Context.WriteOutputLine("UTF8:   " + data.ToUtf8String());
                    }

                    if (data.Length < B64_TH)
                        Context.WriteOutputLine("BASE64: " + data.ToBase64String());

                    if (data.Length > HASH_TH)
                    {
                        Context.WriteOutputLine("MD5:    " + Hash.MD5(data).ToHexString());
                        Context.WriteOutputLine("SM3:    " + Hash.SM3(data).ToHexString());
                    }
                    break;
                default:
                    throw new ArgumentException($"Parameter format should be PrintFormat.", nameof(format));
            }
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
                    var data = Evaluate(dataNode);
                    if (!(dataNode is DataFactor dataFactor && dataFactor.Operator is PrintOperator))
                    {
                        WriteFormatData(dataNode.DefaultPrintFormat, data);
                    }
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
                        TokenEnum.Bin => data.ToBinaryString(),
                        TokenEnum.Url => Converter.UrlEncode(data),
                        _ => throw new NotImplementedException(),
                    };
                case DecodeOperator decodeOperator:
                    return decodeOperator.DecodeFormat switch
                    {
                        TokenEnum.Base64 => Data.FromBase64String(data.ToAsciiString()),
                        TokenEnum.Hex => Data.FromHexString(data.ToAsciiString()),
                        TokenEnum.Bin => Data.FromBinaryString(data.ToAsciiString()),
                        TokenEnum.Url => Converter.UrlDecode(data.ToAsciiString()),
                        _ => throw new NotImplementedException(),
                    };
                case SubOperator subOperator:
                    return data.Sub(subOperator.Start, subOperator.Length);
                case PrintOperator printOperator:
                    WriteFormatData(printOperator.PrintFormat, data);
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
