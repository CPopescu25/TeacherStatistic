using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Core.Api.Data;

namespace TS.Core.Api
{
    /// <summary>
    /// This class exposes RESTful CRUD functionality in a generic way, abstracting
    /// the implementation and useage details of HttpClient, HttpRequestMessage,
    /// HttpResponseMessage, ObjectContent, Formatters etc. 
    /// </summary>
    public interface IApiRequestService : IDisposable
    {
        /// <summary>
        /// Create the HttpClient object
        /// </summary>
        /// <param name="baseUri">Base URI</param>
        /// <param name="useFormEncoded">Whether to use form encoded for payload or not</param>
        void MakeHttpClient(string baseUri, bool? useFormEncoded = null);

        /// <summary>
        /// Place a HTTP Get request on a given url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<TResponse> GetAsync<TResponse>(string url)
                                where TResponse : IDefaultResponse, new();


        /// <summary>
        /// Place a HTTP Get request on a given url using a collection of header strings
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        Task<TResponse> GetAsync<TResponse>(string url, HeaderContent header)
                                where TResponse : IDefaultResponse, new();

        /// <summary>
        /// Place a HTTP Get afile content request on a given url using a collection of header strings
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        Task<byte[]> GetFileAsync(string url, HeaderContent header);

        /// <summary>
        /// Post data on a HTTP request and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<TResponse> PostAsync<TResponse>(object model, string url, string paramName)
                                where TResponse : IDefaultResponse, new();


        /// <summary>
        /// Post data on a HTTP request using a collection of header strings and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        Task<TResponse> PostAsync<TResponse>(object model, string url, string paramName, HeaderContent header)
                                where TResponse : IDefaultResponse, new();

        /// <summary>
        /// PUT data on a HTTP request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<TResponse> PutAsync<TResponse>(object model, string url, string paramName)
                                where TResponse : IDefaultResponse, new();


        /// <summary>
        /// PUT data on a HTTP request using a collection of header strings and recover data as response
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        Task<TResponse> PutAsync<TResponse>(object model, string url, string paramName, HeaderContent header)
                                where TResponse : IDefaultResponse, new();


        /// <summary>
        /// Put a file on a request
        /// </summary>
        /// <param name="model"></param>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<TResponse> PutFileAsync<TResponse>(object model, string url, string fileName, string paramName)
                                where TResponse : IDefaultResponse, new();


        /// <summary>
        /// Make a DELETE request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        Task<string> DeleteAsync(string url, HeaderContent header);

    }
}
