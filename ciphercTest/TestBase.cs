global using cipherc;
global using cipherc.Exceptions;
global using libcipherc;
global using System;
global using Xunit;
global using Xunit.Abstractions;

using cipherc.Handler;
using cipherc.Parser;
using libcipherc.Crypto;
using System.CommandLine.Parsing;
using System.IO;
using System.Text;

namespace ciphercTest;


public class TestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    private Parser _parser;

    public TestBase(ITestOutputHelper output)
    {
        _testOutputHelper = output;
        _parser = ParserGenerator.CreateDefaultBuilder().Build();
        Environment.CurrentDirectory = Path.GetFullPath("../../../../TestFiles", AppContext.BaseDirectory);
    }


    protected void AppendLine(string line)
    {
        _testOutputHelper.WriteLine(line);
    }


    protected void MockRandGenerator(Action action)
    {
        DATA.RandGenerator = (int bytes) =>
        {
            var s = "xujinkai.cipherc.";
            var data = s.Repeat(bytes / s.Length + 1).GetBytes();
            var result = data.SubFirst(bytes);
            if (result.Length != bytes) throw new Exception();
            return result;
        };
        action();
        DATA.RandGenerator = Rand.RandomBytes;
    }

    public string EOL => "\n";

    protected static byte[] HEX(string s) => DATA.HEX(s);

    protected MemoryOutput RunCommand(string command)
    {
        var result = new MemoryOutput();
        var args = cipherc.Utils.CommandLineHelper.CommandLineToArgs(command);
        _parser.Parse(args).Invoke(result);

        AppendLine(command);
        var errmsg = result.GetErrorResult();
        if (!string.IsNullOrWhiteSpace(errmsg))
        {
            AppendLine("// Error");
            AppendLine(errmsg.TrimEnd());
        }
        var outmsg = result.GetOutResult();
        if (!string.IsNullOrWhiteSpace(outmsg))
        {
            AppendLine("// Out");
            AppendLine(outmsg.TrimEnd());
        }
        var bytes = result.GetByteResult();
        if (bytes.Length > 0)
        {
            bool isAscii = bytes.IsPrintableAscii(true);
            AppendLine("// Bytes: " + (isAscii ? "Text" : "HexDump"));
            AppendLine(isAscii ? bytes.ToAsciiString() : bytes.ToHexDumpText());
        }
        return result;
    }
}
