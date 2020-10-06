using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public class TransformExpression : ExpressionBase, IExpression
    {
        public DataSource? Source { get; private set; }
        public DataFormat? Format { get; private set; }
        public string? Arg { get; private set; }

        public override void ContinueParse(Parser parser)
        {
            Contract.Assert(parser != null);

            var token1 = parser.MoveNextTheToken();
            var token2 = parser.MoveNextTheToken();

            if (token1.EnumValue is DataSource s1 && token2.EnumValue is DataFormat f1)
            {
                Source = s1;
                Format = f1;
            }
            else if (token1.EnumValue is DataFormat f2 && token2.EnumValue is DataSource s2)
            {
                Source = s2;
                Format = f2;
            }
            else
            {
                if (token1.EnumValue is DataSource || token1.EnumValue is DataFormat)
                {
                    throw new UnexpectedTokenException(token2);
                }
                else
                {
                    throw new UnexpectedTokenException(token1);
                }
            }

            if (Source == DataSource.File)
            {
                var token3 = parser.MoveNextTheToken();
                Arg = token3.Raw;
            }

            EvalFuncDelegate MakeArgDelegate(Func<Data, string> func)
            {
                return (s) =>
                {
                    Contract.Assert(ParentExpression != null);
                    var r = ParentExpression.EvalResult;
                    s.AppendLine(func(r.Value));
                    return r;
                };
            }
            EvalFuncDelegate MakeFileDelegate(Action<Data> func)
            {
                return (s) =>
                {
                    Contract.Assert(ParentExpression != null);
                    var r = ParentExpression.EvalResult;
                    func(r.Value);
                    return r;
                };
            }

            EvalFunc = (Format, Source) switch
            {
                (DataFormat.Plain, DataSource.Arg) => MakeArgDelegate(d => d.ToAsciiString()),
                (DataFormat.Hex, DataSource.Arg) => MakeArgDelegate(d => d.ToHexString()),
                (DataFormat.Base64, DataSource.Arg) => MakeArgDelegate(d => d.ToBase64String()),

                (DataFormat.Plain, DataSource.File) => MakeFileDelegate(d => d.SaveToFile(Arg)),
                (DataFormat.Hex, DataSource.File) => MakeFileDelegate(d => File.WriteAllText(Arg, d.ToHexString())),
                (DataFormat.Base64, DataSource.File) => MakeFileDelegate(d => File.WriteAllText(Arg, d.ToBase64String())),
                _ => throw new NotValidArgCombinationException(),
            };
        }
    }
}
