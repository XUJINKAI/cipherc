using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc
{
    public interface IAsn1Data : IBytes
    {
        bool CanToPEM { get; }
        string PemTitle { get; }

        string GetPemText(string eol);
        string GetDerDumpText();
    }
}
