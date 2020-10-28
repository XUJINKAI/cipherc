using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text;
using CipherTool.Cipher;
using CipherTool.Exceptions;
using Microsoft.VisualBasic.CompilerServices;
using Org.BouncyCastle.Crypto.Operators;

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

        public static bool operator ==(Token token, TokenType keyword) => token.IsMatch(keyword);

        public static bool operator !=(Token token, TokenType keyword) => !token.IsMatch(keyword);

        public static bool Equals(Token left, Token right) => left.GetTokenType() == right.GetTokenType();

        public override bool Equals(object obj)
        {
            return obj switch
            {
                Token t => this.GetTokenType() == t.GetTokenType(),
                TokenType k => this.GetTokenType() == k,
                _ => false,
            };
        }
    }

    public static class TokenExtension
    {
        private static readonly IDictionary<string, TokenType> TokenMapper;

        static TokenExtension()
        {
            TokenMapper = GetDefaultTokenMapper();
        }

        public static IDictionary<string, TokenType> GetDefaultTokenMapper()
        {
            var tokenMapper = new Dictionary<string, TokenType>();
            var tokens = (TokenType[])Enum.GetValues(typeof(TokenType));
            foreach (var token in tokens)
            {
                var tokenAttr = token.GetType().GetMember(token.ToString())[0].GetCustomAttribute(typeof(TokenAttribute));
                if (tokenAttr is TokenAttribute attr)
                {
                    foreach (var keyword in attr.Keywords)
                    {
                        tokenMapper.Add(keyword.ToLower(ENV.CultureInfo), token);
                    }
                }
                else
                {
                    tokenMapper.Add(token.ToString().ToLower(ENV.CultureInfo), token);
                }
            }
            return tokenMapper;
        }

        public static TokenType GetTokenType(this Token token)
        {
            if (token == null) return TokenType.Null;
            var key = token.Text.ToLower(ENV.CultureInfo);
            if (TokenMapper.ContainsKey(key))
            {
                return TokenMapper[key];
            }
            else
            {
                return TokenType.Unknown;
            }
        }

        public static T? ToEnum<T>(this Token token) where T : struct, Enum
        {
            var type = token.GetTokenType();
            return type.CastToEnum<T>();
        }

        public static bool IsMatch(this Token token, TokenType keyword)
        {
            return token.GetTokenType() == keyword;
        }
    }
}
