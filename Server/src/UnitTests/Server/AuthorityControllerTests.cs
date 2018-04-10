using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Fhrs;
using Model.Fhrs.Responses;
using Model.Transport;
using Moq;
using Server.Controllers.Fhrs;
using Services.Api;
using Services.Api.Interface;
using Services.Cache;
using Xunit;

namespace UnitTests.Server
{
    public class AuthorityControllerTests
    {
        private readonly Mock<IHttpClientServiceFactory> _httpClienServiceFactory;
        private readonly IOptions<ApiConfigurationOption> _configurationOptions;

        public AuthorityControllerTests()
        {
            _httpClienServiceFactory = new Mock<IHttpClientServiceFactory>(MockBehavior.Strict);
            _configurationOptions = Options.Create(new ApiConfigurationOption
            {
                ServiceAddress = "http://api.ratings.food.gov.uk",
                CacheKey = "FHRS",
                EstablishmentEndpoint = "Establishments",
                AuthorityEndpoint = "authorities/basic"
            });
        }

        [Fact]
        public void AuthorityController_Ctor_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AuthorityController(null, new LocalCacheService(), _configurationOptions));
        }

        [Fact]
        public void GetAuthorities_Returns_AuthorityList()
        {

            var httpClientMock = GetHttpClientMock(true);

            _httpClienServiceFactory.Setup(p => p.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
                .Returns(httpClientMock.Object);
            
            var result = new AuthorityController(_httpClienServiceFactory.Object, new LocalCacheService(), _configurationOptions).GetAuthorities();
            var contentResult = result as JsonResult;
            var dataResult = contentResult?.Value as List<Authority>;

            Assert.NotNull(result);
            Assert.NotNull(contentResult);
            Assert.NotNull(dataResult);
            Assert.Equal(5, dataResult.Count);
        }

        [Fact]
        public void GetAuthorities_Handles_InvalidResponse()
        {
            var httpClientMock = GetHttpClientMock(false);

            _httpClienServiceFactory.Setup(p =>p.Create(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
                .Returns(httpClientMock.Object);
               
            var result = new AuthorityController(_httpClienServiceFactory.Object, new LocalCacheService(), _configurationOptions).GetAuthorities();
            var contentResult = result as NotFoundObjectResult;
            Assert.NotNull(result);
            Assert.NotNull(contentResult);
        }

        private static Mock<IHttpClientService> GetHttpClientMock(bool isSuccesful)
        {
            Mock<IHttpClientService> httpClientMock = new Mock<IHttpClientService>(MockBehavior.Strict);
            httpClientMock.Setup(p => p.Dispose());

            httpClientMock.Setup(p => p.GetSingleAsync<AuthorityResponse>())
                .Returns(Task.Run(() => new HttpClientResponse<AuthorityResponse>
                {
                    IsSuccessful = isSuccesful,
                    ResponseContent = new AuthorityResponse
                    {
                        Authorities = new List<Authority>
                        {
                            new Authority
                            {
                                SchemeType = 2,
                                LocalAuthorityId = 196,
                                Name = "Aberdeenshire"
                            },
                            new Authority
                            {
                                SchemeType = 1,
                                LocalAuthorityId = 197,
                                Name = "Aberdeen city"
                            },
                            new Authority
                            {
                                SchemeType = 1,
                                LocalAuthorityId = 20,
                                Name = "London"
                            },
                            new Authority
                            {
                                SchemeType = 2,
                                LocalAuthorityId = 88,
                                Name = "Leeds"
                            },
                            new Authority
                            {
                                SchemeType = 2,
                                LocalAuthorityId = 213,
                                Name = "Calderdale"
                            }
                        }
                    }
                }));
            return httpClientMock;
        }
    }
}
