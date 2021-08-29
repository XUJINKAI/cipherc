using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CipherTool.AST.Validations;
using CipherTool.Tokenizer;

namespace CipherTool.AST
{
    public class ParserObjectNode : Node
    {
        [TokenTypeValidation(TokenType.Object)]
        public TokenEnum ObjectType { get; }

        public DataNode DataNode { get; }

        public IList<ObjectAction> ObjectActions { get; }

        public ParserObjectNode(TokenEnum type, DataNode data)
        {
            ObjectType = type;
            DataNode = data;
            ObjectActions = new List<ObjectAction>();
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public abstract class ObjectAction : Node
    {

    }

    public class ObjectGetProperty : ObjectAction
    {
        public string PropertyName { get; }

        public ObjectGetProperty(string name)
        {
            PropertyName = name;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }

    public class ObjectSetProperty : ObjectAction
    {
        public string PropertyName { get; }

        public ObjectSetProperty(string name)
        {
            PropertyName = name;
        }

        public override void Accept([NotNull] IVisitor visitor) => visitor.Visit(this);
    }
}
