using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace CipherTool.Parse
{
    public enum LogLevel : int
    {
        Fatal = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 11,
        Trace = 12,
    }

    public class ParseSetting
    {
        public string EndOfLine { get; set; } = Environment.NewLine;
        public TextWriter OutputStream { get; set; } = Console.Out;
        public TextWriter ErrorStream { get; set; } = Console.Error;

        public void OutputDataLine(string line)
        {
            OutputStream.Write(line + EndOfLine);
        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
#pragma warning disable IDE0060 // 删除未使用的参数
#pragma warning disable CA1801 // 检查未使用的参数
        private void OnLogging(string msg, DateTime dateTime, LogLevel level, string MN, string FP, int LN)
        {
            ErrorStream.WriteLine($"[{level}] {msg}");
        }

        private string CodeLocation(string CallerMemberName, string CallerFilePath, int CallerLineNumber) => $"{CallerMemberName} in {CallerFilePath}#{CallerLineNumber}";
#pragma warning restore IDE0060 // 删除未使用的参数
#pragma warning restore CA1801 // 检查未使用的参数
#pragma warning restore IDE0051 // 删除未使用的私有成员

        public void Fatal(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Fatal, MN, FP, LN);
        public void Error(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Error, MN, FP, LN);
        public void Warn(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Warn, MN, FP, LN);
        public void Info(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Info, MN, FP, LN);

        [Conditional("DEBUG")]
        public void Debug(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Debug, MN, FP, LN);
        public void Trace(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Trace, MN, FP, LN);
    }
}
