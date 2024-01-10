using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Cache;
using Jiwebapi.Catalog.Domain.Common;
using Jiwebapi.Catalog.History;
using Jiwebapi.Catalog.History.Entity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Jiwebapi.Catalog.Api.BackgroundServices
{
    public class RequestHistoryService : BackgroundService
    {
        private readonly ILogger<TimeCacheService> _logger;
        private readonly IServiceProvider _serviceProvider;
        
        public RequestHistoryService(ILogger<TimeCacheService> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"HistoryService started");

            using var scope = _serviceProvider.CreateScope();
            var historyReceiver = scope.ServiceProvider.GetRequiredService<HistoryReceiver>();
            var historyService = scope.ServiceProvider.GetRequiredService<PublicHistoryService>();

            var channelWithPattern = new RedisChannel(historyReceiver.ChannelName, RedisChannel.PatternMode.Pattern);

            await historyReceiver.Subscriber.SubscribeAsync(channelWithPattern, async (channel, message) =>
            {
                _logger.LogInformation($"Received history message: {message}");

                if (!string.IsNullOrEmpty(message))
                {
                    var entry = JsonConvert.DeserializeObject<PublicEntry>(message!);
                    if (entry != null)
                    {
                        await historyService.CreateAsync(entry);
                    }
                }
            });

        }
    }
}