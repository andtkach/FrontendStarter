using Jiwebapi.Catalog.Application.Models;
using StackExchange.Redis;

namespace Jiwebapi.Catalog.Cache
{
    public class HistoryReceiver
    {
        public HistoryReceiver(IConnectionMultiplexer redis)
        {
            Subscriber = redis.GetSubscriber();
            ChannelName = Constants.ChannelName;
        }

        public string ChannelName { get; }

        public ISubscriber Subscriber { get; }
    }
}
