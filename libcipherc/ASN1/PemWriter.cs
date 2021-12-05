using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libcipherc.ASN1
{
    public static class PemTitle
    {
        public const string Certificate = "Certificate";
    }

    public class PemWriter
    {
        public static void WritePem(TextWriter writer, byte[] data, string title)
        {
            string eol = "\n";
            var result = @$"-----BEGIN {title}-----{eol}{data.ToBase64String(eol)}{eol}-----END {title}-----{eol}";
            writer.Write(result);
        }
    }
}
