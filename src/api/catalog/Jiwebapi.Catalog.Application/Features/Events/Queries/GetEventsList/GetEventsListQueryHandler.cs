using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesList;
using Jiwebapi.Catalog.Domain.Entities;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQueryHandler : IRequestHandler<GetEventsListQuery, EventListVmResponse>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public GetEventsListQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        public async Task<EventListVmResponse> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var take = request.PageSize;
            var allEvents = (await _eventRepository.ListAllAsync()).OrderBy(x => x.EventId).Skip(skip).Take(take);
            var totalItems = await _eventRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);
            var result = _mapper.Map<List<EventListVm>>(allEvents);
            return new EventListVmResponse
            {
                Result = result,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
            };
        }
    }
}
