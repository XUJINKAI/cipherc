using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Tls;

namespace CipherTool.Cipher
{
    public static class Hash
    {
        private static Data MessageDigest<T>(Data data) where T : GeneralDigest
        {
            var digest = Activator.CreateInstance<T>();
            digest.BlockUpdate(data.Bytes, 0, data.Length);
            var output = new byte[digest.GetDigestSize()];
            digest.DoFinal(output, 0);
            return output;
        }

        public static Data SM3(Data data) => MessageDigest<SM3Digest>(data);

        public static Data MD5(Data data) => MessageDigest<MD5Digest>(data);
    }
}
