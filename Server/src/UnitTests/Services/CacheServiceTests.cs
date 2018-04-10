using Model.Fhrs.Reports;
using Services.Cache;
using Services.Cache.Interface;
using Xunit;

namespace UnitTests.Services
{
    public class CacheServiceTests
    {
        [Fact]
        public void StaticCacheHandler_Ctor()
        {
            ICacheService handler = new LocalCacheService();
            Assert.NotNull(handler);
        }

        [Fact]
        public void StaticCacheHandler_GetValueWithArea_NoExistingItems()
        {
            ICacheService handler = new LocalCacheService();
            object value = handler.GetCacheValue("LocalCacheService_GetValueWithRegion_NoExistingItems", "area1");
            Assert.Null(value);
        }

        [Fact]
        public void StaticCacheHandler_PutValueWithArea_NoExisting()
        {
            ICacheService handler = new LocalCacheService();
            handler.PutCacheValue("LocalCacheService_PutValueWithRegion_NoExisting", "value1", "area1");
        }

        [Fact]
        public void StaticCacheHandler_RetrieveStoredItem()
        {
            ICacheService handler = new LocalCacheService();
            handler.PutCacheValue("LocalCacheService_RetrieveStoredItem", "value1", "area1");
            object value = handler.GetCacheValue("LocalCacheService_RetrieveStoredItem", "area1");
            Assert.NotNull(value);
            Assert.Equal("value1", value);
        }

        [Fact]
        public void StaticCacheHandler_RetrieveOverwrittenStoredItem()
        {
            ICacheService handler = new LocalCacheService();
            handler.PutCacheValue("LocalCacheService_RetrieveOverwrittenStoredItem", "value1", "area1");
            handler.PutCacheValue("LocalCacheService_RetrieveOverwrittenStoredItem", "value2", "area1");
            object value = handler.GetCacheValue("LocalCacheService_RetrieveOverwrittenStoredItem", "area1");
            Assert.NotNull(value);
            Assert.Equal("value2", value);
        }

        [Fact]
        public void StaticCacheHandler_Get_None()
        {
            ICacheService handler = new LocalCacheService();

            object value = handler.GetCacheValue("key", "area");
            Assert.Null(value);
        }

        [Fact]
        public void StaticCacheHandler_Get_Found()
        {
            ICacheService handler = new LocalCacheService();

            handler.PutCacheValue("key", "val1", "area");

            object value = handler.GetCacheValue("key", "area");
            Assert.Equal("val1", value);
        }

        [Fact]
        public void StaticCacheHandler_GetWithType_Found()
        {
            ICacheService handler = new LocalCacheService();

            handler.PutCacheValue("key", "val1", "area");

            string value = handler.GetCacheValue<string>("key", "area");
            Assert.Equal("val1", value);
        }

        [Fact]
        public void StaticCacheHandler_GetWithDifferentType_Found()
        {
            ICacheService handler = new LocalCacheService();

            handler.PutCacheValue("key", "val1", "area");

            AuthorityReport site = handler.GetCacheValue<AuthorityReport>("key", "area");
            Assert.Null(site);
        }

        [Fact]
        public void StaticCacheHandler_RemoveCacheValue()
        {
            ICacheService handler = new LocalCacheService();

            handler.PutCacheValue("key", "val1", "area");

            handler.RemoveCacheValue("key", "area");

            object value = handler.GetCacheValue("key", "area");

            Assert.Null(value);
        }
    }
}
