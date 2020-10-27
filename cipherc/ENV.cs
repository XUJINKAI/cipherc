using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CipherTool
{
#pragma warning disable CA1707 // 标识符不应包含下划线
    public static class ENV
    {
        public const string GetBytes_DefaultEncoding = "UTF8";
        public static CultureInfo CultureInfo => CultureInfo.CurrentCulture;
#if DEBUG
        public static bool DEBUG => true;
#else
        public static bool DEBUG => false;
#endif
    }
#pragma warning restore CA1707 // 标识符不应包含下划线
}
