using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public class DataExpression : BaseExpression, IExpression
    {
        public DataSource? Source { get; private set; }
        public DataFormat? Format { get; private set; }
        public string? Arg { get; private set; }

        protected override void SelfParse(Parser parser)
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

            if (Source == DataSource.Pipe)
            {
                Arg = Helper.GetPipeAllTextIn() ?? throw new NoPipeInputException();
            }
            else
            {
                Arg = parser.PopString();
            }
        }

        protected override Data SelfEval(Parser parser)
        {
            if (Source == DataSource.File && !File.Exists(Arg))
            {
                throw new FileNotFoundException(Arg);
            }

            return (Source, Format) switch
            {
                (DataSource.Arg, DataFormat.Plain) => Arg,
                (DataSource.Arg, DataFormat.Hex) => Data.FromHexString(Arg),
                (DataSource.Arg, DataFormat.Base64) => Data.FromBase64String(Arg),
                (DataSource.Arg, DataFormat.Bin) => throw new NotValidArgCombinationException(),
                (DataSource.Arg, DataFormat.Pem) => throw new NotValidArgCombinationException(),

                (DataSource.File, DataFormat.Plain) => File.ReadAllText(Arg),
                (DataSource.File, DataFormat.Hex) => Data.FromHexString(File.ReadAllText(Arg)),
                (DataSource.File, DataFormat.Base64) => Data.FromBase64String(File.ReadAllText(Arg)),
                (DataSource.File, DataFormat.Bin) => File.ReadAllBytes(Arg),
                (DataSource.File, DataFormat.Pem) => throw new NotValidArgCombinationException(),

                (DataSource.Pipe, DataFormat.Plain) => Arg,
                (DataSource.Pipe, DataFormat.Hex) => Data.FromHexString(Arg),
                (DataSource.Pipe, DataFormat.Base64) => Data.FromBase64String(Arg),
                (DataSource.Pipe, DataFormat.Bin) => throw new NotValidArgCombinationException(),
                (DataSource.Pipe, DataFormat.Pem) => throw new NotValidArgCombinationException(),
                _ => throw new NotValidArgCombinationException(),
            };
        }
    }
}
