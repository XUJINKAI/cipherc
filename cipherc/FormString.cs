using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc
{
    /// <summary>
    /// 带有编码信息的string
    /// </summary>
    public class FormString
    {
        public string Value { get; }

        public string Encoding { get; }

        public FormString(string s, string encoding)
        {
            Value = s;
            Encoding = encoding;
        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
