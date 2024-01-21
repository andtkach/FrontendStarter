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

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;
        private readonly ILogger<UpdateEventCommandHandler> _logger;

        public UpdateEventCommandHandler(IMapper mapper, IAsyncRepository<Event> eventRepository,
            ILoggedInUserService loggedInUserService, IPublishService publishService, ILogger<UpdateEventCommandHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
            _logger = logger;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateEventCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventToUpdate == null)
            {
                throw new NotFoundException(nameof(Event), request.EventId);
            }

            var validator = new UpdateEventCommandValidator();
            var validationResult = await validator.ValidateAsync(request ,cancellationToken);

            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult);

            _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));

            eventToUpdate.Person = _loggedInUserService.UserName;
            await _eventRepository.UpdateAsync(eventToUpdate);

            await _publishService.PublishEvent(new EventUpdated
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    Name = eventToUpdate.Name,
                    EventId = eventToUpdate.EventId,
                    DataTraceId = _loggedInUserService.DataTraceId,
            });
        }
    }
}