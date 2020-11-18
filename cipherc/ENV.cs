using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#pragma warning disable CA1707 // 标识符不应包含下划线

namespace CipherTool
{
    public static class ENV
    {
#if DEBUG
        public static bool DEBUG => true;
#else
        public static bool DEBUG => false;
#endif
    }
}
