using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using CipherTool.Cipher;
using CipherTool.Cli;
using CipherTool.Exceptions;

namespace CipherTool.Parse
{
    public enum TokenType
    {
        RawString = 0,
        Expression,
        Enum,
    }

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
        private Type? _expressionType;
        private Enum? _enumValue;

        public TokenType TokenType { get; private set; }
        public string Raw { get; private set; }
        public Type? ExpressionType
        {
            get => _expressionType; private set
            {
                TokenType = TokenType.Expression;
                _expressionType = value;
            }
        }
        public Enum? EnumValue
        {
            get => _enumValue; private set
            {
                TokenType = TokenType.Enum;
                _enumValue = value;
            }
        }

        public IExpression MakeExpression(IExpression? parent)
        {
            if (ExpressionType == null)
            {
                throw new UnexpectedTokenException(this);
            }
            var exp = (IExpression?)Activator.CreateInstance(ExpressionType);
            if (exp == null)
            {
                throw new Exception();
            }
            exp.SetParentExpression(parent);
            return exp;
        }

        public Token(string s)
        {
            Contract.Assert(s != null);
            Raw = s;
            TokenType = TokenType.RawString;
            switch (s.ToLower(ENV.CultureInfo))
            {
                case "from":
                    ExpressionType = typeof(DataExpression);
                    break;
                case "to":
                    ExpressionType = typeof(TransformExpression);
                    break;

                case "rand":
                case "random":
                    ExpressionType = typeof(RandomExpression);
                    break;

                case "sm3":
                    ExpressionType = typeof(HashExpression<SM3Hash>);
                    break;
                case "md5":
                    ExpressionType = typeof(HashExpression<MD5Hash>);
                    break;
                case "sha1":
                    ExpressionType = typeof(HashExpression<SHA1Hash>);
                    break;
                case "sha256":
                    ExpressionType = typeof(HashExpression<SHA256Hash>);
                    break;

                case "sm2":
                    ExpressionType = typeof(AsymExpression);
                    break;
                case "sm4":
                    ExpressionType = typeof(SymExpression);
                    break;

                case "get":
                    ExpressionType = typeof(GetExpression);
                    break;
                case "set":
                    ExpressionType = typeof(SetExpression);
                    break;

                case "enc":
                    ExpressionType = typeof(EncExpression);
                    break;
                case "dec":
                    ExpressionType = typeof(DecExpression);
                    break;
                case "sign":
                    ExpressionType = typeof(SignExpression);
                    break;
                case "check":
                    ExpressionType = typeof(SignCheckExpression);
                    break;

                case "plain":
                case "txt":
                case "text":
                    EnumValue = DataFormat.Plain;
                    break;
                case "hex":
                    EnumValue = DataFormat.Hex;
                    break;
                case "base64":
                    EnumValue = DataFormat.Base64;
                    break;
                case "bin":
                    EnumValue = DataFormat.Bin;
                    break;
                case "pem":
                    EnumValue = DataFormat.Pem;
                    break;

                case "arg":
                case "data":
                    EnumValue = DataSource.Arg;
                    break;
                case "file":
                case "path":
                    EnumValue = DataSource.File;
                    break;
                case "pipe":
                case "stdin":
                    EnumValue = DataSource.Pipe;
                    break;

                case "ecb":
                    EnumValue = CipherMode.Ecb;
                    break;
                case "cbc":
                    EnumValue = CipherMode.Cbc;
                    break;
                case "gcm":
                    EnumValue = CipherMode.Gcm;
                    break;
            }
        }
    }
}
