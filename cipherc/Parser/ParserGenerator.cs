using cipherc.Handler;
using cipherc.Parser.Options;
using libcipherc.Crypto;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cipherc.Parser
{
    public static class ParserGenerator
    {
        static readonly MainHandler H = new MainHandler();

        public static string GetVersionText()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"cipherc {ENV.GitTag} {ENV.CompileVersion} by XUJINKAI");
            sb.AppendLine($"Compile: {ENV.GitHash}, {ENV.CompileDate}");
            sb.AppendLine($"Source: https://github.com/XUJINKAI/cipherc");
            return sb.ToString();
        }

        static string TXT_Help => @"
Load data:
data        rand        zero
";

        static Option<string> OPT_FilePath => new Option<string>("--filePath");

        static Command AddArgOpt_Input_inform_f(this Command command, string defaultForm = "hex")
        {
            command.AddArgument(new Argument<string>($"input", $"Input data, default format: {defaultForm}"));
            command.AddOption(new Option<bool>("-f", "Indicate input is file path"));
            command.AddOption(new InformOption(defaultForm));
            return command;
        }

        static Command AddOption_Out_Dump(this Command command)
        {
            command.AddOption(new OutOption());
            command.AddOption(new DumpOption());
            return command;
        }

        static Command AddArgOpt_DataGenerator(this Command command)
        {
            command.AddArgument(new Argument<int>("bytes", "Bytes number to generate"));
            command.AddOption_Out_Dump();
            return command;
        }

        public static RootCommand CreateRootCommand() =>
            new RootCommand(@"Crypto CLI Tool.")
            {
                // Data generator
                new Command("rand", "Generate random bytes").AddArgOpt_DataGenerator()
                .WithHandler(H.CreateDataIntGenerator(DATA.RAND, DumpForm.Hex)),

                new Command("zero", "Generate all zero bytes").AddArgOpt_DataGenerator()
                .WithHandler(H.CreateDataIntGenerator(DATA.ZEROS, DumpForm.Hex)),

                // Trans
                new Command("base64", "Base64 encode or decode")
                .AddArgOpt_Input_inform_f()
                .AddOptions(new Option<bool>(new string[]{"-d", "--decode" }, "Indicate decode or encode"))
                .AddOptions(new Option<bool>(new string[]{"-b", "--break-lines" }, "Break base64 string every 64 chars"))
                .AddOption_Out_Dump()
                .WithHandler(H.Base64Handler),

                // Hash
                new Command("sm3", "Hash by SM3")
                .AddArgOpt_Input_inform_f()
                .AddOption_Out_Dump()
                .WithHandler(H.CreateDataEncoder(Hash.SM3, DumpForm.Hex)),

                new Command("md5", "Hash by MD5")
                .AddArgOpt_Input_inform_f()
                .AddOption_Out_Dump()
                .WithHandler(H.CreateDataEncoder(Hash.MD5, DumpForm.Hex)),

                new Command("sm4", "SM4 encrypt or decrypt data")
                {
                    new Option<string>(new string[] {"-K", "--key"}, "Sym Key: Hex string"),
                    new Option<string>(new string[] {"-I", "--iv"}, "IV: Hex string"),
                    new Option<bool>(new string[] {"-d", "--dec" }, "Specify Decrypt (default: Encrypt)"),
                    new Option<string>("--algr", ()=>"sm4cbc", "Sym algorithm: sm4cbc(default), sm4ecb, aescbc, aesecb"),
                }
                .AddOption_Out_Dump()
                .WithHandler(H.Sym_SM4),
                
                // parser
                new Command("asn1", "Load ASN.1 file")
                {
                    OPT_FilePath,
                }
                .AddOption_Out_Dump()
                .WithHandler(H.Asn1FileLoader),

                new Command("file", "File info and operation")
                {
                    new Argument<string>("FilePath"),
                    new Option<bool>("--info", "Show file basic info"),
                    new Option<bool>("--sm3", "Show SM3 result"),
                    new Option<bool>("--text", "Show Text Content"),
                    new Option<bool>("--noout", "Disable --out and --outform option"),
                }
                .AddOption_Out_Dump()
                .WithHandler(H.FileOperation),

                // helper
                new Command("help", "Show command list")
                .WithHandler((IOutput output) =>
                {
                    output.WriteLine(TXT_Help);
                }),

                new Command("env", "Show application environment")
                .WithHandler((IOutput output) =>
                {
                    output.WriteLine(ENV.DumpEnv());
                }),

#if DEBUG
                new Command("d", "[DEBUG] test something")
                {
                    new Command("throw", "throw an exception")
                    .WithHandler(()=>{ throw new Exception("Exception throw by throw command."); }),

                    new Command("log", "Write all type output")
                    .WithHandler((IOutput output) =>
                    {
                        output.WriteBytes("Byte".GetBytes().ToFormBytes(DumpForm.Hex));
                        output.Write("Write-");
                        output.WriteLine("WriteLine");
                        output.ErrorLine("Log Error");
                        output.VerboseLine("Log Verbose");
                        output.DebugLine("Log Debug");
                    }),
                }
                .AddOption_Out_Dump()
                .WithHandler((IOutput output) =>
                {
                    output.WriteLine($"YES.");
                    output.WriteBytes(new FormBytes(new byte[]{ 0x61, 0x62, 0x63 }, DumpForm.Base64));
                }),
#endif
            }.AddGlobalOptions(
                new Option<bool>(new string[] { "-v", "--verbose" }, "Show verbose info"),
                new Option<bool>(new string[] { "--debug" }, "Show debug info") { IsHidden = true }
            );

        public static CommandLineBuilder CreateDefaultBuilder()
        {
            var cmd = CreateRootCommand();
            var builder = new CommandLineBuilder(cmd);
            builder
                .UseVersionOption() // --version
                .UseHelp() // -? /? -h /h --help
                           //.UseEnvironmentVariableDirective()
                           //.UseParseDirective()
                           //.UseDebugDirective()
                .UseSuggestDirective()
                .RegisterWithDotnetSuggest()
                .UseTypoCorrections()
                //.UseParseErrorReporting()
                //.UseExceptionHandler()
                //.CancelOnProcessTermination()
                ;

            builder.UseMiddleware(async (context, next) =>
            {
                try
                {
                    await next(context);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException is AbstractRunnerException inner)
                    {
                        throw inner;
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message.StartsWith("Option"))
                    {
                        throw new ParseErrorException(ex.Message, ex,
                            context.ParseResult.CommandResult.Command);
                    }
                }
            }, MiddlewareOrder.ExceptionHandler);

            builder.UseMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Tokens.Any(token => token.Value == "--version"))
                {
                    context.BindingContext.Console.Out.WriteLine(GetVersionText());
                    return;
                }

                await next(context);
            }, (MiddlewareOrder)(-1201)); // VersionOption = -1200

            builder.UseMiddleware(async (context, next) =>
            {
                var currentCommand = context.ParseResult.CommandResult.Command;

                // AddService ICommand
                context.BindingContext.AddService(typeof(ICommand), provider =>
                {
                    return currentCommand;
                });

                if (context.BindingContext.Console is not IOutput output)
                {
                    throw new NotImplementedException("BindingContext.Console cannot convert to IOutput");
                }

                // reset
                output.ByteStream = null;
                output.DumpForms.Clear();
                output.LogLevel = LogLevel.Error;

                // --verbose, --debug
                if (context.ParseResult.ValueForOption<bool>("--verbose"))
                    output.LogLevel = LogLevel.Verbose;
                if (context.ParseResult.ValueForOption<bool>("--debug"))
                    output.LogLevel = LogLevel.Debug;

                output.VerboseLine("ParseResult: " + context.ParseResult.Diagram());

                // --out
                var outResult = context.ParseResult.ValueForOption<string?>(OutOption.NAME);
                if (outResult != null)
                {
                    var stream = File.OpenWrite(outResult);
                    var writer = new BinaryWriter(stream);
                    output.ByteStream = writer;
                }
                // --dump
                var dumpFormResult = context.ParseResult.ValueForOption<string>(DumpOption.NAME);
                if (dumpFormResult is not null)
                {
                    var forms = DumpOption.ParseValue(dumpFormResult, currentCommand);
                    foreach (var form in forms)
                    {
                        output.DumpForms.Add(form);
                    }
                }
                else
                {
                    output.DumpForms.Add(null);
                }

                // AddService IOutput
                context.BindingContext.AddService(typeof(IOutput), provider =>
                {
                    return output;
                });

                await next(context);

                output.Close();
            }, MiddlewareOrder.Configuration);

            builder.UseMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Errors.Count > 0)
                {
                    var errText = context.ParseResult.Errors.Select(err => err.Message).JoinToString("\n");
                    throw new ParseErrorException(errText, context.ParseResult.CommandResult.Command);
                }
                else
                {
                    await next(context);
                }
            }, MiddlewareOrder.ErrorReporting);

            return builder;
        }
    }
}
