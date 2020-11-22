using T = CipherTool.Tokenizer.TokenType;

namespace CipherTool.Tokenizer
{
    public enum TokenEnum : int
    {
        /// <summary>
        /// Should Never Used
        /// </summary>
        [TokenDescription(T.Null)] Null = 0,
        [TokenDescription(T.Null)] Unknown,

        /// <summary>
        /// To skip first word (app name) in shell mode
        /// </summary>
        [TokenDescription(T.Grammar, Alias = "cipherc")] ApplicationName,

        [TokenDescription(T.Command)] Help,
        [TokenDescription(T.Command, Alias = "vars")] Variables,

        [TokenDescription(T.Grammar, Keywords = new string[] { "then", ";" })] SentenceEnd,
        [TokenDescription(T.Grammar, Keywords = new string[] { "is", "=" })] AssignSymbol,
        [TokenDescription(T.Grammar)] Obj,
        [TokenDescription(T.Grammar, Keywords = new string[] { "concat", "+" })] ConcatData,

        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Hex,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Bin,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Base64,
        [TokenDescription(T.DataSource, T.PrintFormat, T.EncodeFormat, T.DecodeFormat)] Url,

        [TokenDescription(T.DataSource, T.PrintFormat, Keywords = new string[] { "txt", "utf8" })] Txt,
        [TokenDescription(T.PrintFormat)] Ascii,
        [TokenDescription(T.PrintFormat, Alias = "auto")] AutoPrint,
        [TokenDescription(T.DecodeFormat)] Pem,

        [TokenDescription(T.DataSource)] Rand,
        [TokenDescription(T.DataSource)] Pipe,
        [TokenDescription(T.DataSource, T.DataDestination)] Var,
        [TokenDescription(T.DataSource, T.DataDestination)] File,

        [TokenDescription(T.DataFunction)] Encode,
        [TokenDescription(T.DataFunction)] Decode,
        [TokenDescription(T.DataFunction, Keywords = new string[] { "repeat", "*" })] DuplicateData,
        [TokenDescription(T.DataFunction, Alias = "sub")] Sub,

        [TokenDescription(T.DataFunction)] Print,
        [TokenDescription(T.DataFunction, Alias = "printf")] PrintReadable,
        [TokenDescription(T.DataFunction, Alias = "to")] SaveData,

        [TokenDescription(T.Hash)] Sm3,
        [TokenDescription(T.Hash)] Md5,
        [TokenDescription(T.Hash)] Sha1,
        [TokenDescription(T.Hash)] Sha256,
        [TokenDescription(T.Hash)] Sha384,
        [TokenDescription(T.Hash)] Sha512,
        [TokenDescription(T.Hash)] Sha3,

        /// <summary>
        /// Print all support hash result
        /// </summary>
        [TokenDescription(T.DataAction, Alias = "hash")] PrintSupportedHashAction,
    }
}
