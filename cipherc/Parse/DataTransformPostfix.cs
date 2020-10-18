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
    public class DataTransformPostfix : BaseUnit, IDataPostfix
    {
        public DataSource? Source { get; private set; }
        public DataFormat? Format { get; private set; }
        public string? Arg { get; private set; }


        public DataTransformPostfix() { }

        public DataTransformPostfix(DataFormat format, DataSource source, string? arg = null)
        {
            Format = format;
            Source = source;
            Arg = arg;
        }

        public void ContinueParse(Parser parser)
        {
            Contract.Assert(parser != null);

            var enum1 = parser.PopEnum(out var token1);
            var enum2 = parser.PopEnum(out var token2);

            if (enum1 is DataSource s1 && enum2 is DataFormat f1)
            {
                Source = s1;
                Format = f1;
            }
            else if (enum1 is DataFormat f2 && enum2 is DataSource s2)
            {
                Source = s2;
                Format = f2;
            }
            else
            {
                if (enum1 is DataSource || enum1 is DataFormat)
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
                Arg = parser.PopString();
            }
        }

        public void Eval(IExpression parent)
        {
            Contract.Assume(parent != null);
            Data? MakeArgDelegate(Func<Data, string> func)
            {
                var data = parent.Eval();
                Log.OutputDataLine(func(data));
                return data;
            }
            Data? MakeFileDelegate(Action<Data> func)
            {
                var data = parent.Eval();
                func(data);
                return null;
            }

            _ = ((Format, Source) switch
            {
                (DataFormat.Plain, DataSource.Arg) => MakeArgDelegate(d => d.ToAsciiString()),
                (DataFormat.Hex, DataSource.Arg) => MakeArgDelegate(d => d.ToHexString()),
                (DataFormat.Base64, DataSource.Arg) => MakeArgDelegate(d => d.ToBase64String()),

                (DataFormat.Plain, DataSource.File) => MakeFileDelegate(d => d.SaveToFile(Arg)),
                (DataFormat.Hex, DataSource.File) => MakeFileDelegate(d => File.WriteAllText(Arg, d.ToHexString())),
                (DataFormat.Base64, DataSource.File) => MakeFileDelegate(d => File.WriteAllText(Arg, d.ToBase64String())),
                _ => throw new NotValidArgCombinationException(),
            });
        }

    }
}
