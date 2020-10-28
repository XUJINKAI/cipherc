using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Tokenizer
{
    public enum TokenType : int
    {
        [Token] Null,
        [Token] Unknown,

        [Token("then")] SentenceEnd,

        [Token("txt", "ascii")] Txt,
        Hex,
        Base64,
        Url,
        Pem,
        Rand,
        File,
        Pipe,

        Sm3,
        Md5,
        Sha1,
        Sha256,
        Encode,
        Decode,
        Sub,
        Print,

        [Token("repeat")] RepeatData,
        [Token("concat")] ConcatData,

        Var,
        Obj,
    }

    public static class TokenTypeExtension
    {
        public static T? CastToEnum<T>(this TokenType tokenType) where T : struct, Enum
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
