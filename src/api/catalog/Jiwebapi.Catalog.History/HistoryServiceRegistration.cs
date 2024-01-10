using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jiwebapi.Catalog.History
{
    public static class HistoryServiceRegistration
    {
        public static IServiceCollection AddHistoryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("HistoryDatabase"));
            services.AddSingleton<PublicHistoryService>();
            services.AddSingleton<PrivateHistoryService>();
            return services;    
        }
    }
}
