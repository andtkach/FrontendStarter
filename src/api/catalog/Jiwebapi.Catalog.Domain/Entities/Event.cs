using Jiwebapi.Catalog.Domain.Common;

namespace Jiwebapi.Catalog.Domain.Entities
{
    public class Event: AuditableEntity
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Person { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;

    }
}
