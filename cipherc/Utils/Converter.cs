using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Web;

namespace CipherTool.Utils
{
    public static class Converter
    {
        public static string UrlEncode(Data data)
        {
            var result = HttpUtility.UrlEncode(data);
            string result_upper = Regex.Replace(result, @"%[a-f\d]{2}", m => m.Value.ToUpper());
            return result_upper;
        }

        public static Data UrlDecode(string encoded)
        {
            Contract.Assert(encoded != null);
            return HttpUtility.UrlDecodeToBytes(encoded);
        }
    }
}
