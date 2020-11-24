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
        /// implicit DataFunction
        /// </summary>
        Hash,

        /// <summary>
        /// has multiple properties and functions
        /// </summary>
        Object,
        Asym,
        Sym,
    }
}