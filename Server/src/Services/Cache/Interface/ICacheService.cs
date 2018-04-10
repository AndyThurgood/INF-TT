namespace Services.Cache.Interface
{
    /// <summary>
    /// Interface contract to be used by all cache services.
    /// Expected usage, Object cache or service (Redis).
    /// </summary>
    public interface ICacheService
    {
        object GetCacheValue(string key, string area);
        T GetCacheValue<T>(string key, string area) where T : class;
        void PutCacheValue(string key, object value, string area);
        void RemoveCacheValue(string key, string area);
    }
}
