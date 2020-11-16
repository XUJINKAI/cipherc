namespace CipherTool.Tokenizer
{
    public enum TokenType
    {
        Null,
        Custom,

        Command,
        Grammar,

        DataSource,
        PrintFormat,
        EncodeFormat,
        DecodeFormat,

        DataAction,

        Hash,
        Asym,
        Sym,
    }
}