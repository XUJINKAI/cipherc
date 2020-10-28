using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CipherTool.Exceptions;

namespace CipherTool
{
    public static class Helper
    {
#if DEBUG
        private static bool _isMockConsoleInput;
        private static string? _mockInputText;
        public static void MockConsoleInput(string? text)
        {
            _isMockConsoleInput = true;
            _mockInputText = text;
        }
#endif
        public static string GetPipeAllTextIn()
        {
#if DEBUG
            if (_isMockConsoleInput)
            {
                _isMockConsoleInput = false;
                return _mockInputText ?? throw new NoPipeInputException();
            }
#endif
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
