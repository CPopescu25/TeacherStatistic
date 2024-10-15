using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TS.Core.Api.Data;

namespace TS.Core.Api
{
    /// <summary>
    /// Base class for Http API service implementation
    /// </summary>
    public abstract class GenericApiRequestService : IApiRequestService
    {
        #region Fields

        /// <summary>
        /// HttpClient
        /// </summary>
        protected HttpClient HttpClient;

        #endregion

        #region Initialization

        /// <summary>
        /// Build the HttpClient instance based on media type formatter
        /// </summary>
        /// <param name="baseUri">Base Url for the requests</param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        protected void MakeHttpClient(string baseUri, MediaTypeWithQualityHeaderValue mediaType)
        {
            if (mediaType == null)
            {
                throw new ArgumentNullException("mediaType");
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Check if we already have a HttpClient
            if (HttpClient != null)
            {
                var hc = HttpClient;
                HttpClient = null;
                hc.Dispose();
            }

            if (string.IsNullOrWhiteSpace(baseUri))
            {
                //fake uri to avoid throwing unwanted exceptions
                baseUri = "http://no.ip";
            }

            HttpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(mediaType);
            //httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));
            HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(new ProductHeaderValue("Gvd_HttpClient", "1.0")));
        }

        #endregion

        #region IApiRequestService implementation
        /// <summary>
        /// Build the HttpClient instance based on media type formatter
        /// </summary>
        /// <param name="baseUri">Base Url for the requests</param>
        /// <param name="useFormEncoded">Marker to use form url encoded for body POST/PUT requests</param>
        /// <returns></returns>
        public abstract void MakeHttpClient(string baseUri, bool? useFormEncoded = null);

        /// <summary>
        /// Place a HTTP Get request on a given url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<TResponse> GetAsync<TResponse>(string url) where TResponse : IDefaultResponse, new()
        {
            var response = await GetAsync<TResponse>(url, null);

            return response;
        }

