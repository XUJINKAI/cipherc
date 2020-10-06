using System;
using CipherTool.Parse;
using Xunit;

namespace CipherTool.Test
{
    public class DataExpression_Test
    {
        [Fact]
        public void Test1()
        {
            Parser.Parse(new string[] { "from", "plain", "data", "1234567890" });
        }
    }
}
