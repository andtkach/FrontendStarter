using Jiwebapi.Catalog.Domain.Message.Category;
using Jiwebapi.Catalog.History;
using Jiwebapi.Catalog.History.Entity;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jiwebapi.Catalog.Message.Consumers.Category;

public class CategoryCreatedConsumer : IConsumer<CategoryCreated>
{
    private readonly ILogger<CategoryCreatedConsumer> _logger;
    private readonly PrivateHistoryService _historyService;

    public CategoryCreatedConsumer(ILogger<CategoryCreatedConsumer> logger, PrivateHistoryService historyService)
    {
        _logger = logger;
        _historyService = historyService;
    }

    public async Task Consume(ConsumeContext<CategoryCreated> context)
    {
        _logger.LogInformation($"--> Consuming {context.Message.GetType().Name} and {context.Message.DataTraceId} data trace id");

        var entry = new PrivateEntry()
        {
            Date = DateTime.UtcNow,
            User = context.Message.UserId,
            Action = context.Message.GetType().Name,
            Data = JsonConvert.SerializeObject(context.Message),
            DataTraceId = context.Message.DataTraceId
        };
        
        await _historyService.CreateAsync(entry);

        _logger.LogInformation($"<-- Consumed {context.Message.GetType().Name}");
    }
}
