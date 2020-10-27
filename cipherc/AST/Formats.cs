using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public enum InputSource : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        File = TokenType.File,
        Var = TokenType.Var,
        Rand = TokenType.Rand,
    }
    public enum PrintFormat : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
    }
    public enum EncodeFormat : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        Url = TokenType.Url,
    }
    public enum DecodeFormat : int
    {
        Txt = TokenType.Txt,
        Hex = TokenType.Hex,
        Base64 = TokenType.Base64,
        Url = TokenType.Url,
        Pem = TokenType.Pem,
    }
}
