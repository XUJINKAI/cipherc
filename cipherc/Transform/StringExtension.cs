using System;
using System.Text;

namespace CipherTool.Transform
{
    public static class StringExtension
    {
        /// <summary>
        /// 将字符串中的每一个字符都转换成对应的byte，对于Unicode字符，默认使用UTF8编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">Available value: ASCII, UTF8, UTF16, UTF32。默认值在SdkConfig中定义。</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str, string? encoding = null)
        {
            encoding ??= ENV.GetBytes_DefaultEncoding;
            return encoding switch
            {
                "UTF8" => Encoding.UTF8.GetBytes(str),
                "ASCII" => Encoding.ASCII.GetBytes(str),
                "UTF16" => Encoding.Unicode.GetBytes(str),
                "UTF32" => Encoding.UTF32.GetBytes(str),
                _ => throw new ArgumentException($"Unknown encoding {encoding}"),
            };
        }

        public static bool IgnoreCaseContains(this string str, string sub)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (sub == null) throw new ArgumentNullException(nameof(sub));
            return str.Contains(sub, StringComparison.OrdinalIgnoreCase);
        }
    }
}
