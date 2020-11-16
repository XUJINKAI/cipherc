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

        [TokenDescription(T.DataSource, T.PrintFormat)] Txt,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Hex,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Bin,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Base64,
        [TokenDescription(T.DataSource, T.EncodeFormat, T.DecodeFormat)] Url,

        [TokenDescription(T.DataSource)] Var,
        [TokenDescription(T.DataSource)] File,
        [TokenDescription(T.DataSource)] Rand,
        [TokenDescription(T.DataSource)] Pipe,

        [TokenDescription(T.DataAction)] Encode,
        [TokenDescription(T.DataAction)] Decode,
        [TokenDescription(T.DataAction)] Sub,
        [TokenDescription(T.DataAction)] Print,
        [TokenDescription("printf", T.DataAction)] PrintReadable,
        [TokenDescription(T.DataAction, "repeat")] DuplicateData,
        [TokenDescription(T.DataAction, "concat")] ConcatData,

        [TokenDescription(T.Hash)] Sm3,
        [TokenDescription(T.Hash)] Md5,
        [TokenDescription(T.Hash)] Sha1,
        [TokenDescription(T.Hash)] Sha256,
        [TokenDescription(T.Hash)] Sha384,
        [TokenDescription(T.Hash)] Sha512,
        [TokenDescription(T.Hash)] Sha3,
    }
}
