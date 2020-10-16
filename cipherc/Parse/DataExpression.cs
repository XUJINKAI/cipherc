using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public class DataExpression : ExpressionBase, IExpression
    {
        public DataSource? Source { get; private set; }
        public DataFormat? Format { get; private set; }
        public string? Arg { get; private set; }

        public override bool IsDataType => true;

        protected override void SelfParse(TokenStream tokenStream)
        {
            Contract.Assume(tokenStream != null);

            Token token1 = tokenStream.PopToken();
            Token token2 = tokenStream.PopToken();
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

            string arg;

            if (Source == DataSource.Pipe)
            {
                arg = Helper.GetPipeAllTextIn() ?? throw new NoPipeInputException();
            }
            else
            {
                var token3 = tokenStream.PopToken();
                arg = token3.Raw;
            }

            Arg = arg;
        }

        protected override Data? SelfEval()
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
