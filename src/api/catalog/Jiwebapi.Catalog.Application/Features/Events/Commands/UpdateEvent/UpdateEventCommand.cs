using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.UpdateEvent
{
    public class UpdateEventCommand: IRequest
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Person { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
    }
}
