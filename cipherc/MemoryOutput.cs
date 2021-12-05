using System;
using System.Collections.Generic;
using System.CommandLine.IO;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipherc
{
    internal class StandardMemoryWriter : IStandardStreamWriter
    {
        public StringBuilder StringBuilder { get; private set; }

        public StandardMemoryWriter(StringBuilder stringBuilder)
        {
            StringBuilder = stringBuilder;
        }

        public void Write(string value)
        {
            StringBuilder.Append(value);
        }
    }

    public class MemoryOutput : IOutput
    {
        public MemoryStream BytesBuilder { get; }
        public StringBuilder OutBuilder { get; }
        public StringBuilder ErrorBuilder { get; }

        public BinaryWriter RealByteStream { get; }
        public BinaryWriter? ByteStream
        {
            get => RealByteStream;
            set { /* IOutput接口允许外部修改，但实际应放到BytesBuilder里 */ }
        }
        public IList<DumpForm?> DumpForms { get; }
        public LogLevel LogLevel { get; set; }

        public IStandardStreamWriter Out { get; }
        public bool IsOutputRedirected => true;
        public IStandardStreamWriter Error { get; }
        public bool IsErrorRedirected => true;
        public bool IsInputRedirected => true;

        public MemoryOutput()
        {
            BytesBuilder = new MemoryStream();
            OutBuilder = new StringBuilder();
            ErrorBuilder = new StringBuilder();
            Out = new StandardMemoryWriter(OutBuilder);
            Error = new StandardMemoryWriter(ErrorBuilder);

            DumpForms = new List<DumpForm?>();
            RealByteStream = new BinaryWriter(BytesBuilder);
        }

        public byte[] GetByteResult() => BytesBuilder.ToArray();
        public string GetOutResult() => OutBuilder.ToString();
        public string GetErrorResult() => ErrorBuilder.ToString();

        public void Close()
        {
        }
    }
}
