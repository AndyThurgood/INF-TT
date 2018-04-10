using System.Collections.Generic;

namespace Services.Api.Interface
{
    public interface IHttpClientServiceFactory
    {
        IHttpClientService Create(string serviceAddress, string endpoint);
        IHttpClientService Create(string serviceAddress, string endpoint, IEnumerable<KeyValuePair<string, string>> headers);
    }
}
