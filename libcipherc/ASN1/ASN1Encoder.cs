using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.OpenSsl;

namespace libcipherc.ASN1
{
    public class ASN1Encoder
    {
        public static Bytes TryParse(Bytes data)
        {
            var memoryStream = new MemoryStream(data);
            var reader = new StreamReader(memoryStream);
            var pemReader = new PemReader(reader);
            var pemObject = pemReader.ReadPemObject();
            return pemObject.Content;
        }
    }
}
