using Microsoft.Extensions.DependencyInjection;
using Services.Api;
using Services.Api.Interface;
using Services.Cache;
using Services.Cache.Interface;

namespace Services.Startup
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService>(new LocalCacheService());
            services.AddSingleton<IHttpClientServiceFactory>(new HttpClientServiceFactory());
            return services;
        }
    }
}
