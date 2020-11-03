using System;
using System.Collections.Generic;
using System.Text;
using CipherTool.AST;
using CipherTool.Utils;

namespace CipherTool.Cli
{
    public static class HelpMenu
    {
        public static string ShowHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Input format: {EnumHelper.StringJoinEnums<DataSource>()}");
            sb.AppendLine($"Encode format: {EnumHelper.StringJoinEnums<EncodeFormat>()}");
            sb.AppendLine($"Decode format: {EnumHelper.StringJoinEnums<DecodeFormat>()}");
            sb.AppendLine($"Hash Algr: {EnumHelper.StringJoinEnums<HashAlgr>()}");
            return sb.ToString();
        }
    }
}
