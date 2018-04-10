using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Fhrs;
using Moq;
using Services.Cache;
using Services.Cache.Interface;
using System;
using Xunit;

namespace Server.Controllers.Fhrs
{
    public class BaseControllerTests
    {
        private readonly IOptions<ApiConfigurationOption> _configurationOptions;
        
        public BaseControllerTests()
        {
            _configurationOptions = Options.Create(new ApiConfigurationOption
            {
                ServiceAddress = "http://api.ratings.food.gov.uk",
                CacheKey = "FHRS",
                EstablishmentEndpoint = "Establishments",
                AuthorityEndpoint = "authorities/basic"
            });
        }

        [Fact]
        public void BaseController_Ctor_ThrowsArguementException_Null_CacheService()
        {
            Assert.Throws<System.ArgumentNullException>(() => new BaseController(null, _configurationOptions));
        }

        [Fact]
        public void BaseController_Ctor_ThrowsArgumentException_Null_ConfigurationOptions()
        {
            Assert.Throws<System.ArgumentNullException>(() => new BaseController(new LocalCacheService(), null));
        }

        [Fact]
        public void BaseController_GetOrAddToCache_ThrowsArgumentException_Null_Key()
        {
            BaseControllerWrapper baseController = new BaseControllerWrapper(new LocalCacheService(), _configurationOptions);
            Assert.Throws<ArgumentNullException>(() => baseController.TestGetOrAddToCache(null, () => GetEstablishment(), "area"));
        }

        [Fact]
        public void BaseController_GetOrAddToCache_GetsValueFromCacheIfExists()
        {
            Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
            cacheServiceMock.Setup(x => x.GetCacheValue<Establishment>(It.IsAny<string>(), It.IsAny<string>())).Returns(GetEstablishment());
           
            BaseControllerWrapper baseController = new BaseControllerWrapper(cacheServiceMock.Object, _configurationOptions);
            Establishment establishment = baseController.TestGetOrAddToCache("key", () => GetEstablishment(), "area");

            Assert.NotNull(establishment);
            cacheServiceMock.Verify(x => x.GetCacheValue<Establishment>(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void BaseController_GetOrAddToCache_GetsValueFromDelegateIfNotExists()
        {
            Mock<ICacheService> cacheServiceMock = new Mock<ICacheService>(MockBehavior.Strict);
            cacheServiceMock.Setup(x => x.GetCacheValue<Establishment>(It.IsAny<string>(), It.IsAny<string>())).Returns(()=> null);
            cacheServiceMock.Setup(x => x.PutCacheValue(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()));

            BaseControllerWrapper baseController = new BaseControllerWrapper(cacheServiceMock.Object, _configurationOptions);
            Establishment establishment = baseController.TestGetOrAddToCache("key", () => GetEstablishment(), "area");

            Assert.NotNull(establishment);
            cacheServiceMock.Verify(x => x.GetCacheValue<Establishment>(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            cacheServiceMock.Verify(x => x.PutCacheValue(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<string>()), Times.Once);
        }

        private Establishment GetEstablishment()
        {
            return new Establishment { RatingKey = "fhrs_5_en-gb", RatingValue = "5" };
        }
    }

    /// <summary>
    /// Wraper for the BaseController class to allow for testing of protected methods
    /// Alt could be to make methids internal, or use reflection - yuck :(
    /// </summary>
    public class BaseControllerWrapper: BaseController
    {
        public BaseControllerWrapper(ICacheService cacheService, IOptions<ApiConfigurationOption> configurationOptions) : base(cacheService, configurationOptions) {

        }

        public T TestGetOrAddToCache<T>(string key, Func<T> getFunction, string area) where T : class
        {
            return base.GetOrAddToCache<T>(key, getFunction, area);
        }

        public void TestInvalidateCache(string key, string area) {
            base.InvalidateCache(key, area);
        }


    }
}


