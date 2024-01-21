using System.Threading.Channels;

namespace Jiwebapi.Catalog.Api.BackgroundServices
{
    public class ContentCacheProcessingChannel
    {
        private const int MaxMessagesInChannel = 100;

        private readonly Channel<string> _channel;
        private readonly ILogger<ContentCacheProcessingChannel> _logger;

        public ContentCacheProcessingChannel(ILogger<ContentCacheProcessingChannel> logger)
        {
            var options = new BoundedChannelOptions(MaxMessagesInChannel)
            {
                SingleWriter = false,
                SingleReader = true                
            };

            _channel = Channel.CreateBounded<string>(options);

            _logger = logger;
        }

        public async Task<bool> ProcessContentAsync(string contentItemId, string dataName, CancellationToken ct = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                string data = $"{dataName}|{contentItemId}";
                if (_channel.Writer.TryWrite(data))
                {
                    Log.ChannelMessageWritten(_logger, data);

                    return true;
                }
            }

            return false;
        }

        public IAsyncEnumerable<string> ReadAllAsync(CancellationToken ct = default) =>
            _channel.Reader.ReadAllAsync(ct);

        public bool TryCompleteWriter(Exception ex) => _channel.Writer.TryComplete(ex);

        internal static class EventIds
        {
            public static readonly EventId ChannelMessageWritten = new EventId(100, "ChannelMessageWritten");
        }

        private static class Log
        {
            private static readonly Action<ILogger, string, Exception?> ChannelAction = LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIds.ChannelMessageWritten,
                "ConfigurationId {configurationId} was written to the channel.");

            public static void ChannelMessageWritten(ILogger logger, string configurationId)
            {
                ChannelAction(logger, configurationId, null);
            }
        }   
    }
}