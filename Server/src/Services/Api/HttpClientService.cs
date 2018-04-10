using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Model.Transport;
using Services.Api.Interface;

namespace Services.Api
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _httpClient;
        private string _endpoint;
        private const string MediaType = "application/json";
        private bool _disposed;


        public HttpClientService(string serviceAddress, string endpoint, HttpClient httpClient, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromHours(1);
            _httpClient.BaseAddress = new Uri(serviceAddress) ?? throw new ArgumentNullException(nameof(serviceAddress));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }
        
        public async Task<HttpClientResponse<T>> GetSingleAsync<T>()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_endpoint).ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return await GenerateErrorResponse<T>(responseMessage);
            }

            return new HttpClientResponse<T>
            {
                HttpStatusCode = (int)responseMessage.StatusCode,
                IsSuccessful = true,
                ResponseContent = await responseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false)
            };
        }

        public async Task<HttpClientResponse<T>> GetAsync<T>(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            string param = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"{_endpoint}?{param}").ConfigureAwait(false);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return await GenerateErrorResponse<T>(responseMessage);
            }

            return new HttpClientResponse<T>
            {
                HttpStatusCode = (int)responseMessage.StatusCode,
                IsSuccessful = true,
                ResponseContent = await responseMessage.Content.ReadAsAsync<T>().ConfigureAwait(false)
            };

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_httpClient != null)
                {
                    HttpClient client = _httpClient;
                    _httpClient = null;
                    client.Dispose();
                }
                _disposed = true;
            }
        }
        
        private static async Task<HttpClientResponse<T>> GenerateErrorResponse<T>(HttpResponseMessage responseMessage)
        {
            string errorResponse = "Unsuccessful message request : " + (int)responseMessage.StatusCode + " (" +
                                   responseMessage.StatusCode + "). " + Environment.NewLine +
                                          await responseMessage.Content.ReadAsStringAsync();

            return new HttpClientResponse<T>
            {
                HttpStatusCode = (int)responseMessage.StatusCode,
                IsSuccessful = false,
                ErrorMessage = errorResponse,
                ResponseContent = default(T)
            };
        }
    }
}
