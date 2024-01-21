namespace Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventsList
{
    public class EventListVmResponse
    {
        public required IEnumerable<EventListVm> Result { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
