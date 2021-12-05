using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace libcipherc.Crypto
{
    public static class Hash
    {
        private static byte[] MessageDigest<T>(byte[] data) where T : IDigest
        {
            var digest = Activator.CreateInstance<T>();
            digest.BlockUpdate(data, 0, data.Length);
            var output = new byte[digest.GetDigestSize()];
            digest.DoFinal(output, 0);
            return output;
        }

        public static byte[] SM3(byte[] data) => MessageDigest<SM3Digest>(data);
        public static byte[] MD5(byte[] data) => MessageDigest<MD5Digest>(data);
        public static byte[] SHA1(byte[] data) => MessageDigest<Sha1Digest>(data);
        public static byte[] SHA256(byte[] data) => MessageDigest<Sha256Digest>(data);
        public static byte[] SHA384(byte[] data) => MessageDigest<Sha384Digest>(data);
        public static byte[] SHA512(byte[] data) => MessageDigest<Sha512Digest>(data);
        public static byte[] SHA3(byte[] data) => MessageDigest<Sha3Digest>(data);
    }
}
