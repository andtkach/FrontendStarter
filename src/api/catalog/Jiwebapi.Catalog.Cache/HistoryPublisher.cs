using Jiwebapi.Catalog.Application.Contracts.Cache;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jiwebapi.Catalog.Application.Models;

namespace Jiwebapi.Catalog.Cache
{
    public class HistoryPublisher : IHistoryPublisher
    {
        private readonly ISubscriber _subscriber;
        
        public HistoryPublisher(IConnectionMultiplexer redis)
        {
            _subscriber = redis.GetSubscriber();
            ChannelName = Constants.ChannelName;
        }

        public string ChannelName { get; }

        public async Task Publish(string message)
        {
            RedisChannel channelWithPattern = new RedisChannel(ChannelName, RedisChannel.PatternMode.Pattern);
            await _subscriber.PublishAsync(channelWithPattern, message);
        }
    }
}
