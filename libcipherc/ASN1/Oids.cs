using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace libcipherc.ASN1
{
    public static class Oids
    {
        private static readonly Dictionary<string, string> OidNames = new()
        {
            { "1.2.156.10197.1.104.1", "sm4-ecb" },
            { "1.2.156.10197.1.104.2", "sm4-cbc" },
            { "1.2.156.10197.1.104.3", "sm4-ofb" },
            { "1.2.156.10197.1.104.4", "sm4-cfb" },
            { "1.2.156.10197.1.104.5", "sm4-cfb1" },
            { "1.2.156.10197.1.104.6", "sm4-cfb8" },
            { "1.2.156.10197.1.104.7", "sm4-ctr" },
        };

        public static string? GetFrindlyName(string oid)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            string? name = null;
            name ??= OidNames.ContainsKey(oid) ? OidNames[oid] : null;
            name ??= GetFrindlyNameByOidTxt(oid);
            name ??= new Oid(oid).FriendlyName;
            return name;
        }

        private static string? GetFrindlyNameByOidTxt(string oid)
        {
            using Stream? stream = Assembly.GetCallingAssembly().GetManifestResourceStream("libcipherc.Asn1.Oids.txt");
            if (stream == null)
            {
                return null;
            }

            using StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();

            var needFind = $"OID = {oid.Replace('.', ' ')}";
            var posOid = result.IndexOf(needFind);
            if (posOid < 0)
            {
                return null;
            }
            var posComment = result.IndexOf('\n', posOid) + 1;
            var posDescription = result.IndexOf('\n', posComment) + 1;
            var posEnd = result.IndexOf('\n', posDescription) + 1;

            var descText = "Description = ";
            var start = result.IndexOf(descText, posDescription) + descText.Length;
            if (start > 0 && start < posEnd)
            {
                return result.Substring(start, posEnd - start - 1).Trim();
            }
            return null;
        }
    }
}