using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Fhrs;
using Model.Fhrs.Reports;
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
    public class EstablishmentControllerTests
    {
        private readonly Mock<IHttpClientServiceFactory> _httpClienServiceFactory;
        private readonly IOptions<ApiConfigurationOption> _configurationOptions;

        public EstablishmentControllerTests()
        {
            _httpClienServiceFactory = new Mock<IHttpClientServiceFactory>(MockBehavior.Strict);
            _configurationOptions = Options.Create(new ApiConfigurationOption
            {
                ServiceAddress = "http://api.ratings.food.gov.uk",
                CacheKey = "FHRS",
                EstablishmentEndpoint = "establishments",
                AuthorityEndpoint = "authorities/basic"
            });
        }

        [Fact]
        public void AuthorityController_Ctor_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new EstablishmentController(null, new LocalCacheService(), _configurationOptions));
        }

        [Fact]
        public void GetEstablishmentReport_Returns_ValidReport()
        {
            var httpClientMock = GetHttpClientMock(true);

            _httpClienServiceFactory.Setup(p => p.Create(It.IsAny<string>(), It.IsAny<string>(),It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
                .Returns(httpClientMock.Object);

            var result = new EstablishmentController(_httpClienServiceFactory.Object, new LocalCacheService(), _configurationOptions).GetEstablishmentReport(1, 1);
            var contentResult = result as JsonResult;
            var dataResult = contentResult?.Value as AuthorityReport;

            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Value);
            Assert.NotNull(dataResult);
            Assert.Equal(6, dataResult.TotalEstablishments);
        }

        [Fact]
        public void GetEstablishmentReport_Handles_InvalidReponse()
        {
            var httpClientMock = GetHttpClientMock(false);

            _httpClienServiceFactory.Setup(p => p.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
                .Returns(httpClientMock.Object);

            var result = new EstablishmentController(_httpClienServiceFactory.Object, new LocalCacheService(), _configurationOptions).GetEstablishmentReport(1, 1);
            var contentResult = result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(contentResult);
        }
        
        private static Mock<IHttpClientService> GetHttpClientMock(bool isSucessful)
        {
            Mock<IHttpClientService> httpClientMock = new Mock<IHttpClientService>(MockBehavior.Strict);
            httpClientMock.Setup(p => p.Dispose());

            httpClientMock.Setup(p => p.GetAsync<EstablishmentResponse>(It.IsAny<List<KeyValuePair<string, string>>>()))
                .Returns(Task.Run(() => new HttpClientResponse<EstablishmentResponse>
                {
                    IsSuccessful = isSucessful,
                    ResponseContent = new EstablishmentResponse
                    {
                        Establishments = new List<Establishment>
                        {
                            new Establishment
                            {
                                RatingValue = "5",
                                RatingKey = "fhrs_5_en-gb"
                            },
                            new Establishment
                            {
                                RatingValue = "5",
                                RatingKey = "fhrs_5_en-gb"
                            },
                            new Establishment
                            {
                                RatingValue = "5",
                                RatingKey = "fhrs_5_en-gb"
                            },
                            new Establishment
                            {
                                RatingValue = "4",
                                RatingKey = "fhrs_4_en-gb"
                            },
                            new Establishment
                            {
                                RatingValue = "4",
                                RatingKey = "fhrs_4_en-gb"
                            },
                            new Establishment
                            {
                                RatingValue = "4",
                                RatingKey = "fhrs_4_en-gb"
                            }
                        }
                    }
                }));
            return httpClientMock;
        }
    }
}
