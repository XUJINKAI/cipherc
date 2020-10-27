using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public enum DataSource : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        File = TokenType.File,
        Var = TokenType.Var,
        Rand = TokenType.Rand,
        Pipe = TokenType.Pipe,
    }

    public enum PrintFormat : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
    }

    public enum EncodeFormat : int
    {
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        Url = TokenType.Url,
    }

    public enum DecodeFormat : int
    {
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        Url = TokenType.Url,
        Pem = TokenType.Pem,
    }

    public enum HashAlgr : int
    {
        Sm3 = TokenType.Sm3,
        Md5 = TokenType.Md5,
        Sha1 = TokenType.Sha1,
        Sha256 = TokenType.Sha256,
    }

}
