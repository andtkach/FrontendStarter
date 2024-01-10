using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Message;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Exceptions;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Domain.Entities;
using Jiwebapi.Catalog.Domain.Message.Event;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;
        private readonly ILogger<DeleteEventCommandHandler> _logger;

        public DeleteEventCommandHandler(IMapper mapper, IAsyncRepository<Event> eventRepository,
        ILoggedInUserService loggedInUserService, IPublishService publishService, ILogger<DeleteEventCommandHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteEventCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var eventToDelete = await _eventRepository.GetByIdAsync(request.EventId);

            if (eventToDelete == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            await _eventRepository.DeleteAsync(eventToDelete);

            await _publishService.PublishEvent(new EventDeleted
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    EventId = eventToDelete.EventId,
                    DataTraceId = _loggedInUserService.DataTraceId,
            });
        }
    }
}
