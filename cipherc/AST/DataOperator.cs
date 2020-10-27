using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public abstract class DataOperator : Node
    {

    }

    public class PrintOperator : DataOperator
    {
        public PrintFormat PrintFormat { get; }

        public PrintOperator(PrintFormat format)
        {
            PrintFormat = format;
        }
    }

    public class HashOperator : DataOperator
    {
        public HashAlgr HashAlgr { get; set; }

        public HashOperator(HashAlgr algr)
        {
            HashAlgr = algr;
        }
    }

    public class EncodeOperator : DataOperator
    {
        public EncodeFormat EncodeFormat { get; set; }

        public EncodeOperator(EncodeFormat format)
        {
            EncodeFormat = format;
        }
    }

    public class DecodeOperator : DataOperator
    {
        public DecodeFormat DecodeFormat { get; set; }

        public DecodeOperator(DecodeFormat format)
        {
            DecodeFormat = format;
        }
    }

    public class SubOperator : DataOperator
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public SubOperator(int start, int length)
        {
            Start = start;
            Length = length;
        }
    }
}
