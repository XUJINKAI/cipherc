using System;
using cipherc.Exceptions;

namespace cipherc.Utils
{
    public static class ConsolePipeHelper
    {
        private static string? _mockInputText;

        public static void MockConsoleInput(string? text)
        {
            _mockInputText = text;
        }

        public static string? GetPipeAllTextIn()
        {
            if (_mockInputText != null)
            {
                return _mockInputText;
            }

            try
            {
                var x = Console.KeyAvailable;
                return null;
            }
            catch (InvalidOperationException)
            {
                return Console.In.ReadToEnd();
            }
        }
    }
}
