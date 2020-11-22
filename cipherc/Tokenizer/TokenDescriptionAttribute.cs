using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenDescriptionAttribute : Attribute
    {
        public string[] Keywords { get; set; }

        /// <summary>
        /// set single Keywords
        /// </summary>
        public string Alias
        {
            get => throw new NotImplementedException();
            set => Keywords = new string[] { value };
        }

        public IList<TokenType> Types { get; set; }

        public TokenDescriptionAttribute(TokenType firstType, params TokenType[] tokenTypes)
        {
            Keywords = Array.Empty<string>();
            Types = new List<TokenType>(tokenTypes)
            {
                firstType
            };
        }
    }
}