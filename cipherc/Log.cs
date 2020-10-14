using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace CipherTool
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

    public static class Log
    {
        public static string EndOfLine { get; set; } = Environment.NewLine;
        public static TextWriter OutputStream { get; set; } = Console.Out;
        public static TextWriter ErrorStream { get; set; } = Console.Error;

        public static void OutputDataLine(string line)
        {
            OutputStream.Write(line + EndOfLine);
        }

        private static void OnLogging(string msg, DateTime dateTime, LogLevel level, string MN, string FP, int LN)
        {
            ErrorStream.WriteLine($"{dateTime:yyyyMMddHHmmSS} {level} {msg} {CodeLocation(MN, FP, LN)}");
        }

        private static string CodeLocation(string CallerMemberName, string CallerFilePath, int CallerLineNumber) => $"{CallerMemberName} in {CallerFilePath}#{CallerLineNumber}";

        public static void Fatal(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Fatal, MN, FP, LN);
        public static void Error(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Error, MN, FP, LN);
        public static void Warn(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Warn, MN, FP, LN);
        public static void Info(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Info, MN, FP, LN);

        [Conditional("DEBUG")]
        public static void Debug(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Debug, MN, FP, LN);
        public static void Trace(string msg, [CallerMemberName] string MN = "", [CallerFilePath] string FP = "", [CallerLineNumber] int LN = 0)
            => OnLogging(msg, DateTime.Now, LogLevel.Trace, MN, FP, LN);
    }
}
