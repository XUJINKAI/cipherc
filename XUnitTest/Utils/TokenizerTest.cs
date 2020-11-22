using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CipherTool.AST;
using CipherTool.Interpret;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Utils
{
    public class TokenizerTest : TestBase
    {
        public TokenizerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TokenTypeValidationAttributeTest()
        {
            var node = new PrintOperator(Tokenizer.TokenEnum.Unknown, false);
            var validator = new NodeValidator();
            node.Accept(validator);
            Assert.True(validator.ValidationResults.Count > 0);
            AppendLine(validator.ValidationResults.First()?.ErrorMessage ?? "");
        }
    }
}
