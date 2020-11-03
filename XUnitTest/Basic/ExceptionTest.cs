using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CipherTool.Exceptions;
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

        private void TestErrorMessage(string command, string mustInclude)
        {
            TestOutput(command, (output, error) =>
            {
                Assert.Contains(mustInclude, error);
            });
        }

        private void TestThrow<T>(string command) where T : Exception
        {
            Assert.Throws<T>(() =>
            {
                TestOutput(command, null, ctx => ctx.ThrowOnException = true);
            });
        }

        [Fact]
        public void UnknownTokenErrorMessage() => TestErrorMessage("unknown token", UnexpectedTokenException.FragmentMessage);

        [Fact]
        public void UnknownTokenThrow() => TestThrow<UnexpectedTokenException>("unknown token");

        [Fact]
        public void ExpectedMoreTokenErrorMessage() => TestErrorMessage("print hex txt", ExpectedMoreTokenException.FragmentMessage);

        [Fact]
        public void ExpectedMoreTokenThrow() => TestThrow<ExpectedMoreTokenException>("print hex txt");

        [Fact]
        public void NoPipeInputErrorMessage()
        {
            MockConsoleIn(null);
            TestErrorMessage("print hex pipe", NoPipeInputException.FragmentMessage);
        }

        [Fact]
        public void NoPipeInputThrow()
        {
            MockConsoleIn(null);
            TestThrow<NoPipeInputException>("print hex pipe");
        }

        [Fact]
        public void NotNumberErrorMessage() => TestErrorMessage("hex 1234 repeat x", UnexpectedTokenException.FragmentMessage);

        [Fact]
        public void NotNumberThrow() => TestThrow<UnexpectedTokenException>("hex 1234 repeat x");

        [Fact]
        public void NotNumberRandErrorMessage() => TestErrorMessage("rand x", UnexpectedTokenException.FragmentMessage);

        [Fact]
        public void NotNumberRandThrow() => TestThrow<UnexpectedTokenException>("rand x");

    }
}
