using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CipherTool.AST;

namespace CipherTool.Interpret
{
    public class NodeValidator : IVisitor
    {
        public List<ValidationResult> ValidationResults { get; } = new List<ValidationResult>();

        public NodeValidator()
        {

        }

        public void Visit(Node node)
        {
            var ctx = new ValidationContext(node);
            Validator.TryValidateObject(node, ctx, ValidationResults, true);
        }
    }
}
