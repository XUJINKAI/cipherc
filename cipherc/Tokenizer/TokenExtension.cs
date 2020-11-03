using System;
using System.Collections.Generic;
using CipherTool.Utils;

namespace CipherTool.Tokenizer
{
    public static class TokenExtension
    {
        private static readonly IDictionary<string, TokenEnum> TokenMapper = GetDefaultTokenMapper();

        public static IDictionary<string, TokenEnum> GetDefaultTokenMapper()
        {
            var tokenMapper = new Dictionary<string, TokenEnum>();
            var tokens = EnumHelper.GetEnumValues<TokenEnum>();
            foreach (var token in tokens)
            {
                var tokenAttr = token.GetAttribute<TokenDescriptionAttribute>();
                if (tokenAttr != null)
                {
                    if (tokenAttr.Type == TokenType.Null)
                    {
                        continue;
                    }
                    if (tokenAttr.Keywords.Count == 0)
                    {
                        tokenMapper.Add(token.ToString().ToLower(ENV.CultureInfo), token);
                    }
                    else
                    {
                        foreach (var keyword in tokenAttr.Keywords)
                        {
                            tokenMapper.Add(keyword.ToLower(ENV.CultureInfo), token);
                        }
                    }
                }
                else
                {
                    tokenMapper.Add(token.ToString().ToLower(ENV.CultureInfo), token);
                }
            }
            return tokenMapper;
        }

        public static TokenEnum GetTokenType(this Token token)
        {
            if (token == null) return TokenEnum.Null;
            var key = token.Text.ToLower(ENV.CultureInfo);
            if (TokenMapper.ContainsKey(key))
            {
                return TokenMapper[key];
            }
            else
            {
                return TokenEnum.Unknown;
            }
        }

        public static T? ToEnum<T>(this Token token) where T : struct, Enum
        {
            var type = token.GetTokenType();
            return type.CastToEnum<T>();
        }

        public static bool IsMatch(this Token token, TokenEnum keyword)
        {
            return token.GetTokenType() == keyword;
        }
    }
}