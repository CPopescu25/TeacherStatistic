using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TS.Core.Api
{
    public class XmlApiRequestService : GenericApiRequestService, IXmlApiRequestService
    {
        #region Fields
        private const string XmlMediaType = "application/xml";

        #endregion

        /// <summary>
        /// Build the HttpClient instance based on media type formatter
        /// </summary>
        /// <param name="baseUri">Base Url for the requests</param>
        /// <param name="useFormEncoded">Marker to use form url encoded for body POST/PUT requests</param>
        /// <returns></returns>
        public override void MakeHttpClient(string baseUri, bool? useFormEncoded = null)
        {
            base.MakeHttpClient(baseUri, new MediaTypeWithQualityHeaderValue(XmlMediaType));
        }

        /// <summary>
        /// Put a file on a request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public override async Task<TResponse> PutFileAsync<TResponse>(object model, string url, string fileName, string paramName)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var boundary = Guid.NewGuid().ToString().Replace("-", "");

            using (var content = new MultipartFormDataContent(boundary))
            {
                var requestSerializer = new System.Xml.Serialization.XmlSerializer(model.GetType());
                var writer = new Utf8StringWriter();
                requestSerializer.Serialize(writer, model);

                //Content-Disposition: form-data; name="json"
                var stringContent = new StringContent(writer.ToString() + Environment.NewLine + "--" + boundary + "--");
                stringContent.Headers.ContentType.MediaType = "text/xml";
                stringContent.Headers.Add("Content-Disposition", string.Format("form-data; name=\"{0}\"", paramName));
                content.Add(stringContent, paramName);

                var fileContentString = System.IO.File.ReadAllText(fileName);
                byte[] byt = System.Text.Encoding.UTF8.GetBytes(fileContentString);

                // convert the byte array to a Base64 string

                //var fileContent = new StringContent(System.Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName)));
                var fileContent = new StringContent(Convert.ToBase64String(byt));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "\"" + Path.GetFileName(fileName) + "\"",
                    Name = "\"inputStream\""
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                fileContent.Headers.Add("Content-Transfer-Encoding", "base64");

                content.Add(fileContent);

                var response = await HttpClient.PutAsync(url, content).ConfigureAwait(false);
                var responseStream = await response.Content.ReadAsStreamAsync();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TResponse));
                return (TResponse)serializer.Deserialize(responseStream);
            }

        }



        #region Utilities
        /// <summary>
        /// The expected response content type.
        /// </summary>
        protected override string ExpectedContentType
        {
            get
            {
                return XmlMediaType;
            }
            set { }
        }

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override async Task<TResponse> CustomReadDataAsync<TResponse>(HttpResponseMessage response, string url)
        {
            try
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                //var serializer = new XmlSerializer(typeof(TResponse));
                var serializer = XmlSerializer.FromTypes(new[] { typeof(TResponse) })[0];
                //var serializer = new System.Xml.Serialization.XmlSerializer(typeof(GalaxyResponse));
                var data = (TResponse)serializer.Deserialize(responseStream);

                return data;
            }
            catch (Exception ex)
            {
                var res = new TResponse()
                {
                    Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                    Success = false,
                    UserMessage =
                        string.Format("{0} - error on response decode. Error message:  {1}", url,
                            ex.ToString())
                };
                return res;
            }

        }

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override async Task<string> CustomReadStringAsync(HttpResponseMessage response, string url)
        {
            try
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(string));
                var data = (string)serializer.Deserialize(responseStream);

                return data;
            }
            catch (Exception ex)
            {

                return string.Format("{0} - error on response decode. Error message:  {1}", url,
                            ex.ToString());
            }

        }

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override async Task<IEnumerable<TResponse>> CustomReadListAsync<TResponse>(HttpResponseMessage response,
            string url)
        {
            try
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(IEnumerable<TResponse>));
                var data = (IEnumerable<TResponse>)serializer.Deserialize(responseStream);

                return data;
            }
            catch (Exception ex)
            {
                var res = new List<TResponse>()
                {
                    new TResponse()
                    {
                        Status = ((int) HttpStatusCode.InternalServerError).ToString(),
                        Success = false,
                        UserMessage =
                            string.Format("{0} - error on response decode. Error message:  {1}", url,
                                ex.ToString())
                    }
                };
                return res;
            }
        }

        /// <summary>
        /// Prepare data to be send
        /// Used just for testing...
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected override HttpContent PrepareContent(object model, string paramName)
        {
            if (model == null)
            {
                return null;
            }
            var sb = new StringBuilder();
            using (var tw = new StringWriter(sb))
            {
                var xmlS = new System.Xml.Serialization.XmlSerializer(model.GetType());
                xmlS.Serialize(tw, model);
                string serialized = sb.ToString();
            }

            return null;
        }

        /// <summary>
        /// Post the content on url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> PostAsync(string url, object model, string paramName)
        {
            var content = PrepareContent(model, paramName);

            return await PostAsXmlAsync(url, model);
        }

        /// <summary>
        /// Put the content on url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> PutAsync(string url, object model, string paramName)
        {
            var content = PrepareContent(model, paramName);

            return await PutAsXmlAsync(url, model);
        }

        #endregion

        #region IDisposable implementation
        private bool _disposed = false;

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //

            _disposed = true;
            // Call base class implementation.
            base.Dispose(disposing);
        }

        #endregion IDisposable Members

        #endregion

    }
}
