using libcipherc.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cipherc.Parser.Options;
using System.IO;
using libcipherc.ASN1;

namespace cipherc.Handler
{
    public class MainHandler
    {
        public Delegate CreateDataIntGenerator(Func<int, byte[]> genFunc, DumpForm defDisForm)
        {
            return (IOutput output, int bytes) =>
            {
                var data = genFunc(bytes);
                output.WriteBytes(data, defDisForm);
            };
        }

        public Delegate CreateDataEncoder(Func<byte[], byte[]> encodeFunc, DumpForm defDisForm)
        {
            return (IOutput output, InputDataModel input) =>
            {
                var data = input.GetBytes();
                var result = encodeFunc(data);
                output.WriteBytes(result, defDisForm);
            };
        }

        public void Base64Handler(IOutput output, InputDataModel input, bool decode, bool breakLines)
        {
            if (decode)
            {
                var result = Convert.FromBase64String(input.Input);
                output.WriteBytes(result, DumpForm.UTF8);
            }
            else
            {
                var data = input.GetBytes();
                var b64str = data.ToBase64String(breakLines ? ENV.EndOfLine : "");
                var result = b64str.GetBytes();
                output.WriteBytes(result, DumpForm.UTF8);
            }
        }

        public void Asn1FileLoader(IOutput output,
                string filePath
                )
        {
            var content = File.ReadAllBytes(filePath);
            var bytes = ASN1Encoder.TryParse(content);
            var dump = new DerParser().Dump(bytes);
            output.WriteLine(dump);
        }

        public void FileOperation(IOutput output,
            string filePath,
            bool __info = false,
            bool __sm3 = false,
            bool __text = false,
            bool __noout = false
            )
        {
            var bytes = DATA.FILE(filePath);

            if (__info)
            {
                var info = new FileInfo(filePath);
                const string format = "yyyy-MM-dd HH:mm:ss";
                output.WriteLine($"Size:   {info.Length}");
                output.WriteLine($"Create: {info.CreationTime.ToString(format)}");
                output.WriteLine($"Modify: {info.LastWriteTime.ToString(format)}");
                output.WriteLine($"Access: {info.LastAccessTime.ToString(format)}");
            }

            if (__sm3)
            {
                output.WriteLine($"SM3: {Hash.SM3(bytes).ToHexString()}");
            }

            if (__text)
            {
                var encoder = CharsetEncoder.GuessEncoder(bytes, 0.5f);
                if (encoder == null)
                {
                    output.WriteLine($"Encoding: Unknown");
                }
                else
                {
                    output.WriteLine($"Encoding: {encoder.Charset}{(encoder.UseBOM ? " with BOM" : "")}");
                    output.WriteLine("Text:");
                    output.WriteLine(encoder.Decode(bytes) + "<<<");
                }
            }

            if (!__noout)
            {
                output.WriteBytes(new FormBytes(bytes, DumpForm.Hex));
            }
        }


        public void Sym_SM4(IOutput output, InputDataModel input,
             string inKey,
            string? inIV = null,
            bool isDec = false,
           string inAlgr = "sm4cbc")
        {
            var data = input.GetBytes();

            var (alg, mode) = inAlgr.ToLower() switch
            {
                "sm4" => ("SM4", "CBC"),
                "sm4cbc" => ("SM4", "CBC"),
                "sm4ecb" => ("SM4", "ECB"),

                "aes" => ("AES", "CBC"),
                "aescbc" => ("AES", "CBC"),
                "aesecb" => ("AES", "ECB"),
                _ => throw new ParseErrorException($"Unknown sym algr {inAlgr}"),
            };
            var direction = isDec ? "DEC" : "ENC";
            var blockSize = alg switch
            {
                "SM4" => 16,
                "AES" => 16,
                _ => throw new NotImplementedException(),
            };

            var key = inKey switch
            {
                "rand" => DATA.RAND(blockSize),
                _ => DATA.HEX(inKey),
            };

            var iv = inIV switch
            {
                "0" => DATA.ZEROS(blockSize),
                _ => DATA.HEX(inIV ?? ""),
            };

            Validator.BytesLength(key, blockSize, "Key");
            Validator.BytesLength(iv, blockSize, "IV");

            output.VerboseLine($"ALG: {alg}-{mode}-{direction}");
            output.VerboseLine($"KEY: {key.ToHexString()}");
            output.VerboseLine($"IV : {iv.ToHexString()}");
            output.VerboseLine($"Data: {data.ToHexString()}");

            var result = (alg, mode, direction) switch
            {
                ("SM4", "CBC", "ENC") => Sym.SM4_CBC_Enc(data, key, iv),
                ("SM4", "CBC", "DEC") => Sym.SM4_CBC_Dec(data, key, iv),

                ("SM4", "ECB", "ENC") => Sym.SM4_ECB_Enc(data, key),
                ("SM4", "ECB", "DEC") => Sym.SM4_ECB_Dec(data, key),

                ("AES", "CBC", "ENC") => Sym.AES128_CBC_Enc(data, key, iv),
                ("AES", "CBC", "DEC") => Sym.AES128_CBC_Dec(data, key, iv),

                ("AES", "ECB", "ENC") => Sym.AES128_ECB_Enc(data, key),
                ("AES", "ECB", "DEC") => Sym.AES128_ECB_Dec(data, key),
                _ => throw new NotImplementedException($""),
            };

            output.WriteBytes(new FormBytes(result, DumpForm.Hex));
        }
    }
}
