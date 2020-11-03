using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public enum DataSource : int
    {
        Txt = TokenEnum.Txt,
        Hex = TokenEnum.Hex,
        Base64 = TokenEnum.Base64,
        File = TokenEnum.File,
        Var = TokenEnum.Var,
        Rand = TokenEnum.Rand,
        Pipe = TokenEnum.Pipe,
    }

    public enum PrintFormat : int
    {
        Txt = TokenEnum.Txt,
        Hex = TokenEnum.Hex,
        Base64 = TokenEnum.Base64,
    }

    public enum EncodeFormat : int
    {
        Hex = TokenEnum.Hex,
        Base64 = TokenEnum.Base64,
        Url = TokenEnum.Url,
    }

    public enum DecodeFormat : int
    {
        Hex = TokenEnum.Hex,
        Base64 = TokenEnum.Base64,
        Url = TokenEnum.Url,
        Pem = TokenEnum.Pem,
    }

    public enum HashAlgr : int
    {
        Sm3 = TokenEnum.Sm3,
        Md5 = TokenEnum.Md5,
        Sha1 = TokenEnum.Sha1,
        Sha256 = TokenEnum.Sha256,
    }

}
