using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace CipherTool.Exceptions
{
    public class UnknownNodeTypeException : BaseException
    {
        public const string KeyFragment = "Unknown Type";

        public override string Message => $"{KeyFragment} {NodeType.FullName}.";

        public Type NodeType { get; }

        public UnknownNodeTypeException(Type nodeType)
        {
            Contract.Assume(nodeType != null);
            NodeType = nodeType;
        }
    }
}