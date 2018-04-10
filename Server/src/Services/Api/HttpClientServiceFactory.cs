using System.Collections.Generic;
using System.Net.Http;
using Services.Api.Interface;

namespace Services.Api
{
    public class HttpClientServiceFactory : IHttpClientServiceFactory
    {
        public IHttpClientService Create(string serviceAddress, string endpoint)
        {
            return new HttpClientService(serviceAddress, endpoint, new HttpClient(), null);
        }

        public IHttpClientService Create(string serviceAddress, string endpoint, IEnumerable<KeyValuePair<string, string>> headers)
        {
            return new HttpClientService(serviceAddress, endpoint, new HttpClient(), headers);
        }
    }
}
