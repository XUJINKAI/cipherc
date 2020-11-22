namespace CipherTool.Tokenizer
{
    public enum TokenType
    {
        Null,
        Custom,

        Command,
        Grammar,

        DataSource,
        DataDestination,
        PrintFormat,
        EncodeFormat,
        DecodeFormat,

        /// <summary>
        /// one Data input and one Data output
        /// </summary>
        DataFunction,

        /// <summary>
        /// one Data input, no output
        /// </summary>
        DataAction,

        /// <summary>
        /// implicit DataFunction
        /// </summary>
        Hash,
        Asym,
        Sym,
    }
}