using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;

namespace CipherTool.Cipher
{
    public interface IHash
    {
        public static Data MessageDigest<T>(Data data) where T : GeneralDigest
        {
            var digest = Activator.CreateInstance<T>();
            digest.BlockUpdate(data.Bytes, 0, data.Length);
            var output = new byte[digest.GetDigestSize()];
            digest.DoFinal(output, 0);
            return output;
        }

        public Data DoHash(Data data);

    }

    public class SM3Hash : IHash
    {
        public Data DoHash(Data data) => IHash.MessageDigest<SM3Digest>(data);
    }
    public class MD5Hash : IHash
    {
        public Data DoHash(Data data) => IHash.MessageDigest<MD5Digest>(data);
    }
    public class SHA1Hash : IHash
    {
        public Data DoHash(Data data) => IHash.MessageDigest<Sha1Digest>(data);
    }
    public class SHA256Hash : IHash
    {
        public Data DoHash(Data data) => IHash.MessageDigest<Sha256Digest>(data);
    }
}
