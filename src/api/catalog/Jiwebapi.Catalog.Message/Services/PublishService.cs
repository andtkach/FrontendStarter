using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message;
using Jiwebapi.Catalog.Message.Repositories;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Message.Services
{
    public class PublishService : IPublishService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMessageRepository<Log> _messageRepository;
        private readonly ILogger<PublishService> _logger;
        
        public PublishService(IPublishEndpoint publishEndpoint, IMessageRepository<Log> messageRepository, 
            ILogger<PublishService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<bool> PublishEvent<T>(T eventData) where T : BaseEvent
        {
            _logger.LogInformation($"PublishService for {eventData.GetType().Name} and {eventData.DataTraceId} data trace id");
            await _publishEndpoint.Publish(eventData);
            await _messageRepository.AddAsync(new Log { Content = $"Publish event {eventData.GetType().Name}", DataTraceId = eventData.DataTraceId });

            return true;
        }
    }
}
