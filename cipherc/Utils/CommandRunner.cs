using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Utils
{
    class CommandRunner
    {
        public static void System(string line)
        {
            string fileName, argument;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                fileName = "cmd.exe";
                argument = $"/C {line}";
            }
            else
            {
                fileName = "/bin/bash";
                argument = $"-c '{line}'";
            }

            var procStartInfo = new ProcessStartInfo(fileName, argument)
            {
            };
            var proc = new Process
            {
                StartInfo = procStartInfo
            };
            proc.Start();
            proc.WaitForExit(1000);
            proc.Kill();
        }
    }
}
