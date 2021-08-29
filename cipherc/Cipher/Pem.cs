using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CipherTool.AST;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.OpenSsl;

namespace CipherTool.Cipher
{
    public class Pem : IDataParserObject
    {
        private byte[]? PemStream { get; set; }

        public void LoadData(Data data)
        {
            var memoryStream = new MemoryStream(data);
            var reader = new StreamReader(memoryStream);
            var pemReader = new PemReader(reader);
            var pemObject = pemReader.ReadPemObject();
            PemStream = pemObject.Content;
        }

        public string ToDisplayString()
        {
            return PemStream.ToFormatHexText();
        }
    }
}
