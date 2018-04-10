using Model.Transport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Api.Interface
{
    public interface IHttpClientService : IDisposable
    {
        Task<HttpClientResponse<T>> GetSingleAsync<T>();
        Task<HttpClientResponse<T>> GetAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters);
    }
}
