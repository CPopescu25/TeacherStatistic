using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    public interface IClient
    {
        HttpClient MakeHttpClient(string baseUri, string mediaType);

        HttpContent MakeHttpContent(object model);

        string PostAsync(string baseUri, object model, string route);

        string GetAsync(string baseUri, string route);

    }
}
