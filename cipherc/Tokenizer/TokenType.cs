using System;
using System.Collections.Generic;
using System.Text;

namespace CipherTool.Tokenizer
{
    public enum TokenType : int
    {
        Null,
        Unknown,

        SentenceEnd,

        Txt,
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

        Var,
        Obj,
    }

    public static class TokenTypeExtension
    {

    }
}
