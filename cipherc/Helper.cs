using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CipherTool.Exceptions;

namespace CipherTool
{
    public static class Helper
    {
        public static string GetPipeAllTextIn()
        {
            try
            {
                var x = Console.KeyAvailable;
                throw new NoPipeInputException();
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
