using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Client.Interfaces
{
    public class ClientRequest:IClient
    {
        protected HttpClient HttpClient;
        protected HttpContent HttpContent;
        protected HttpResponseMessage response;
       
       
        public HttpClient MakeHttpClient(string baseUri,string mediaType)
        {
            HttpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            HttpClient.DefaultRequestHeaders.Accept.Clear();

            if(mediaType !=null)
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return HttpClient;
        }

        public HttpContent MakeHttpContent(object model)
        {
            HttpContent = new StringContent(JsonConvert.SerializeObject(model),
                Encoding.UTF8, "application/json");
            return HttpContent;
        }

        public string PostAsync(string baseUri,object model,string route)
        {
            string mediaType=null;
            response = MakeHttpClient(baseUri,mediaType).PostAsync(route, MakeHttpContent(model)).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return null;
            
        }

        public string GetAsync(string baseUri, string route)
        {
            string mediaType = "application/json";
            response = MakeHttpClient(baseUri, mediaType).GetAsync(route).Result;

            if (response.IsSuccessStatusCode)
                return response.Content.ReadAsStringAsync().Result;
            else
                return null;
            
        }

        
    }
}