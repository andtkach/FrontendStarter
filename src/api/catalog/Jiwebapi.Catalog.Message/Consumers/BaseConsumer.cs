using Jiwebapi.Catalog.Domain.Message;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Message.Consumers;

public class BaseConsumer : IConsumer<BaseEvent>
{
    private readonly ILogger<BaseConsumer> _logger;

    public BaseConsumer(ILogger<BaseConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BaseEvent> context)
    {
        _logger.LogInformation($"--> Consuming {context.Message.GetType().Name}");
        await Task.CompletedTask;
    }
}
