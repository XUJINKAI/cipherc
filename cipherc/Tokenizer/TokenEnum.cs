using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Tokenizer
{
    public enum TokenEnum : int
    {
        [TokenDescription(TokenType.Null)] Null,
        [TokenDescription(TokenType.Null)] Unknown,

        [TokenDescription(TokenType.Command, "help")] Help,
        [TokenDescription(TokenType.Command, "vars")] Variables,

        [TokenDescription(TokenType.Grammar, "then")] SentenceEnd,
        [TokenDescription(TokenType.Grammar)] Var,
        [TokenDescription(TokenType.Grammar)] Obj,
        [TokenDescription(TokenType.Grammar, "is", "=")] AssignSymbol,

        [TokenDescription(TokenType.DataFormat, "txt", "ascii")] Txt,
        [TokenDescription(TokenType.DataFormat)] Hex,
        [TokenDescription(TokenType.DataFormat)] Base64,
        [TokenDescription(TokenType.DataFormat)] Pem,
        [TokenDescription(TokenType.DataFormat)] Url,

        [TokenDescription(TokenType.DataSource)] Rand,
        [TokenDescription(TokenType.DataSource)] File,
        [TokenDescription(TokenType.DataSource)] Pipe,

        [TokenDescription(TokenType.DataAction)] Encode,
        [TokenDescription(TokenType.DataAction)] Decode,
        [TokenDescription(TokenType.DataAction)] Sub,
        [TokenDescription(TokenType.DataAction)] Print,

        [TokenDescription(TokenType.DataAction, "repeat")] DuplicateData,
        [TokenDescription(TokenType.DataAction, "concat")] ConcatData,

        [TokenDescription(TokenType.Hash)] Sm3,
        [TokenDescription(TokenType.Hash)] Md5,
        [TokenDescription(TokenType.Hash)] Sha1,
        [TokenDescription(TokenType.Hash)] Sha256,
    }

    public static class TokenEnumExtension
    {
        public static T? CastToEnum<T>(this TokenEnum tokenType) where T : struct, Enum
        {
            int i = (int)tokenType;
            if (Enum.IsDefined(typeof(T), i))
            {
                return Enum.Parse<T>(i.ToString());
            }
            return null;
        }
    }
}
