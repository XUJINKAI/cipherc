using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenDescriptionAttribute : Attribute
    {
        public IReadOnlyCollection<string> Keywords { get; }

        public string Description { get; set; } = "";

        public IReadOnlyCollection<TokenType> Types { get; set; }

        public TokenDescriptionAttribute(TokenType tokenType)
        {
            Types = new List<TokenType>(new TokenType[] { tokenType });
            Keywords = new List<string>();
        }

        public TokenDescriptionAttribute(TokenType tokenType, params string[] keywords)
        {
            Types = new List<TokenType>(new TokenType[] { tokenType });
            Keywords = new List<string>(keywords);
        }

        public TokenDescriptionAttribute(params TokenType[] tokenTypes)
        {
            Keywords = new List<string>();
            Types = new List<TokenType>(tokenTypes);
        }

        public TokenDescriptionAttribute(string keyword, params TokenType[] tokenTypes)
        {
            Keywords = new List<string>(new string[] { keyword });
            Types = new List<TokenType>(tokenTypes);
        }
    }
}