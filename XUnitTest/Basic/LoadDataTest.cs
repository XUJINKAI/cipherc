using System;
using System.Linq;
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