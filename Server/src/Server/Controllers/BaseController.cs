using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Services.Cache.Interface;

namespace Server.Controllers
{
    /// <summary>
    /// BaseController to hold common functionality that all API controller may need to leverage.
    /// </summary>
    public class BaseController : Controller
    {
        // Assume caching is important.
        private readonly ICacheService _cacheService;
        protected readonly IOptions<ApiConfigurationOption> ConfigurationOptions;

        /// <summary>
        /// Creates a new instance of the BaseController.
        /// </summary>
        /// <param name="cacheService">Cache Service to to be used.</param>
        /// <param name="configurationOptions">Configuration to to be used.</param>
        public BaseController(ICacheService cacheService, IOptions<ApiConfigurationOption> configurationOptions)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            ConfigurationOptions = configurationOptions ?? throw new ArgumentNullException(nameof(configurationOptions));
        }

        /// <summary>
        /// Gets data from either the current cache, or the delegate passed as
        /// a function.
        /// </summary>
        /// <typeparam name="T">Type of data being retrieved, implied from function wrapper.</typeparam>
        /// <param name="key">The key that is to be used to get or set the cache data.</param>
        /// <param name="getFunction">The delegate to be called to get data from the original data source.</param>
        /// <param name="area">The area the that is calling the cache, allowing for greater storage complexity.</param>
        /// <returns></returns>
        protected T GetOrAddToCache<T>(string key, Func<T> getFunction, string area)
            where T : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (getFunction == null)
            {
                throw new ArgumentNullException(nameof(getFunction));
            }

            T retrievedValue = _cacheService.GetCacheValue<T>(key, area);
            if (retrievedValue != null)
            {
                return retrievedValue;
            }

            T retrieved = getFunction();

            if (retrieved != null)
            {
                _cacheService.PutCacheValue(key, retrieved, area);
                return retrieved;
            }

            return null;
        }

        /// <summary>
        /// Invalidates the cache for the supplied key and area.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="area"></param>
        protected void InvalidateCache(string key, string area)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (area == null)
            {
                throw new ArgumentNullException(nameof(area));
            }

            _cacheService.RemoveCacheValue(key, area);
        }

        /// <summary>.
        /// Generates a standard key from the data type and associated id field.
        /// </summary>
        /// <param name="type">type key</param>
        /// <param name="id">id key.</param>
        /// <returns></returns>
        protected string GenerateCacheKey(string type, int id)
        {
            return $"{type}:{id}";
        }

        /// <summary>
        /// Returns an enumeration of KVP's that contain api header information.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<KeyValuePair<string, string>> GetApiHeaders()
        {
            var apiHeaders = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("x-api-version", "2")
            };

            return apiHeaders;
        }
    }
}