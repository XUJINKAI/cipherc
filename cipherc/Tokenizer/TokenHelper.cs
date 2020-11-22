using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using CipherTool.Utils;

namespace CipherTool.Tokenizer
{
    public static class TokenHelper
    {
        public static IDictionary<string, TokenEnum> TokenMapper { get; } = GetDefaultTokenMapper();

        private static IDictionary<string, TokenEnum> GetDefaultTokenMapper()
        {
            var tokenMapper = new Dictionary<string, TokenEnum>();
            var tokens = Reflector.GetEnumValues<TokenEnum>();
            foreach (var token in tokens)
            {
                var tokenAttr = token.GetTokenDescriptionAttribute();
                if (tokenAttr != null)
                {
                    if (tokenAttr.Types.Contains(TokenType.Null))
                    {
                        continue;
                    }
                    if (tokenAttr.Keywords.Length == 0)
                    {
                        tokenMapper.Add(token.ToString().ToLower(), token);
                    }
                    else
                    {
                        foreach (var keyword in tokenAttr.Keywords)
                        {
                            tokenMapper.Add(keyword.ToLower(), token);
                        }
                    }
                }
                else
                {
                    tokenMapper.Add(token.ToString().ToLower(), token);
                }
            }
            return tokenMapper;
        }
    }

    public static class TokenExtension
    {
        public static bool IsMatch(this Token token, TokenEnum tokenEnum)
        {
            return token.GetTokenEnum() == tokenEnum;
        }

        public static TokenEnum GetTokenEnum(this Token token)
        {
            if (token == null) return TokenEnum.Null;
            var key = token.Text.ToLower();
            if (TokenHelper.TokenMapper.ContainsKey(key))
            {
                return TokenHelper.TokenMapper[key];
            }
            else
            {
                return TokenEnum.Unknown;
            }
        }
    }

    public static class TokenEnumExtension
    {
        public static TokenDescriptionAttribute? GetTokenDescriptionAttribute(this TokenEnum tokenEnum)
        {
            var enumType = typeof(TokenEnum);
            var EnumValue = enumType.GetMember(tokenEnum.ToString()).First();
            return EnumValue.GetCustomAttribute<TokenDescriptionAttribute>();
        }

        public static IList<TokenType> GetTokenTypes(this TokenEnum tokenEnum)
        {
            var attr = tokenEnum.GetTokenDescriptionAttribute();
            return attr != null ? attr.Types : new List<TokenType>();
        }

        public static bool IsMatchTokenType(this TokenEnum tokenEnum, TokenType tokenType)
        {
            return tokenEnum.GetTokenTypes().Contains(tokenType);
        }
    }

    public static class TokenTypeExtension
    {
        public static IReadOnlyCollection<TokenEnum> GetTokenEnums(this TokenType tokenType)
        {
            var values = (TokenEnum[])typeof(TokenEnum).GetEnumValues();
            return values.Where(@enum => @enum.GetTokenTypes().Contains(tokenType)).ToList();
        }
    }
}