using System;
using Org.BouncyCastle.X509;

namespace libcipherc.ASN1
{
    public class X509 : AbstractAsn1Data
    {
        public override byte[] ByteArray => X509Certificate.GetEncoded();

        public X509Certificate X509Certificate { get; private set; }

        public X509(byte[] byteArray) : base(byteArray)
        {
            var parser = new X509CertificateParser();
            X509Certificate = parser.ReadCertificate(byteArray);
        }

        public override string ToString()
        {
            return X509Certificate.ToString();
        }

        public override bool CanToPEM => throw new NotImplementedException();

        public override string PemTitle => throw new NotImplementedException();
    }
}
