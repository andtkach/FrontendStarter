using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Message.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Jiwebapi.Catalog.Message.Consumers;
using Jiwebapi.Catalog.Message.Services;

namespace Jiwebapi.Catalog.Message
{
    public static class MessageServiceRegistration
    {
        public static IServiceCollection AddMessageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MessageDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("MessageConnectionString"), 
                    b => b.MigrationsAssembly(typeof(MessageDbContext).Assembly.FullName));
            });

            services.AddScoped(typeof(IMessageRepository<>), typeof(MessageRepository<>));
            services.AddScoped<IPublishService, PublishService>();

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<MessageDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(10);

                    o.UsePostgres();
                    o.UseBusOutbox();
                });

                x.AddConsumersFromNamespaceContaining<BaseConsumer>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("message", false));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                    {
                        r.Handle<RabbitMqConnectionException>();
                        r.Interval(5, TimeSpan.FromSeconds(10));
                    });

                    cfg.Host(configuration["RabbitMq:Host"], configuration["RabbitMq:Vhost"], host =>
                    {
                        host.Username(configuration.GetValue("RabbitMq:Username", "guest"));
                        host.Password(configuration.GetValue("RabbitMq:Password", "guest"));
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;    
        }
    }
}
