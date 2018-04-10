using Model.Fhrs;
using Model.Transport;
using Moq;
using Services.Api;
using Services.Api.Interface;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Xunit;

namespace UnitTests.Services
{
    public class HttpClientServiceTests
    {
        private Mock<MockMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;

        public HttpClientServiceTests()
        {
            _mockHttpMessageHandler = new Mock<MockMessageHandler> { CallBase = true };
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        }

        [Fact]
        public void HttpClientService_Ctor()
        {
            _mockHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<Establishment>(new Establishment { }, new JsonMediaTypeFormatter())
            });

            IHttpClientService httpClientService = new HttpClientService("http://api.ratings.food.gov.uk", "Establishment", _httpClient);
            Assert.NotNull(httpClientService);
        }

        [Fact]
        public void HttpClientService_GetSingleAsync_ReturnsErrorObject_WhenInvalidHttpResponse()
        {
            _mockHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Error encountered during processing")
            });

            IHttpClientService httpClientService = new HttpClientService("http://api.ratings.food.gov.uk", "Establishment", _httpClient);
            HttpClientResponse<Establishment> response = httpClientService.GetSingleAsync<Establishment>().Result;

            Assert.NotNull(response);
            Assert.Equal(404, response.HttpStatusCode);
            Assert.Contains("Error encountered during processing", response.ErrorMessage);
            Assert.Null(response.ResponseContent);
        }

        [Fact]
        public void HttpClientService_GetSingleAsync_ReturnsDataFromValidHttpResponse()
        {
            _mockHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<Establishment>(new Establishment { RatingValue = "5", RatingKey = "fhrs_5_en-gb" }, new JsonMediaTypeFormatter())
            });

            IHttpClientService httpClientService = new HttpClientService("http://api.ratings.food.gov.uk", "Establishment", _httpClient);
            HttpClientResponse<Establishment> response = httpClientService.GetSingleAsync<Establishment>().Result;

            Assert.NotNull(response);
            Assert.Null(response.ErrorMessage);
            Assert.Equal(200, response.HttpStatusCode);
            Assert.NotNull(response.ResponseContent);
        }

        [Fact]
        public void HttpClientService_GetAsync_ReturnsErrorObject_WhenInvalidHttpResponse()
        {
            _mockHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Error encountered during processing")
            });

            var apiHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("x-api-version", "2")
            };

            IHttpClientService httpClientService = new HttpClientService("http://api.ratings.food.gov.uk", "Establishment", _httpClient);
            HttpClientResponse<Establishment> response = httpClientService.GetAsync<Establishment>(apiHeaders).Result;

            Assert.NotNull(response);
            Assert.Equal(404, response.HttpStatusCode);
            Assert.Contains("Error encountered during processing", response.ErrorMessage);
            Assert.Null(response.ResponseContent);
        }

        [Fact]
        public void HttpClientService_GetAsync_ReturnsDataFromValidHttpResponse()
        {
            _mockHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<Establishment>(new Establishment { RatingValue = "5", RatingKey = "fhrs_5_en-gb" }, new JsonMediaTypeFormatter())
            });

            var apiHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("x-api-version", "2")
            };

            IHttpClientService httpClientService = new HttpClientService("http://api.ratings.food.gov.uk", "Establishment", _httpClient);
            HttpClientResponse<Establishment> response = httpClientService.GetAsync<Establishment>(apiHeaders).Result;

            Assert.NotNull(response);
            Assert.Equal(200, response.HttpStatusCode);
            Assert.NotNull(response.ResponseContent);
            Assert.Null(response.ErrorMessage);
        }
    }
}
