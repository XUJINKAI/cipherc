using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace cipherc
{
    public enum LogLevel
    {
        Error,
        Verbose,
        Debug,
    }

    /// <summary>
    /// 拥有三个输出通道: Out, Error, Bytes，同时兼容IConsole(用于注入)
    /// </summary>
    public interface IOutput : IConsole
    {
        IList<DumpForm?> DumpForms { get; }

        BinaryWriter? ByteStream { get; set; }

        LogLevel LogLevel { get; set; }

        void Close();
    }

    public static class IOutputExtensions
    {
        public static void WriteBytes(this IOutput output, byte[] formBytes, DumpForm defForm)
        {
            output.WriteBytes(new FormBytes(formBytes, defForm));
        }

        public static void WriteBytes(this IOutput output, FormBytes formBytes)
        {
            output.DebugLine($"Bytes: {output.ByteStream != null}, Out Forms: {output.DumpForms.Select(f => f?.ToString() ?? "null").JoinToString(", ")}");

            if (output.ByteStream != null)
            {
                output.ByteStream.Write(formBytes.ByteArray);
            }

            foreach (var form in output.DumpForms)
            {
                output.WriteLine(formBytes.Dump(form));
            }
        }

        public static void Write(this IOutput output, string s)
        {
            output.Out.Write(s);
        }

        public static void WriteLine(this IOutput output, string s)
        {
            output.Out.Write(s + ENV.EndOfLine);
        }

        private static void LogLine(this IOutput output, LogLevel level, string s)
        {
            if (level > output.LogLevel)
                return;

            if (!Console.IsErrorRedirected) Console.ForegroundColor = level switch
            {
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Verbose => ConsoleColor.Cyan,
                LogLevel.Debug => ConsoleColor.DarkGray,
                _ => throw new NotImplementedException(),
            };

            if (level == LogLevel.Error)
                output.Error.Write(s + ENV.EndOfLine);
            else
                output.Out.Write(s + ENV.EndOfLine);

            if (!Console.IsErrorRedirected) Console.ResetColor();
        }

        public static void VerboseLine(this IOutput output, string s)
        {
            output.LogLine(LogLevel.Verbose, s);
        }

        public static void DebugLine(this IOutput output, string s, [CallerMemberName] string? caller = null)
        {
            output.LogLine(LogLevel.Debug, $"[{caller}] {s}");
        }

        public static void ErrorLine(this IOutput output, string s)
        {
            output.LogLine(LogLevel.Error, s);
        }
    }
}
