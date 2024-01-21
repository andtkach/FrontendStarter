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

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventCommandResponse>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly ILoggedInUserService _loggedInUserService;
        private readonly IPublishService _publishService;

        public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository, 
            ILogger<CreateEventCommandHandler> logger,
            ILoggedInUserService loggedInUserService, IPublishService publishService)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _logger = logger;
            _loggedInUserService = loggedInUserService;
            _publishService = publishService;
        }

        public async Task<CreateEventCommandResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CreateEventCommandHandler for {_loggedInUserService.DataTraceId} data trace id");

            var createEventCommandResponse = new CreateEventCommandResponse();

            var validator = new CreateEventCommandValidator(_eventRepository);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                createEventCommandResponse.Success = false;
                createEventCommandResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createEventCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (createEventCommandResponse.Success)
            {
                var newItem = _mapper.Map<Event>(request);
                newItem.Person = _loggedInUserService.UserName;
                newItem = await _eventRepository.AddAsync(newItem);
                createEventCommandResponse.Event = _mapper.Map<CreateEventDto>(newItem);

                await _publishService.PublishEvent(new EventCreated
                {
                    Id = Guid.NewGuid(),
                    UserId = _loggedInUserService.UserId,
                    Name = newItem.Name,
                    EventId = newItem.EventId,
                    DataTraceId = _loggedInUserService.DataTraceId,
                });
            }
            
            return createEventCommandResponse;
        }
    }
}