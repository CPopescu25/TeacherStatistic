using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TS.Core.Api
{
    /// <summary>
    /// Specific implementation for JSON Http API
    /// </summary>
    public class JsonApiRequestService : GenericApiRequestService, IJsonApiRequestService
    {
        #region Fields

        private const string JsonMediaType = "application/json";

        /// <summary>
        /// Whether to use form url encoded or not
        /// </summary>
        private bool _useFormEncoded = true;


        #endregion

        /// <summary>
        /// Build the HttpClient instance based on media type formatter
        /// </summary>
        /// <param name="baseUri">Base Url for the requests</param>
        /// <param name="useFormEncoded">Marker to use form url encoded for body POST/PUT requests</param>
        /// <returns></returns>
        public override void MakeHttpClient(string baseUri, bool? useFormEncoded = null)
        {
            _useFormEncoded = useFormEncoded.GetValueOrDefault();
            base.MakeHttpClient(baseUri, new MediaTypeWithQualityHeaderValue(JsonMediaType));
        }

        /// <summary>
        /// Put a file on a request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public override Task<TResponse> PutFileAsync<TResponse>(object model, string url, string fileName,
            string paramName)
        {
            throw new NotSupportedException("Not supported");
        }

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected override async Task<TResponse> CustomReadDataAsync<TResponse>(HttpResponseMessage response, string url)
        {
            var responseText = string.Empty;
            try
            {
                responseText = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<TResponse>(responseText.Trim(),
                    new JsonSerializerSettings { ContractResolver = new CustomDateContractResolver() });
                if (model == null)
                {
                    //no response body
                    return new TResponse()
                    {
                        Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                        Success = false,
                        UserMessage =
                            string.Format("No response body content. {0} says: {1} {2}", url, response.StatusCode,
                                response.ReasonPhrase)

                    };
                }

                return model;
            }
            catch (Exception ex)
            {
                var res = new TResponse()
                {
                    Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                    Success = false,
                    UserMessage =
                        string.Format("{0} - error on response decode: {1}. Error message:  {2}", url,
                            responseText.Trim(),
                            ex)
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
        protected override async Task<IEnumerable<TResponse>> CustomReadListAsync<TResponse>(
            HttpResponseMessage response,
            string url)
        {
            var responseText = string.Empty;
            try
            {
                responseText = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IEnumerable<TResponse>>(responseText.Trim(),
                    new JsonSerializerSettings { ContractResolver = new CustomDateContractResolver() });
                if (model == null)
                {
                    //no response body
                    return new List<TResponse>()
                    {
                        new TResponse()
                        {
                            Status = ((int) HttpStatusCode.InternalServerError).ToString(),
                            Success = false,
                            UserMessage =
                                string.Format("No response body content. {0} says: {1} {2}", url, response.StatusCode,
                                    response.ReasonPhrase)

                        }
                    };
                }
                return model;
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
                            string.Format("{0} - error on response decode: {1}. Error message:  {2}", url,
                                responseText.Trim(),
                                ex)
                    }
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
            var responseText = string.Empty;
            try
            {
                responseText = await response.Content.ReadAsStringAsync();
                return responseText;
            }
            catch (Exception ex)
            {
                return string.Format("{0} - error on response decode: {1}. Error message:  {2}", url,
                    responseText.Trim(),
                    ex);
            }
        }

        /// <summary>
        /// Prepare data to be send
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected override HttpContent PrepareContent(object model, string paramName)
        {
            return _useFormEncoded ? (HttpContent)CreateFormUrlEncodedContent(model, paramName) : CreateContent(model);
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

            return await PostAsync(url, content);
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

            return await PutAsync(url, content);
        }


        #region Utilities

        protected HttpContent CreateFormUrlEncodedContent(object model, string paramName)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            //var formModel = new[]
            //{
            //    new KeyValuePair<string, string>(paramName, jsonModel)
            //};

            //var jsonContent = new StringContent(JsonConvert.SerializeObject(formModel), Encoding.UTF8, JsonMediaType);

            var content = new LargeFormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(paramName, jsonModel)
            });
            //var content = new FormUrlEncodedContent(new[]
            //    {
            //        new KeyValuePair<string, string>(paramName, jsonModel)
            //    });

            return content;
        }

        protected StringContent CreateContent(object model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, JsonMediaType);

            return content;
        }

        /// <summary>
        /// The expected response content type.
        /// When app is responding in text/html instead of application/json
        /// </summary>
        protected override string ExpectedContentType
        {
            get { return JsonMediaType; }
            set { }
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
