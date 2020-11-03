using System.Collections.Generic;
using System.Text;
using CipherTool.AST;
using CipherTool.Tokenizer;
using Xunit;
using Xunit.Abstractions;

namespace CipherTool.Test.Utils
{
    public class EnumsTest : TestBase
    {
        public EnumsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EnumCast()
        {
            Assert.True(TokenEnum.Txt.CastToEnum<PrintFormat>() != null);
            Assert.True(TokenEnum.Txt.CastToEnum<HashAlgr>() == null);
        }

        [Fact]
        public void TokensKeywordList()
        {
            var list = TokenExtension.GetDefaultTokenMapper();
            var keys = new List<string>();
            foreach (var (key, token) in list)
            {
                keys.Add(key);
                AppendLine($"{key,-10}  {token}");
            }

            var keywords = string.Join(", ", keys);

            AppendLine(Endline + "keywords: " + keywords);

            Assert.Contains("then", keywords);
            Assert.Contains("ascii", keywords);
            Assert.DoesNotContain("null", keywords);
        }
    }
}