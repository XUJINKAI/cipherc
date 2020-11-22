using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTool.Exceptions
{
    public class EvaluateException : BaseException
    {
        public string ParameterType { get; }

        public new string Message => $"Evaluate Error when parameter is {ParameterType}";

        public EvaluateException(string typeName)
        {
            ParameterType = typeName;
        }
    }
}
