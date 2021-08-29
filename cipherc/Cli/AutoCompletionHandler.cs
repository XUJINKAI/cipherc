using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Cli
{
    class AutoCompletionHandler : IAutoCompleteHandler
    {
        // characters to start completion from
        public char[] Separators { get; set; } = new char[] { ' ', '.', '/' };

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            var splits = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (string.IsNullOrEmpty(text) || text.EndsWith(" ") || splits.Length == 0)
            {
                return Directory.GetFiles("./");
            }
            var last = splits.Last();
            return Directory.GetFiles(".", last + "*");
        }
    }

}
