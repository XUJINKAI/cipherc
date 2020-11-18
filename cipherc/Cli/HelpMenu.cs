using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.AST;
using CipherTool.Tokenizer;
using CipherTool.Utils;

namespace CipherTool.Cli
{
    public static class HelpMenu
    {
        public static string GetHelpText()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Input  format: {TokenType.DataSource.GetTokenEnumsString().ToLower()}");
            sb.AppendLine($"Print  format: {TokenType.PrintFormat.GetTokenEnumsString().ToLower()}");
            sb.AppendLine($"Encode format: {TokenType.EncodeFormat.GetTokenEnumsString().ToLower()}");
            sb.AppendLine($"Decode format: {TokenType.DecodeFormat.GetTokenEnumsString().ToLower()}");
            sb.AppendLine($"Hash Algr: {TokenType.Hash.GetTokenEnumsString()}");
            return sb.ToString();
        }
    }
}
