global using System;
global using cipherc.Exceptions;
global using cipherc.Utils;
global using libcipherc;

using libcipherc.Utils;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace cipherc
{
    public static partial class ENV
    {
        public static string EndOfLine { get; } = "\n";

        public static CharsetEncoder DefaultEncoder { get; } = CharsetEncoder.GetUTF8Encoder();

        public static CharsetEncoder UTF8Encoder { get; } = CharsetEncoder.GetUTF8Encoder();
        public static CharsetEncoder GBKEncoder { get; } = CharsetEncoder.GetGB18030Encoder();

#if DEBUG
        public static bool DEBUG => true;
#else
        public static bool DEBUG => false;
#endif

        private static string? GetGenString(string key)
        {
            var result = typeof(ENV).GetField(key, BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null)?.ToString();
            return String.IsNullOrEmpty(result) ? null : result;
        }

        public static string? GitTag => GetGenString("_genGitTag");
        public static string? GitHash => GetGenString("_genGitHash");
        public static string? CompileDate => GetGenString("_genDate");
        public static string CompileVersion => DEBUG ? "DEBUG" : "RELEASE";

        public static string DumpEnv()
        {
            var dumper = new DumpHelper(0, 0)
            {
                EndLine = EndOfLine,
            };
            dumper.AppendLine($"Compile", $"{GitTag ?? GitHash}, {CompileVersion}, {CompileDate}");
            dumper.AppendLine($"Process ID", Environment.ProcessId.ToString());
            dumper.AppendLine($"CurrentDirectory", Environment.CurrentDirectory);
            dumper.AppendLine($"Location", AppContext.BaseDirectory);
            dumper.AppendLine($"CommandLine", Environment.CommandLine);
            dumper.AppendLine($"System", $"{Environment.OSVersion} {(Environment.Is64BitOperatingSystem ? "x64" : "x86")}");
            dumper.AppendLine($"Process Arch", Environment.Is64BitProcess ? "x64" : "x86");
            dumper.AppendLine($"Console Charset", Console.OutputEncoding.EncodingName);
            dumper.AppendLine($"NewLine", Regex.Escape(Environment.NewLine));
            return dumper.ToString();
        }
    }
}
