using T = CipherTool.Tokenizer.TokenType;

namespace CipherTool.Tokenizer
{
    public enum TokenEnum : int
    {
        [TokenDescription(T.Null)] Null,
        [TokenDescription(T.Null)] Unknown,

        [TokenDescription(T.Command, "help")] Help,
        [TokenDescription(T.Command, "vars")] Variables,

        [TokenDescription(T.Grammar, "then")] SentenceEnd,
        [TokenDescription(T.Grammar)] Obj,
        [TokenDescription(T.Grammar, "is", "=")] AssignSymbol,

        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Hex,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Bin,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Base64,

        [TokenDescription(T.DataSource, T.PrintFormat)] Txt,
        [TokenDescription(T.EncodeFormat, T.DecodeFormat)] Url,
        [TokenDescription(T.DecodeFormat)] Pem,

        [TokenDescription(T.DataSource)] Var,
        [TokenDescription(T.DataSource)] File,
        [TokenDescription(T.DataSource)] Rand,
        [TokenDescription(T.DataSource)] Pipe,

        [TokenDescription(T.DataFunction)] Encode,
        [TokenDescription(T.DataFunction)] Decode,
        [TokenDescription(T.DataFunction)] Sub,
        [TokenDescription(T.DataFunction, "repeat")] DuplicateData,
        [TokenDescription(T.DataFunction, "concat")] ConcatData,
        [TokenDescription(T.DataFunction)] Print,
        [TokenDescription("printf", T.DataFunction)] PrintReadable,

        [TokenDescription("hash", T.DataAction, Description = "Print all support hash result")] PrintSupportedHashAction,

        [TokenDescription(T.Hash)] Sm3,
        [TokenDescription(T.Hash)] Md5,
        [TokenDescription(T.Hash)] Sha1,
        [TokenDescription(T.Hash)] Sha256,
        [TokenDescription(T.Hash)] Sha384,
        [TokenDescription(T.Hash)] Sha512,
        [TokenDescription(T.Hash)] Sha3,
    }
}
