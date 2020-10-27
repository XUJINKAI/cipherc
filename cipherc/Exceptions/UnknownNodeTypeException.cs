using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace CipherTool.Exceptions
{
    public class UnknownNodeTypeException : Exception
    {
        public Type Type { get; }

        public UnknownNodeTypeException(Type type)
            : base($"Unknown Type {type.FullName}.")
        {
            Contract.Assume(type != null);
            Type = type;
        }
    }
}