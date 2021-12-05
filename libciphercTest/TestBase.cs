global using libcipherc;
global using Xunit;
global using Xunit.Abstractions;
global using System;

using libcipherc.Crypto;
using System.IO;

namespace libciphercTest;

public delegate void TestOutputDelegate(string[] output, string error);

public class TestBase
{
    private readonly ITestOutputHelper _testOutputHelper;


    public TestBase(ITestOutputHelper output)
    {
        _testOutputHelper = output;
    }

    protected void OutputLine(string line)
    {
        _testOutputHelper.WriteLine(line);
    }


    protected Bytes LoadTestFile(string fileName)
    {
        var dir = Path.GetRelativePath(Environment.CurrentDirectory, "../../../../");
        var path = Path.Combine(dir, "TestFiles", fileName);
        return File.ReadAllBytes(path);
    }

    protected static byte[] GetRandom(int bytes)
    {
        return Rand.RandomBytes(bytes);
    }

    protected static byte[] ZERO(int bytes)
    {
        return new byte[bytes];
    }

    protected static byte[] HEX(string s)
    {
        return BytesExtension.FromHexStringToByteArray(s.Replace(" ", ""));
    }

    protected static byte[] ASCII(string s)
    {
        return s.GetBytes("ASCII");
    }
}
