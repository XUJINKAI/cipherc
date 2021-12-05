using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc.ASN1
{
    public abstract class AbstractAsn1Data : IAsn1Data
    {
        public virtual byte[] ByteArray { get; set; }

        protected AbstractAsn1Data(byte[] byteArray)
        {
            ByteArray = byteArray;
        }

        public abstract bool CanToPEM { get; }

        public abstract string PemTitle { get; }

        public string GetPemText(string eol = "\n")
        {
            if (PemTitle == null)
                throw new FormatException($"Data has no PEM header.");
            return $"-----BEGIN {PemTitle}-----{eol}{ByteArray.ToBase64String(eol)}{eol}-----END {PemTitle}-----{eol}";
        }

        public string GetDerDumpText()
        {
            throw new NotImplementedException();
        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }
    }
}
