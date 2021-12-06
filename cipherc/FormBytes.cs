namespace cipherc
{
    /// <summary>
    /// 带有类型信息的bytes
    /// </summary>
    public class FormBytes : Bytes, IBytes
    {
        public DumpForm Form { get; }

        public FormBytes(byte[] byteArray, DumpForm form) : base(byteArray)
        {
            Form = form;
        }

        public string Dump(DumpForm? form)
        {
            return ByteArray.Dump(form ?? Form);
        }
    }
}
