using System.Collections.Concurrent;
using Services.Cache.Interface;

namespace Services.Cache
{
    /// <summary>
    /// Concurrenty dictionary version of the cache service.
    /// Typically used for local runs of the application via IOC.
    /// </summary>
    public class LocalCacheService : ICacheService
    {
        private ConcurrentDictionary<string, object> _cache;

        /// <summary>
        /// Get an object from the local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="area">Area that the key belongs.</param>
        public object GetCacheValue(string key, string area)
        {
            return GetCacheValue(GenerateKey(key, area));
        }

        /// <summary>
        /// Gets an entry from the local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="area">Area that the key belongs.</param>
        public T GetCacheValue<T>(string key, string area) where T : class
        {
            return GetCacheValue(GenerateKey(key, area)) as T;
        }

        /// <summary>
        /// Adds a value to the local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="value">The object to be stored.</param>
        /// <param name="area">Area that the key belongs.</param>
        public void PutCacheValue(string key, object value, string area)
        {
            PutCacheValue(GenerateKey(key, area), value);
        }

        /// <summary>
        /// Removes an entry from the local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="area">Area that the key belongs.</param>
        public void RemoveCacheValue(string key, string area)
        {
            GetCache().TryRemove(GenerateKey(key, area), out _);
        }

        /// <summary>
        /// Adds a value to the local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="value">The object to be stored.</param>
        private void PutCacheValue(string key, object value)
        {
            GetCache().AddOrUpdate(key, value, (keyValue, oldValue) => value);
        }

        /// <summary>
        /// Gets an entry fromt eh local cache.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <returns>Object.</returns>
        private object GetCacheValue(string key)
        {
            GetCache().TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Generates a key to be used as a reference.
        /// </summary>
        /// <param name="key">Key to be searched.</param>
        /// <param name="area">Area that the key belongs.</param>
        /// <returns></returns>
        private string GenerateKey(string key, string area)
        {
            return $"{area}:{key}";
        }

        /// <summary>
        /// Gets the cache, or creates the cache.
        /// </summary>
        /// <returns>A concurrent dictionary for caching.</returns>
        private ConcurrentDictionary<string, object> GetCache()
        {
            return _cache ?? (_cache = new ConcurrentDictionary<string, object>());
        }
    }
}
