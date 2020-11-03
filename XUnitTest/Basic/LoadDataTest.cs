using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;
using CipherTool;

namespace CipherTool.Test.Basic
{
    public class LoadDataTest : TestBase
    {
        public LoadDataTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void LoadText()
        {
            var txt = GetRandom(20).ToHexString("-");
            TestOutput($"txt \"{txt}\"", (output, error) =>
            {
                Assert.Equal(txt.GetBytes().ToHexString(), output.First());
            });
        }

        [Fact]
        public void LoadHex()
        {
            var data = GetRandom(20);
            TestOutput($"hex {data.ToHexString()}", (output, error) =>
            {
                Assert.Equal(data.ToHexString(), output.First());
            });
        }

        [Fact]
        public void LoadBase64()
        {
            var data = GetRandom(20);
            TestOutput($"base64 {data.ToBase64String()}", (output, error) =>
            {
                Assert.Equal(data.ToHexString(), output.First());
            });
        }

        [Fact]
        public void LoadFile()
        {
            AppendLine(Directory.GetCurrentDirectory());
            var data = GetRandom(40);
            const string filename = "tmp";
            File.WriteAllBytes(filename, data);
            TestOutput($"file {filename}", (output, error) =>
            {
                Assert.Equal(data.ToHexString(), output.First());
            });
        }

        [Fact]
        public void LoadVar()
        {
            const string varName = "x";
            var data = GetRandom(20);
            TestOutput($"var {varName}", (output, error) =>
            {
                Assert.Equal(data.ToHexString(), output.First());
            }, ctx =>
            {
                ctx.Variables.Add(varName, data);
            });
        }

        [Fact]
        public void LoadPipeInput()
        {
            var data = GetRandom(20).ToHexString();
            MockConsoleIn(data);
            TestOutput($"pipe", (output, error) =>
            {
                Assert.Equal(data.GetBytes().ToHexString(), output.First());
            });
        }
    }
}