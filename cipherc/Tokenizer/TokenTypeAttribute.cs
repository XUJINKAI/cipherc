using System;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TokenTypeAttribute : Attribute
    {
        public TokenTypeAttribute() { }
        public TokenTypeAttribute(params string[] Alias) { }
    }
}