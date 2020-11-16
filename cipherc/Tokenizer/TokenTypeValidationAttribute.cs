using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using CipherTool.Utils;

namespace CipherTool.Tokenizer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TokenTypeValidationAttribute : ValidationAttribute
    {
        public TokenType ValidType { get; }

        public TokenTypeValidationAttribute(TokenType validType)
        {
            ValidType = validType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            Contract.Assume(ctx != null);
            if (value == null)
            {
                return null;
            }
            var @enum = (TokenEnum)value;
            var types = @enum.GetTokenTypes();
            if (types.Contains(ValidType))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Property {ctx.DisplayName} must be {ValidType} TokenType.");
            }
        }
    }
}
