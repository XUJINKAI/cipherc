namespace cipherc
{
    /// <summary>
    /// 带有类型信息的bytes
    /// </summary>
    public class FormBytes : Bytes, IBytes
    {
        public DumpForm? Form { get; protected set; }

        public FormBytes(byte[] byteArray, DumpForm form) : base(byteArray)
        {
            Form = form;
        }

        public string Dump(DumpForm? form, string? eol = null)
        {
            form ??= Form;
            eol ??= ENV.EndOfLine;
            var bytes = ByteArray;
            return form switch
            {
                DumpForm.Hex => $"{bytes.ToHexString()}",
                DumpForm.Hex_CStr => $"\\x{bytes.ToHexString("\\x")}",
                DumpForm.Base64 => $"{bytes.ToBase64String()}",
                DumpForm.Base64_Endline => $"{bytes.ToBase64String(eol)}",
                DumpForm.HexDump => bytes.ToHexDumpText(eol),
                DumpForm.UTF8 => ENV.UTF8Encoder.Decode(bytes),
                DumpForm.GBK => ENV.GBKEncoder.Decode(bytes),
                _ => throw new NotImplementedException($"Unknown BytesForm {form}."),
            };
        }
    }

    public static class FormBytesExtension
    {
        public static FormBytes ToFormBytes(this byte[] bytes, DumpForm form) => new FormBytes(bytes, form);
    }
}
