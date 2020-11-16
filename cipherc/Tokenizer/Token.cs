using System;
using System.Reflection;

namespace CipherTool.Tokenizer
{
    public class Token
    {
        public string Text { get; private set; }
        public int Position { get; private set; }

        public Token(string arg, int position)
        {
            Text = arg;
            Position = position;
        }

        public override int GetHashCode() => ToString().GetHashCode(StringComparison.Ordinal);

        public override string ToString() => $"<Token {Position}:'{Text}'>";

        public static bool operator ==(Token token, TokenEnum keyword) => token.IsMatch(keyword);

        public static bool operator !=(Token token, TokenEnum keyword) => !token.IsMatch(keyword);

        public static bool Equals(Token left, Token right) => left.GetTokenEnum() == right.GetTokenEnum();

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Token t => this.GetTokenEnum() == t.GetTokenEnum(),
                TokenEnum k => this.GetTokenEnum() == k,
                _ => false,
            };
        }
    }
}
