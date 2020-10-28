using System;
using System.Collections.Generic;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenAttribute : Attribute
    {
        public IReadOnlyCollection<string> Keywords { get; }

        public TokenAttribute()
        {
            Keywords = new List<string>();
        }

        public TokenAttribute(params string[] keywords)
        {
            Keywords = new List<string>(keywords);
        }
    }
}