using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public enum HashAlgr : int
    {
        Sm3 = TokenType.Sm3,
        Md5 = TokenType.Md5,
        Sha1 = TokenType.Sha1,
        Sha256 = TokenType.Sha256,
    }

    public class HashOperator : DataOperator
    {
        public HashAlgr HashAlgr { get; set; }
    }

    public class EncodeOperator : DataOperator
    {
        public EncodeFormat EncodeFormat { get; set; }
    }

    public class DecodeOperator : DataOperator
    {
        public DecodeFormat DecodeFormat { get; set; }
    }

    public abstract class DataOperator : Node
    {
        public DataPrimary DataPrimary { get; set; }

        public override void Accept(IVisitor visitor)
        {
            Contract.Assume(visitor != null);
            visitor.Visit(this);
        }
    }
    public class SubOperator : DataOperator
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
