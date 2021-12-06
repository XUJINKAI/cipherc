using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Handler
{
    /// <summary>
    /// input, --inform, -f
    /// </summary>
    public class InputDataModel
    {
        public const string ValidInforms = "hex, base64; utf8, gbk; rand, zero";

        public string Input { get; }

        public string? Inform { get; }

        public bool IsFile { get; }

        public IOutput Output { get; }

        public ICommand Command { get; }

        public InputDataModel(IOutput output, ICommand command, string input, string? inform, bool f)
        {
            output.VerboseLine($"InputDataModel: Input: {input}, Inform: {inform}, IsFile: {f}");
            Output = output; ;
            Command = command;
            Input = input;
            Inform = inform;
            IsFile = f;
        }

        public byte[] GetBytes()
        {
            byte[] result = IsFile
                ? DATA.FILE(Input)
                : Inform?.ToLower() switch
                {
                    "hex" => DATA.HEX(Input),
                    "base64" => DATA.BASE64(Input),
                    "utf8" => ENV.UTF8Encoder.Encode(Input),
                    "gbk" => ENV.GBKEncoder.Encode(Input),
                    "rand" => DATA.RAND(Input),
                    "zero" => DATA.ZEROS(Input),
                    _ => throw new ParseErrorException($"Inform not valid: {Inform}", Command),
                };
            Output.VerboseLine($"InputData[{result.Length}]: " + (result.Length > 64 ? (result.SubFirst(64).ToHexString() + "...") : result.ToHexString()));
            return result;
        }

        public string GetString()
        {
            string result = IsFile
                ? TEXT.FILE(Input)
                : Input;
            return result;
        }
    }
}
