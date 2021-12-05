namespace ciphercTest;

public class Core_Test : TestBase
{
    public Core_Test(ITestOutputHelper output) : base(output) { }


    [Fact]
    public void Rand16_DumpHexBase64()
    {
        MockRandGenerator(() =>
        {
            var result = RunCommand("rand 16 --dump hex,base64");
            Assert.Equal($"78756A696E6B61692E63697068657263{EOL}eHVqaW5rYWkuY2lwaGVyYw=={EOL}", result.GetOutResult());
            Assert.Equal("xujinkai.cipherc".GetBytes(), result.GetByteResult());
        });
    }

    [Fact]
    public void Zero8()
    {
        var result = RunCommand("zero 8");
        var expectHex = "0000000000000000";
        Assert.Equal(expectHex + EOL, result.GetOutResult());
        Assert.Equal(HEX(expectHex), result.GetByteResult());
    }

    [Fact]
    public void Data()
    {
        var result = RunCommand("data -f 0000.bin --dump base64");
    }

    [Fact]
    public void Base64_Encode_Utf8Text()
    {
        var result = RunCommand("base64 cipherc");
        Assert.Equal("Y2lwaGVyYw==", result.GetOutResult());
        //Assert.Equal("cipherc", result.GetByteResult());
    }

    [Fact]
    public void Base64_Decode_AutoForm()
    {
        var result = RunCommand("base64 -d \"Y2lwaGVyYw==\"");
        Assert.Equal("[UTF8] cipherc" + EOL, result.GetOutResult());
        Assert.Equal("cipherc".GetBytes(), result.GetByteResult());
    }

    [Fact]
    public void Base64_EncodeFile()
    {
        var result = RunCommand("base64 -f cipherc.txt");
        Assert.Equal("Y2lwaGVyYw==" + EOL, result.GetOutResult());
    }

    [Fact]
    public void Base64_DecodeFile()
    {
        var result = RunCommand("base64 -d author.base64.txt");
        Assert.Equal("[UTF8] xujinkai" + EOL, result.GetOutResult());
        Assert.Equal("xujinkai".GetBytes(), result.GetByteResult());
    }

    [Fact]
    public void SM3_0000_file()
    {
        var result = RunCommand("sm3 -f 0000.bin");
        var expectHex = "AF83A966222057AC761246A7543C580D9111014F4E5E3CB1281DB33151160335";
        Assert.Equal(expectHex + EOL, result.GetOutResult());
        Assert.Equal(HEX(expectHex), result.GetByteResult());
    }

    [Fact]
    public void MD5_0000()
    {
        var result = RunCommand("md5 0000");
        var expectHex = "C4103F122D27677C9DB144CAE1394A66";
        Assert.Equal(expectHex + EOL, result.GetOutResult());
        Assert.Equal(HEX(expectHex), result.GetByteResult());
    }

    [Fact]
    public void SM4_Enc_0000_file()
    {
        var result = RunCommand("sm4 -d -K 0123456789ABCDEFFEDCBA9876543210 --iv 0 -f 0000.bin --dump utf8");
    }
}
