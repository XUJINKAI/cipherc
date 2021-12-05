using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;
using cipherc.Exceptions;
using libcipherc;

namespace cipherc.Utils
{
    public static class Converter
    {
        public static string UrlEncode(Bytes data)
        {
            var result = HttpUtility.UrlEncode(data);
            string result_upper = Regex.Replace(result, @"%[a-f\d]{2}", m => m.Value.ToUpper());
            return result_upper;
        }

        public static Bytes UrlDecode(string encoded)
        {
            Contract.Assert(encoded != null);
            return HttpUtility.UrlDecodeToBytes(encoded);
        }
    }
}
