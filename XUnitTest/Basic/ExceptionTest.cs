using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1.Crmf;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Basic
{
    public class ExceptionTest : TestBase
    {
        public ExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UnknownToken()
        {
            TestOutput($"unknown token", (output, error) =>
            {
                Assert.True(error.Length > 0);
            });
        }

        [Fact]
        public void ExpectedMoreToken()
        {
            TestOutput($"print hex txt", (output, error) =>
            {
                Assert.True(error.Length > 0);
            });
        }

        [Fact]
        public void NoPipeInput()
        {
            MockConsoleIn(null);
            TestOutput($"print hex pipe", (output, error) =>
            {
                Assert.True(error.Length > 0);
            });
        }

    }
}
