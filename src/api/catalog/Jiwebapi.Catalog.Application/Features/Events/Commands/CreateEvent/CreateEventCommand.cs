using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommand: IRequest<CreateEventCommandResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string? Person { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public override string ToString()
        {
            return $"Event name: {Name}; By: {Person}; On: {Date.ToShortDateString()}; Description: {Description}";
        }
    }
}
