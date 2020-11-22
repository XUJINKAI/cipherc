using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using CipherTool.Cli;
using CipherTool.Tokenizer;

namespace CipherTool.AST.Validations
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
                return new ValidationResult($"Valid {ctx.DisplayName} values: {HelpText.GetTokenKeywordsString(ValidType)}");
            }
        }
    }
}
