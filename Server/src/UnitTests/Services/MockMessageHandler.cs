using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class MockMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException("This method will be mocked by Moq, only need to handle send as this is called internally by the HttpClient");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
