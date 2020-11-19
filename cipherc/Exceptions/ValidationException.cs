using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Exceptions
{
    public class ValidationException : BaseException
    {
        public IEnumerable<ValidationResult> ValidationResults { get; }

        public ValidationException(IEnumerable<ValidationResult> validationResults)
        {
            ValidationResults = validationResults;
        }
    }
}
