using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api
{
    public class LargeFormUrlEncodedContent : ByteArrayContent
    {
        public LargeFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
            : base(LargeFormUrlEncodedContent.GetContentByteArray(nameValueCollection))
        {
            base.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        }

        private static byte[] GetContentByteArray(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException("nameValueCollection");
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> current in nameValueCollection)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('&');
                }

                stringBuilder.Append(LargeFormUrlEncodedContent.Encode(current.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(LargeFormUrlEncodedContent.Encode(current.Value));
            }
            return Encoding.Default.GetBytes(stringBuilder.ToString());
        }

        private static string Encode(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            return System.Net.WebUtility.UrlEncode(data).Replace("%20", "+");
        }
    }
}