        /// <summary>
        /// Place a HTTP Get request on a given url using a collection of header strings
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<TResponse> GetAsync<TResponse>(string url, HeaderContent header)
            where TResponse : IDefaultResponse, new()
        {
            //add header entries to the request, if any
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    var content = header.Content[key];

                    HttpClient.DefaultRequestHeaders.Add(key, content);
                });
            }

            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await HttpClient.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();

                //return await responseMessage.Content.ReadAsAsync<TResource>();
                return await RetrieveDataFromRequest<TResponse>(responseMessage, url);
            }
            catch (Exception ex)
            {
                return new TResponse()
                {
                    Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                    Success = false,
                    UserMessage = string.Format("An internal error occurred: {0}", ex)
                };
            }
            finally
            {
                PrepareResponseHeaders(responseMessage, header);
            }
        }

        /// <summary>
        /// Place a HTTP Get afile content request on a given url using a collection of header strings
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<byte[]> GetFileAsync(string url, HeaderContent header)
        {
            //add header entries to the request, if any
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    var content = header.Content[key];

                    HttpClient.DefaultRequestHeaders.Add(key, content);
                });
            }

            try
            {
                var responseMessage = await HttpClient.GetByteArrayAsync(url);

                //return await responseMessage.Content.ReadAsAsync<TResource>();
                return responseMessage;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Post data on a HTTP request and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(object model, string url, string paramName) where TResponse : IDefaultResponse, new()
        {
            var response = await PostAsync<TResponse>(model, url, paramName, null);

            return response;
        }

        /// <summary>
        /// Post data on a HTTP request using a collection of header strings and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<TResponse> PostAsync<TResponse>(object model, string url, string paramName, HeaderContent header) where TResponse : IDefaultResponse, new()
        {
            //add header entries to the request, if any
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    var content = header.Content[key];

                    HttpClient.DefaultRequestHeaders.Add(key, content);
                });
            }

            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await PostAsync(url, model, paramName);

                //responseMessage.EnsureSuccessStatusCode();

                //return await responseMessage.Content.ReadAsAsync<TResource>();
                return await RetrieveDataFromRequest<TResponse>(responseMessage, url);
            }
            catch (Exception ex)
            {
                return new TResponse()
                {
                    Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                    Success = false,
                    UserMessage = string.Format("An internal error occurred: {0}", ex)
                };
            }
            finally
            {
                PrepareResponseHeaders(responseMessage, header);
            }
        }

        /// <summary>
        /// PUT data on a HTTP request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<TResponse> PutAsync<TResponse>(object model, string url, string paramName) where TResponse : IDefaultResponse, new()
        {
            var response = await PutAsync<TResponse>(model, url, paramName, null);

            return response;
        }

        /// <summary>
        /// PUT data on a HTTP request using a collection of header strings and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<TResponse> PutAsync<TResponse>(object model, string url, string paramName, HeaderContent header) where TResponse : IDefaultResponse, new()
        {
            //add header entries to the request, if any
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    var content = header.Content[key];

                    HttpClient.DefaultRequestHeaders.Add(key, content);
                });
            }

            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await PutAsync(url, model, paramName);

                //responseMessage.EnsureSuccessStatusCode();

                //return await responseMessage.Content.ReadAsAsync<TResource>();
                return await RetrieveDataFromRequest<TResponse>(responseMessage, url);
            }
            catch (Exception ex)
            {
                return new TResponse()
                {
                    Status = ((int)HttpStatusCode.InternalServerError).ToString(),
                    Success = false,
                    UserMessage = string.Format("An internal error occurred: {0}", ex)
                };
            }
            finally
            {
                PrepareResponseHeaders(responseMessage, header);
            }
        }

        /// <summary>
        /// Put a file on a request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public abstract Task<TResponse> PutFileAsync<TResponse>(object model, string url, string fileName,
            string paramName) where TResponse : IDefaultResponse, new();

        /// <summary>
        /// Make a DELETE request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public async Task<string> DeleteAsync(string url, HeaderContent header)
        {
            //add header entries to the request, if any
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    var content = header.Content[key];

                    HttpClient.DefaultRequestHeaders.Add(key, content);
                });
            }

            HttpResponseMessage responseMessage = null;
            try
            {
                responseMessage = await HttpClient.DeleteAsync(url);

                //responseMessage.EnsureSuccessStatusCode();

                //return await responseMessage.Content.ReadAsAsync<TResource>();
                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return string.Format("An internal error occurred: {0}", ex);
            }
            finally
            {
                PrepareResponseHeaders(responseMessage, header);
            }
        }


        #endregion

        #region Utilities


        /// <summary>
        /// The expected response content type.
        /// Applications may respond in text/html instead of application/json or application/xml
        /// </summary>
        protected abstract string ExpectedContentType { get; set; }

        public class Utf8StringWriter : StringWriter
        {
            // Use UTF8 encoding but write no BOM to the wire
            public override Encoding Encoding
            {
                get { return new UTF8Encoding(false); } // in real code I'll cache this encoding.
            }
        }

        private void RemoveCustomHeaders(HeaderContent header)
        {
            if (header != null && header.Content != null)
            {
                header.Content.Keys.ForEach(key =>
                {
                    if (HttpClient.DefaultRequestHeaders.Contains(key))
                    {
                        HttpClient.DefaultRequestHeaders.Remove(key);
                    }
                });
            }
        }

        /// <summary>
        /// Convert data from response to the desired objects
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<TResponse> ReadDataAsync<TResponse>(HttpResponseMessage response, string url)
            where TResponse : IDefaultResponse, new()
        {
            //In case of the content type text/html instead of application/json
            if (response.Content.Headers.ContentType.MediaType != ExpectedContentType)
            {
                //Just customize data recovery in this case
                return await CustomReadDataAsync<TResponse>(response, url);
            }
            return await CustomReadDataAsync<TResponse>(response, url);

            //var data = await response.Content.ReadAsStreamAsync();
            //var serializer = new XmlSerializer(typeof(TResponse));
            //return (TResponse)serializer.Deserialize(data);


            //Standard data recovery
            //return await response.Content.ReadAsAsync<TResponse>(new List<MediaTypeFormatter>(){new XmlMediaTypeFormatter()});

        }

        /// <summary>
        /// Convert data from response to the desired objects
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<IEnumerable<TResponse>> ReadDataListAsync<TResponse>(HttpResponseMessage response, string url)
            where TResponse : IDefaultResponse, new()
        {
            //In case of the content type is text/html instead of application/json
            if (response.Content.Headers.ContentType.MediaType != ExpectedContentType)
            {
                //Just customize data recovery in this case
                return await CustomReadListAsync<TResponse>(response, url);
            }

            //Standard data recovery
            return await response.Content.ReadAsAsync<IEnumerable<TResponse>>();

        }

        /// <summary>
        /// Convert data from response to the desired objects
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<string> ReadStringAsync(HttpResponseMessage response, string url)
        {
            //In case of the content type is text/html instead of application/json
            if (response.Content.Headers.ContentType.MediaType != ExpectedContentType ||
                ExpectedContentType != "application/json")
            {
                //Just customize data recovery in this case
                return await CustomReadStringAsync(response, url);
            }

            //Standard data recovery
            return await response.Content.ReadAsAsync<string>();

        }

        /// <summary>
        /// Convert data from response to the desired objects
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<IEnumerable<TResponse>> ReadListAsync<TResponse>(HttpResponseMessage response, string url)
            where TResponse : IDefaultResponse, new()
        {
            //In case of the content type is text/html instead of application/json
            if (response.Content.Headers.ContentType.MediaType != ExpectedContentType)
            {
                //Just customize data recovery in this case
                return await CustomReadListAsync<TResponse>(response, url);
            }

            //Standard data recovery
            return await response.Content.ReadAsAsync<IEnumerable<TResponse>>();

        }

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected abstract Task<TResponse> CustomReadDataAsync<TResponse>(HttpResponseMessage response, string url)
            where TResponse : IDefaultResponse, new();

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected abstract Task<string> CustomReadStringAsync(HttpResponseMessage response, string url);

        /// <summary>
        /// Custom implementation of data deserialization
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected abstract Task<IEnumerable<TResponse>> CustomReadListAsync<TResponse>(HttpResponseMessage response,
            string url)
            where TResponse : IDefaultResponse, new();

        /// <summary>
        /// Prepare data to be send
        /// </summary>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected abstract HttpContent PrepareContent(object model, string paramName);

        /// <summary>
        /// Post the content on url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected abstract Task<HttpResponseMessage> PostAsync(string url, object model, string paramName);

        /// <summary>
        /// Delegate function
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await HttpClient.PostAsync(url, content);
        }

        /// <summary>
        /// Delegate function
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PostAsXmlAsync(string url, object content)
        {
            return await HttpClient.PostAsXmlAsync(url, content);
        }

        /// <summary>
        /// Put the content on url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected abstract Task<HttpResponseMessage> PutAsync(string url, object model, string paramName);

        /// <summary>
        /// Delegate function
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await HttpClient.PutAsync(url, content);
        }

        /// <summary>
        /// Delegate function
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> PutAsXmlAsync(string url, object content)
        {
            return await HttpClient.PutAsXmlAsync(url, content);
        }

        /// <summary>
        /// Prepare response headers if we have any
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="header"></param>
        protected void PrepareResponseHeaders(HttpResponseMessage responseMessage, HeaderContent header)
        {
            if (responseMessage != null && header != null && header.Content != null)
            {
                var responses = new Dictionary<string, string>();
                header.Content.Keys.ForEach(key =>
                {
                    if (responseMessage.Headers.Contains(key))
                    {
                        IEnumerable<string> headerRes;
                        if (responseMessage.Headers.TryGetValues(key, out headerRes))
                        {
                            responses[key] = headerRes.Aggregate((crt, next) => crt + next + ";");
                        }
                    }
                });
                responses.Keys.ForEach(key =>
                {
                    header.Content[key] = responses[key];
                });
            }
            RemoveCustomHeaders(header);

        }

        /// <summary>
        /// Extract data from request
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<TResponse> RetrieveDataFromRequest<TResponse>(HttpResponseMessage responseMessage,
            string url)
            where TResponse : IDefaultResponse, new()
        {
            TResponse response = default(TResponse);

            if (responseMessage.StatusCode != HttpStatusCode.NoContent)
            {
                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = new DefaultResponse()
                    {
                        Status = response.Status,
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase
                    };
                    try
                    {
                        response = await ReadDataAsync<TResponse>(responseMessage, url);
                        //response.Success = true;
                    }
                    catch (Exception ex)
                    {
                        throw new TSCoreApiException()
                        {
                            HttpResponseCode = responseMessage.StatusCode,
                            HttpErrorCode = response == null ? HttpStatusCode.BadRequest.ToString() : "",
                            HttpErrorMessage = response == null ? error.DeveloperMessage : ex.Message
                        };

                    }
                }
                else
                {
                    response = await ReadDataAsync<TResponse>(responseMessage, url);
                    response.Success = true;

                }
            }
            else
            {
                response = new TResponse()
                {
                    Status = response.Status,
                    Success = false,
                    DeveloperMessage = responseMessage.ReasonPhrase,
                    UserMessage = "Response returned no content"
                };

            }

            return response;
        }


        /// <summary>
        /// Extract data from request
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<TResponse>> RetrieveDataListFromRequest<TResponse>(
            HttpResponseMessage responseMessage,
            string url)
            where TResponse : IDefaultResponse, new()
        {
            IEnumerable<TResponse> response = new List<TResponse>();

            if (responseMessage.StatusCode != HttpStatusCode.NoContent)
            {
                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = new DefaultResponse()
                    {
                        Status = ((int)responseMessage.StatusCode).ToString(),
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase
                    };
                    try
                    {
                        response = await ReadDataListAsync<TResponse>(responseMessage, url);

                    }
                    catch (Exception ex)
                    {
                        throw new TSCoreApiException()
                        {
                            HttpResponseCode = responseMessage.StatusCode,
                            HttpErrorCode = response == null ? HttpStatusCode.BadRequest.ToString() : "",
                            HttpErrorMessage = response == null ? error.DeveloperMessage : ex.Message
                        };

                    }
                }
                else
                {
                    response = await ReadDataListAsync<TResponse>(responseMessage, url);
                }
            }
            else
            {
                response = new List<TResponse>
                {
                    new TResponse()
                    {
                        Status = ((int) responseMessage.StatusCode).ToString(),
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase,
                        UserMessage = "Response returned no content"
                    }
                };

            }

            return response;
        }

        /// <summary>
        /// Extract data from request
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<string> RetrieveStringFromRequest(HttpResponseMessage responseMessage, string url)
        {
            string response = null;

            if (responseMessage.StatusCode != HttpStatusCode.NoContent)
            {
                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = new DefaultResponse()
                    {
                        Status = ((int)responseMessage.StatusCode).ToString(),
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase
                    };
                    try
                    {
                        response = await ReadStringAsync(responseMessage, url);

                    }
                    catch (Exception ex)
                    {
                        throw new TSCoreApiException()
                        {
                            HttpResponseCode = responseMessage.StatusCode,
                            HttpErrorCode = response == null ? HttpStatusCode.BadRequest.ToString() : "",
                            HttpErrorMessage = response == null ? error.DeveloperMessage : ex.Message
                        };

                    }
                }
                else
                {
                    response = await ReadStringAsync(responseMessage, url);
                }
            }
            else
            {
                return "Response returned no content";
            }

            return response;
        }

        /// <summary>
        /// Extract data from request
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<TResponse>> RetrieveListFromRequest<TResponse>(
            HttpResponseMessage responseMessage,
            string url)
            where TResponse : IDefaultResponse, new()
        {
            IEnumerable<TResponse> response = null;

            if (responseMessage.StatusCode != HttpStatusCode.NoContent)
            {
                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = new DefaultResponse()
                    {
                        Status = ((int)responseMessage.StatusCode).ToString(),
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase
                    };
                    try
                    {
                        response = await ReadListAsync<TResponse>(responseMessage, url);

                    }
                    catch (Exception ex)
                    {
                        throw new TSCoreApiException()
                        {
                            HttpResponseCode = responseMessage.StatusCode,
                            HttpErrorCode = response == null ? HttpStatusCode.BadRequest.ToString() : "",
                            HttpErrorMessage = response == null ? error.DeveloperMessage : ex.Message
                        };

                    }
                }
                else
                {
                    response = await ReadListAsync<TResponse>(responseMessage, url);
                }
            }
            else
            {
                response = new List<TResponse>
                {
                    new TResponse()
                    {
                        Status = ((int) responseMessage.StatusCode).ToString(),
                        Success = false,
                        DeveloperMessage = responseMessage.ReasonPhrase,
                        UserMessage = "Response returned no content"
                    }
                };
            }

            return response;
        }

        #endregion

        #region Implementation of IDisposable

        private bool _disposed;

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (HttpClient != null)
                {
                    var hc = HttpClient;
                    HttpClient = null;
                    hc.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion IDisposable Members

        #endregion

        #region Internal classes

        internal class CustomDateContractResolver : DefaultContractResolver
        {
            protected override JsonContract CreateContract(Type objectType)
            {
                JsonContract contract = base.CreateContract(objectType);
                bool b = objectType == typeof(DateTime?);
                if (b)
                {
                    contract.Converter = new ZerosIsoDateTimeConverter("yyyy-MM-dd HH:mm:ss", "0000-00-00",
                        "0000-00-00 00:00:00");
                }
                return contract;
            }
        }

        #endregion

    }

    /// <summary>
    /// Custom IsoDateTimeConverter for DateTime strings with zeros.
    /// 
    /// Usage Sample
    ///  [JsonConverter(typeof(ZerosIsoDateTimeConverter), "yyyy-MM-dd hh:mm:ss", "0000-00-00 00:00:00")]
    ///  public DateTime Zerodate { get; set; }
    /// </summary>
    public class ZerosIsoDateTimeConverter : Newtonsoft.Json.Converters.IsoDateTimeConverter
    {
        /// <summary>
        /// The string representing a datetime value with zeros. E.g. "0000-00-00 00:00:00"
        /// </summary>
        private readonly string _zeroDateString;
        private readonly string _zeroMinDateString = "0001-01-01";

        private readonly string _zeroDateTimeString;
        private readonly string _dateTimeFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZerosIsoDateTimeConverter"/> class.
        /// </summary>
        /// <param name="dateTimeFormat">The date time format.</param>
        /// <param name="zeroDateString">The zero date string. 
        /// Please be aware that this string should match the date time format.</param>
        /// <param name="zerDateTimeString"></param>
        public ZerosIsoDateTimeConverter(string dateTimeFormat, string zeroDateString, string zerDateTimeString)
        {
            DateTimeFormat = dateTimeFormat;
            _dateTimeFormat = dateTimeFormat;
            _zeroDateString = zeroDateString;
            _zeroDateTimeString = zerDateTimeString;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// If a DateTime value is DateTime.MinValue than the zeroDateString will be set as output value.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime && (DateTime)value == DateTime.MinValue)
            {
                value = _zeroDateTimeString;
                serializer.Serialize(writer, value);
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// If  an input value is same a zeroDateString than DateTime.MinValue will be set as return value
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return (DateTime?)null;
            }
            var value = reader.Value.ToString();
            if (value == _zeroDateString || value == _zeroMinDateString || value == _zeroDateTimeString || value.Length < 10)
            {
                return (DateTime?)null;
            }

            if (value.Length == 10 && objectType == typeof(DateTime?))
            {
                DateTimeFormat = "yyyy-MM-dd";
            }
            else
            {
                DateTimeFormat = _dateTimeFormat;
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
