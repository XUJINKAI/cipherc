using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using libcipherc.Utils;
using Org.BouncyCastle.Asn1;

namespace libcipherc.ASN1
{
    public class DerParser
    {
        public string Dump(Bytes data)
        {
            var dumper = new DumpHelper(0, 0, 0);
            var obj = Asn1Object.FromByteArray(data);
            DumpAsn1Object(dumper, 0, obj);
            return dumper.ToString();
        }

        private void DumpAsn1Object(DumpHelper dumper, int level, Asn1Object obj)
        {
            dumper.SetLineLevel(0, level);
            switch (obj)
            {
                case DerSequence sequence:
                    dumper.SetLineContent(0, $"SEQUENCE");
                    dumper.SubmitLine();
                    foreach (Asn1Object item in sequence)
                    {
                        DumpAsn1Object(dumper, level + 1, item);
                    }
                    break;
                case DerSet set:
                    dumper.SetLineContent(0, $"SET");
                    dumper.SubmitLine();
                    foreach (Asn1Object item in set)
                    {
                        DumpAsn1Object(dumper, level + 1, item);
                    }
                    break;
                case DerBitString bitString:
                    dumper.SetLineContent(0, $"BIT STRING");
                    try
                    {
                        var subtry = Asn1Object.FromByteArray(bitString.GetBytes());
                        dumper.SubmitLine();
                        DumpAsn1Object(dumper, level + 1, subtry);
                    }
                    catch
                    {
                        dumper.SetLineContent(1, $"{bitString.GetBytes().ToHexString()}");
                        dumper.SubmitLine();
                    }
                    break;
                case DerOctetString octetString:
                    dumper.SetLineContent(0, $"OCTET STRING");
                    try
                    {
                        var subtry = Asn1Object.FromByteArray(octetString.GetOctets());
                        dumper.SubmitLine();
                        DumpAsn1Object(dumper, level + 1, subtry);
                    }
                    catch
                    {
                        var octets = octetString.GetOctets();
                        dumper.SetLineContent(1, $"{(octets.IsPrintableAscii() ? $"\"{octets.ToAsciiString()}\"" : octets.ToHexString())}");
                        dumper.SubmitLine();
                    }
                    break;
                case DerTaggedObject taggedObject:
                    dumper.SetLineContent(0, $"[]");
                    dumper.SubmitLine();
                    DumpAsn1Object(dumper, level + 1, taggedObject.GetObject());
                    break;
                case DerObjectIdentifier oid:
                    dumper.SetLineContent(0, $"OBJECT IDENTIFIER");
                    var friendlyName = Oids.GetFrindlyName(oid.Id);
                    dumper.SetLineContent(1, $"{oid.Id} {(friendlyName == null ? "" : $"({friendlyName})")}");
                    dumper.SubmitLine();
                    break;
                case DerPrintableString printableString:
                    dumper.SetLineContent(0, $"PrintableString");
                    dumper.SetLineContent(1, $"{printableString}");
                    dumper.SubmitLine();
                    break;
                case DerBoolean boolean:
                    dumper.SetLineContent(0, $"BOOLEAN");
                    dumper.SetLineContent(1, $"{boolean.IsTrue}");
                    dumper.SubmitLine();
                    break;
                case DerInteger integer:
                    dumper.SetLineContent(0, "INTERGET");
                    dumper.SetLineContent(1, $"{integer.Value.ToByteArray().ToHexString()}");
                    dumper.SubmitLine();
                    break;
                case DerUtcTime utcTime:
                    dumper.SetLineContent(0, "UTCTIME");
                    dumper.SetLineContent(1, $"{utcTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss zz")} ({utcTime.GetEncoded().ToHexString()})");
                    dumper.SubmitLine();
                    break;
                case DerNull:
                    dumper.SetLineContent(0, $"NULL");
                    dumper.SubmitLine();
                    break;
                default:
                    dumper.SetLineContent(0, $"{obj.GetType().Name}");
                    dumper.SubmitLine();
                    break;
            }
        }

        private string Print(object obj, int level)
        {
            if (obj is not IEnumerable c)
            {
                return new string(' ', level * 4) + obj.ToString() ?? "";
            }

            IEnumerator enumerator = c.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(Print(enumerator.Current, level + 1));
            while (enumerator.MoveNext())
            {
                sb.AppendLine(Print(enumerator.Current, level + 1));
            }

            return sb.ToString();
        }
    }
}
