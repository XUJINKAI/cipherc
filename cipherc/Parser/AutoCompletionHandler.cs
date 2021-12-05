using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc.Parser
{
    class AutoCompletionHandler : IAutoCompleteHandler
    {
        // characters to start completion from
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/' };

        public static string GetLastPart(string text)
        {
            var splits = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(text) || text.EndsWith(" ") || splits.Length == 0)
            {
                return "";
            }
            return splits.Last();
        }

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            var last = GetLastPart(text);
            return Directory.GetFiles(".", last + "*", SearchOption.AllDirectories);
        }
    }

}
