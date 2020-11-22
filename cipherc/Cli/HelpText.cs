using System.Linq;
using CipherTool.Tokenizer;

namespace CipherTool.Cli
{
    public static class HelpText
    {
        public static string GetCliHelpText()
        {
            return $@"
Use 'cipherc help' for full help.
Use 'cipherc <COMMAND>' to run command.

e.g. cipherc hex 57656C636F6D6520746F206369706865726321 print txt

Project HomePage: https://github.com/XUJINKAI/cipherc
";
        }

        public static string GetShellWhecomeText()
        {
            return @"Project cipherc
Type ""help"" for more information.
";
        }

        public static string GetFullHelpText()
        {
            return $@"
Welcome to cipherc.

Load Data:
> DataSource <ARG>
e.g. hex 1234
e.g. base64 AAAA

Print Data:
> print/printf DataFormat
e.g. hex 1234 print base64

Data Function:
e.g. hex 1234 encode bin
e.g. hex 1234 sm3

Valid Keywords List:
DataSource:   {GetTokenKeywordsString(TokenType.DataSource)}
DataFormat:   {GetTokenKeywordsString(TokenType.PrintFormat)}
DataOperater: {GetTokenKeywordsString(TokenType.DataFunction)}
EncodeFormat: {GetTokenKeywordsString(TokenType.EncodeFormat)}
DecodeFormat: {GetTokenKeywordsString(TokenType.DecodeFormat)}
HashAlgr:     {GetTokenKeywordsString(TokenType.Hash)}

For more help, report issue, please go homepage:
https://github.com/XUJINKAI/cipherc
";
        }

        public static string GetTokenKeywordsString(TokenType tokenType)
        {
            var values = tokenType.GetTokenEnums();
            var list = values.Where(@enum => @enum.GetTokenTypes().Contains(tokenType))
                .Select(@enum =>
                {
                    var attr = @enum.GetTokenDescriptionAttribute();
                    if (attr != null && attr.Keywords.Length > 0)
                    {
                        if (attr.Keywords.Length == 1)
                        {
                            return attr.Keywords[0];
                        }
                        else
                        {
                            return $"{attr.Keywords[0]}({string.Join(", ", attr.Keywords[1..])})";
                        }
                    }
                    else
                    {
                        return @enum.ToString();
                    }
                }).ToList();
            return string.Join(", ", list).ToLower();
        }
    }
}
