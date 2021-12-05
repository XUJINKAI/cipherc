using libcipherc.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libciphercTest.Crypto_Test
{
    public class Sym_Test : TestBase
    {
        public Sym_Test(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData("00000000000000000000000000000000", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "00000000000000000000000000000000",
            "9F1F7BFF6F5511384D9430531E538FD3683AB2DE89DA899F5F9C02CE9EBAC2FD")]
        [InlineData("00000000000000000000000000000000112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000",
            "00000000000000000000000000000000",
            "9F1F7BFF6F5511384D9430531E538FD37C9C84F978683CF68DD362CC7C560AF1")]
        public void SM4_CBC_Enc(string data, string key, string iv, string expect)
        {
            var act_enc = Sym.SM4_CBC_Enc(HEX(data), HEX(key), HEX(iv));
            OutputLine(expect);
            OutputLine(act_enc.ToHexString());
            Assert.Equal(HEX(expect), act_enc);
        }


        [Theory]
        [InlineData("000102030405060708090A0B0C0D0E0F", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "00000000000000000000000000000000")]
        [InlineData("000102030405060708090A0B0C0D0E0F112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000",
            "00000000000000000000000000000000")]
        public void SM4_CBC_EncDec(string data, string key, string iv)
        {
            var enc_res = Sym.SM4_CBC_Enc(HEX(data), HEX(key), HEX(iv));
            var dec_res = Sym.SM4_CBC_Dec(enc_res, HEX(key), HEX(iv));
            OutputLine(enc_res.ToHexString());
            OutputLine(dec_res.ToHexString());
            Assert.Equal(HEX(data), dec_res);
        }


        [Theory]
        [InlineData("10101010101010101010101010101010", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "A83F90CC9F35CAC4DAF66BFA071C4182A83F90CC9F35CAC4DAF66BFA071C4182")]
        public void SM4_ECB_Enc(string data, string key, string expect)
        {
            var act_enc = Sym.SM4_ECB_Enc(HEX(data), HEX(key));
            OutputLine(expect);
            OutputLine(act_enc.ToHexString());
            Assert.Equal(HEX(expect), act_enc);
        }


        [Theory]
        [InlineData("000102030405060708090A0B0C0D0E0F112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000")]
        public void SM4_ECB_EncDec(string data, string key)
        {
            var enc_res = Sym.SM4_ECB_Enc(HEX(data), HEX(key));
            var dec_res = Sym.SM4_ECB_Dec(enc_res, HEX(key));
            OutputLine(enc_res.ToHexString());
            OutputLine(dec_res.ToHexString());
            Assert.Equal(HEX(data), dec_res);
        }


        [Theory]
        [InlineData("00000000000000000000000000000000", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "00000000000000000000000000000000",
            "66E94BD4EF8A2C3B884CFA59CA342B2E9434DEC2D00FDAC765F00C0C11628CD1")]
        [InlineData("00000000000000000000000000000000112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000",
            "00000000000000000000000000000000",
            "66E94BD4EF8A2C3B884CFA59CA342B2EF845F26CD54A6019BAF809C88A75B361")]
        public void AES128_CBC_Enc(string data, string key, string iv, string expect)
        {
            var act_enc = Sym.AES128_CBC_Enc(HEX(data), HEX(key), HEX(iv));
            OutputLine(expect);
            OutputLine(act_enc.ToHexString());
            Assert.Equal(HEX(expect), act_enc);
        }


        [Theory]
        [InlineData("000102030405060708090A0B0C0D0E0F", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "00000000000000000000000000000000")]
        [InlineData("000102030405060708090A0B0C0D0E0F112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000",
            "00000000000000000000000000000000")]
        public void AES128_CBC_EncDec(string data, string key, string iv)
        {
            var enc_res = Sym.AES128_CBC_Enc(HEX(data), HEX(key), HEX(iv));
            var dec_res = Sym.AES128_CBC_Dec(enc_res, HEX(key), HEX(iv));
            OutputLine(enc_res.ToHexString());
            OutputLine(dec_res.ToHexString());
            Assert.Equal(HEX(data), dec_res);
        }


        [Theory]
        [InlineData("10101010101010101010101010101010", // 10101010101010101010101010101010
            "00000000000000000000000000000000",
            "0143DB63EE66B0CDFF9F69917680151E0143DB63EE66B0CDFF9F69917680151E")]
        public void AES128_ECB_Enc(string data, string key, string expect)
        {
            var act_enc = Sym.AES128_ECB_Enc(HEX(data), HEX(key));
            OutputLine(expect);
            OutputLine(act_enc.ToHexString());
            Assert.Equal(HEX(expect), act_enc);
        }


        [Theory]
        [InlineData("000102030405060708090A0B0C0D0E0F112233445566", // 0A0A0A0A0A0A0A0A0A0A
            "00000000000000000000000000000000")]
        public void AES128_ECB_EncDec(string data, string key)
        {
            var enc_res = Sym.AES128_ECB_Enc(HEX(data), HEX(key));
            var dec_res = Sym.AES128_ECB_Dec(enc_res, HEX(key));
            OutputLine(enc_res.ToHexString());
            OutputLine(dec_res.ToHexString());
            Assert.Equal(HEX(data), dec_res);
        }

    }
}
