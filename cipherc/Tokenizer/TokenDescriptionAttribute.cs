using System;
using System.Collections.Generic;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenDescriptionAttribute : Attribute
    {
        public IReadOnlyCollection<string> Keywords { get; }

        public string Description { get; set; } = "";

        public TokenType Type { get; set; }

        public TokenDescriptionAttribute(TokenType tokenType)
        {
            Type = tokenType;
            Keywords = new List<string>();
        }

        public TokenDescriptionAttribute(TokenType tokenType, params string[] keywords)
        {
            Type = tokenType;
            Keywords = new List<string>(keywords);
        }
    }
}