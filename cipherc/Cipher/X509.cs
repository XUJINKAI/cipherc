using System;
using CipherTool.AST;
using Org.BouncyCastle.X509;

namespace CipherTool.Cipher
{
    public class X509 : IDataParserObject
    {
        public X509Certificate? Certificate { get; private set; }

        public void LoadData(Data data)
        {
            var parser = new X509CertificateParser();
            Certificate = parser.ReadCertificate(data);
        }

        public string ToDisplayString()
        {
            return Certificate?.ToString() ?? throw new Exception();
        }
    }
}
