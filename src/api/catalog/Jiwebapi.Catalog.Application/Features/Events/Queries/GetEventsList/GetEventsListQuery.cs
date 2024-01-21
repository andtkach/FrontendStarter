using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventsList
{
    public class GetEventsListQuery: IRequest<EventListVmResponse>
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
