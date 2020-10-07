using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using CipherTool.Cli;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public class DataExpression : ExpressionBase, IExpression
    {
        public DataSource? Source { get; private set; }
        public DataFormat? Format { get; private set; }
        public string? Arg { get; private set; }

        public override bool IsDataType => true;

        public override void ContinueParse(Parser parser)
        {
            Contract.Assert(parser != null);

            Token token1 = parser.PopToken();
            Token token2 = parser.PopToken();
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
                arg = Util.GetPipeAllTextIn() ?? throw new NoPipeInputException();
            }
            else
            {
                var token3 = parser.PopToken();
                arg = token3.Raw;
            }

            Arg = arg;
            if (Source == DataSource.File && !File.Exists(arg))
            {
                throw new FileNotFoundException(arg);
            }

            EvalFunc = (Source, Format) switch
            {
                (DataSource.Arg, DataFormat.Plain) => s => Arg,
                (DataSource.Arg, DataFormat.Hex) => s => Data.FromHexString(Arg),
                (DataSource.Arg, DataFormat.Base64) => s => Data.FromBase64String(Arg),
                (DataSource.Arg, DataFormat.Bin) => s => throw new NotValidArgCombinationException(),
                (DataSource.Arg, DataFormat.Pem) => s => throw new NotValidArgCombinationException(),

                (DataSource.File, DataFormat.Plain) => s => File.ReadAllText(Arg),
                (DataSource.File, DataFormat.Hex) => s => Data.FromHexString(File.ReadAllText(Arg)),
                (DataSource.File, DataFormat.Base64) => s => Data.FromBase64String(File.ReadAllText(Arg)),
                (DataSource.File, DataFormat.Bin) => s => File.ReadAllBytes(Arg),
                (DataSource.File, DataFormat.Pem) => s => throw new NotValidArgCombinationException(),

                (DataSource.Pipe, DataFormat.Plain) => s => Arg,
                (DataSource.Pipe, DataFormat.Hex) => s => Data.FromHexString(Arg),
                (DataSource.Pipe, DataFormat.Base64) => s => Data.FromBase64String(Arg),
                (DataSource.Pipe, DataFormat.Bin) => s => throw new NotValidArgCombinationException(),
                (DataSource.Pipe, DataFormat.Pem) => s => throw new NotValidArgCombinationException(),
                _ => s => throw new NotValidArgCombinationException(),
            };

            ContinueProcess(parser);
        }

    }
}
