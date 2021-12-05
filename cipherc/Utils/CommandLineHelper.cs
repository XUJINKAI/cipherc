using System;
using System.CommandLine.Parsing;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;

namespace cipherc.Utils
{
    public static class CommandLineHelper
    {
        public static string[] CommandLineToArgs(string commandLine)
        {
            return CommandLineStringSplitter.Instance.Split(commandLine).ToArray();
            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    return CommandLineToArgsPInvoke(commandLine);
            //}
            //else
            //{
            //    return CommandLineToArgsNativeImplement(commandLine);
            //}
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
            out int pNumArgs);

        private static string[] CommandLineToArgsPInvoke(string commandLine)
        {
            if (string.IsNullOrEmpty(commandLine))
                return Array.Empty<string>();

            var argv = CommandLineToArgvW(commandLine, out int argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();

            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p) ?? throw new Exception();
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        // https://gist.github.com/csMACnz/043750b363f12d28496bf1d926ef4ab3
        // Based on https://github.com/dotnet/coreclr/blob/726d1a3244b80bf963fd0d51e57d4bb90af1e426/src/utilcode/util.cpp#L2780
        // But in C#.
        private static string[] CommandLineToArgsNativeImplement(string args)
        {
            Contract.Assert(args != null);

            //        STATIC_CONTRACT_NOTHROW;
            //        STATIC_CONTRACT_GC_NOTRIGGER;
            //        STATIC_CONTRACT_FAULT;


            //        *pNumArgs = 0;
            var pNumArgs = 0;

            //        int nch = (int)wcslen(lpCmdLine);
            var nch = args.Length;

            // Calculate the worstcase storage requirement. (One pointer for
            // each argument, plus storage for the arguments themselves.)
            //        int cbAlloc = (nch+1)*sizeof(LPWSTR) + sizeof(WCHAR)*(nch + 1);
            //        LPWSTR pAlloc = new (nothrow) WCHAR[cbAlloc / sizeof(WCHAR)];
            //        if (!pAlloc)
            //            return NULL;

            //        LPWSTR *argv = (LPWSTR*) pAlloc;  // We store the argv pointers in the first halt
            var argv = new string[nch];
            //        LPWSTR  pdst = (LPWSTR)( ((BYTE*)pAlloc) + sizeof(LPWSTR)*(nch+1) ); // A running pointer to second half to store arguments
            //        LPCWSTR psrc = lpCmdLine;
            var psrc = args;
            //        WCHAR   c;
            //        char c;
            //        BOOL    inquote;
            bool inquote;
            //        BOOL    copychar;
            bool copychar;
            int numslash;

            // First, parse the program name (argv[0]). Argv[0] is parsed under
            // special rules. Anything up to the first whitespace outside a quoted
            // subtring is accepted. Backslashes are treated as normal characters.
            //        argv[ (*pNumArgs)++ ] = pdst;
            //        inquote = FALSE;
            //        do {
            //            if (*psrc == W('"') )
            //            {
            //                inquote = !inquote;
            //                c = *psrc++;
            //                continue;
            //            }
            //            *pdst++ = *psrc;

            //            c = *psrc++;

            //        } while ( (c != W('\0') && (inquote || (c != W(' ') && c != W('\t')))) );

            //        if ( c == W('\0') ) {
            //            psrc--;
            //        } else {
            //            *(pdst-1) = W('\0');
            //        }

            //        inquote = FALSE;
            inquote = false;

            var currentIndex = 0;

            /* loop on each argument */
            for (; ; )
            {
                //            if ( *psrc )
                if (currentIndex != psrc.Length)
                {
                    //                while (*psrc == W(' ') || *psrc == W('\t'))
                    while (currentIndex != psrc.Length && (psrc[currentIndex] == ' ' || psrc[currentIndex] == '\t'))
                    {
                        //                    ++psrc;
                        currentIndex++;
                    }
                }

                //            if (*psrc == W('\0'))
                if (currentIndex == psrc.Length)
                    break;              /* end of args */

                /* scan an argument */
                //            argv[ (*pNumArgs)++ ] = pdst;
                var result = "";
                /* loop through scanning one argument */
                for (; ; )
                {
                    //                copychar = 1;
                    copychar = true;
                    /* Rules: 2N backslashes + " ==> N backslashes and begin/end quote
                    2N+1 backslashes + " ==> N backslashes + literal "
                    N backslashes ==> N backslashes */
                    numslash = 0;
                    //                while (*psrc == W('\\'))
                    while (currentIndex != psrc.Length && psrc[currentIndex] == '\\')
                    {
                        /* count number of backslashes for use below */
                        //                    ++psrc;
                        currentIndex++;
                        ++numslash;
                    }
                    //                if (*psrc == W('"'))
                    if (currentIndex != psrc.Length && psrc[currentIndex] == '"')
                    {
                        /* if 2N backslashes before, start/end quote, otherwise
                        copy literally */
                        if (numslash % 2 == 0)
                        {
                            //                        if (inquote && psrc[1] == W('"'))
                            if (inquote && psrc.Length != currentIndex + 1 && psrc[currentIndex + 1] == '"')
                            {
                                //                            psrc++;    /* Double quote inside quoted string */
                                currentIndex++;
                            }
                            else
                            {
                                /* skip first quote char and copy second */
                                //                            copychar = 0;       /* don't copy quote */
                                copychar = false;
                                inquote = !inquote;
                            }
                        }
                        numslash /= 2;          /* divide numslash by two */
                    }

                    /* copy slashes */
                    //                while (numslash--)
                    while (numslash != 0)
                    {
                        numslash--;
                        //                    *pdst++ = W('\\');
                        result += '\\';
                    }

                    /* if at end of arg, break loop */
                    //                if (*psrc == W('\0') || (!inquote && (*psrc == W(' ') || *psrc == W('\t'))))
                    if (currentIndex == psrc.Length || (!inquote && (psrc[currentIndex] == ' ' || psrc[currentIndex] == '\t')))
                        break;

                    /* copy character into argument */
                    if (copychar)
                    {
                        //                    *pdst++ = *psrc;
                        result += psrc[currentIndex];
                    }
                    //                ++psrc;
                    currentIndex++;
                }

                /* null-terminate the argument */

                //            *pdst++ = W('\0');          /* terminate string */
                argv[pNumArgs] = result;
                pNumArgs++;
            }

            /* We put one last argument in -- a null ptr */
            //        argv[ (*pNumArgs) ] = NULL;

            // If we hit this assert, we overwrote our destination buffer.
            // Since we're supposed to allocate for the worst
            // case, either the parsing rules have changed or our worse case
            // formula is wrong.
            //        _ASSERTE((BYTE*)pdst <= (BYTE*)pAlloc + cbAlloc);
            //        return argv;

            return argv.TakeWhile(n => n != null).ToArray();
        }
    }
}
