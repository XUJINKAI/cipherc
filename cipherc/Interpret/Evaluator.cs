using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using CipherTool.AST;
using CipherTool.Cipher;
using CipherTool.Exceptions;
using CipherTool.Transform;

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
                        RunSentence(sentence);
                    }
                    return;
                case DataNode dataNode:
                    Evaluate(dataNode);
                    return;
                case Assignment assignment:
                    Context.Variables.Add(assignment.VarName, Evaluate(assignment.Data));
                    return;
                default:
                    throw new UnknownNodeTypeException(node.GetType());
            }
        }

        private void RunSentence(Node sentence)
        {
            switch (sentence)
            {
                case DataNode node:
                    Evaluate(node);
                    return;
            }
        }

        private Data Evaluate(DataNode node)
        {
            switch (node)
            {
                case DataConcator dataConcator:
                    return Evaluate(dataConcator.Left).Concat(Evaluate(dataConcator.Right));
                case DataRepeator dataRepeator:
                    return Evaluate(dataRepeator.DataFactor).Repeat(dataRepeator.RepeatTimes);
                case DataFactor dataFactor:
                    return Evaluate(dataFactor);
                case DataPrimary dataPrimary:
                    return Evaluate(dataPrimary);
                default:
                    throw new Exception();
            }
        }

        private Data Evaluate(DataFactor factor)
        {
            var data = Evaluate(factor.Data);
            switch (factor.Operator)
            {
                case HashOperator hashOperator:
                    return hashOperator.HashAlgr switch
                    {
                        HashAlgr.Sm3 => Hash.SM3(data),
                        HashAlgr.Md5 => Hash.MD5(data),
                        HashAlgr.Sha1 => Hash.SHA1(data),
                        HashAlgr.Sha256 => Hash.SHA256(data),
                        _ => throw new NotImplementedException(),
                    };
                case EncodeOperator encodeOperator:
                    return encodeOperator.EncodeFormat switch
                    {
                        EncodeFormat.Base64 => data.ToBase64String(),
                        EncodeFormat.Hex => data.ToHexString(),
                        EncodeFormat.Url => HttpUtility.UrlEncode(data),
                        _ => throw new NotImplementedException(),
                    };
                case DecodeOperator decodeOperator:
                    return decodeOperator.DecodeFormat switch
                    {
                        DecodeFormat.Base64 => Data.FromBase64String(data.ToAsciiString()),
                        DecodeFormat.Hex => Data.FromHexString(data.ToAsciiString()),
                        DecodeFormat.Url => HttpUtility.UrlDecodeToBytes(data.ToAsciiString()),
                        _ => throw new NotImplementedException(),
                    };
                case SubOperator subOperator:
                    return data.Sub(subOperator.Start, subOperator.Length);
                case PrintOperator printOperator:
                    switch (printOperator.PrintFormat)
                    {
                        case PrintFormat.Txt:
                            Context.WriteOutputLine(data.ToAsciiString());
                            break;
                        case PrintFormat.Hex:
                            Context.WriteOutputLine(data.ToHexString());
                            break;
                        case PrintFormat.Base64:
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
                (DataSource.Txt) => node.InputText,
                (DataSource.Hex) => Data.FromHexString(node.InputText),
                (DataSource.Base64) => Data.FromBase64String(node.InputText),
                (DataSource.File) => File.ReadAllBytes(node.InputText),
                (DataSource.Rand) => Cipher.Random.RandomBytes(int.Parse(node.InputText)),
                (DataSource.Var) => Context.Variables[node.InputText],
                _ => throw new Exception(),
            };
        }
    }
}
