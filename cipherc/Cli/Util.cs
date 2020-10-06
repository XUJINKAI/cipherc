using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CipherTool.Cli
{
    public static class Util
    {
        public static string? GetPipeAllTextIn()
        {
            try
            {
                var x = Console.KeyAvailable;
                return null;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (InvalidOperationException)
            {
                return Console.In.ReadToEnd();
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
