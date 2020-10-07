using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace CipherTool.Cipher
{
    public interface ISym
    {
        Data Key { get; set; }
        Data IV { get; set; }

        Data Enc(Data data);
        Data Dec(Data data);
    }

    public abstract class SymBase : ISym
    {
        public Data Key { get; set; }
        public Data IV { get; set; }

        public Data Enc(Data data)
        {
            AesEngine engine = new AesEngine();
            CbcBlockCipher modeCipher = new CbcBlockCipher(engine);
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(modeCipher);
            KeyParameter keyParam = new KeyParameter(Key);
            ParametersWithIV keyParamWithIV = new ParametersWithIV(keyParam, IV, 0, 16);
            cipher.Init(true, keyParamWithIV);
            byte[] output = new byte[cipher.GetOutputSize(data.Length)];
            int length = cipher.ProcessBytes(data, output, 0);
            cipher.DoFinal(output, length);
            return output;
        }

        public Data Dec(Data data) => throw new NotImplementedException();
    }
}
