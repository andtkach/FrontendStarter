using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Cache.Cache;
using Jiwebapi.Catalog.Cache.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Jiwebapi.Catalog.Cache
{
    public static class CacheServiceRegistration
    {
        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContentCache, ContentCache>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("ContentCache:CacheDatabase");
            });
            
            services.AddSingleton<IConnectionMultiplexer>(opt => 
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("StorageConnectionString") ?? string.Empty,
                    options =>
                    {
                        options.AbortOnConnectFail = false;
                    }));

            services.AddScoped<ISimpleStorageService, SimpleStorageService>();
            services.AddScoped<IObjectStorageService, ObjectStorageService>();

            services.AddSingleton<IHistoryPublisher, HistoryPublisher>();
            services.AddSingleton<HistoryReceiver>();

            return services;    
        }
    }
}
