using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Cipher;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public enum DataFormat
    {
        Plain,
        Hex,
        Base64,
        Bin,
        Pem,
    }

    public enum DataSource
    {
        Arg,
        File,
        Pipe,
    }

    public enum CipherMode
    {
        Ecb,
        Cbc,
        Gcm,
    }

    public class Token
    {
        public int Pos { get; private set; }
        public string Raw { get; private set; }

        public Token(string arg, int position)
        {
            Raw = arg;
            Pos = position;
        }
    }
}
