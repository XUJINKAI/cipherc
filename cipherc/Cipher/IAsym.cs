using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;

namespace CipherTool.Cipher
{
    public interface IAsym
    {
        Data Key { get; set; }
        Data Pk { get; set; }

        Data Enc(Data data);
        Data Dec(Data data);
        Data Sign(Data data);
        bool SignCheck(Data data);
    }

    public class SM2Asym : IAsym
    {
        public Data Key { get; set; }
        public Data Pk { get; set; }

        public Data Enc(Data data) => throw new NotImplementedException();
        public Data Dec(Data data) => throw new NotImplementedException();
        public Data Sign(Data data) => throw new NotImplementedException();
        public bool SignCheck(Data data) => throw new NotImplementedException();
    }
}
