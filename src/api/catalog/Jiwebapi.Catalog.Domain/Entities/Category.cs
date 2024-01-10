using Jiwebapi.Catalog.Domain.Common;

namespace Jiwebapi.Catalog.Domain.Entities
{
    public class Category: AuditableEntity
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Event>? Events { get; set; }
        public Guid UserId { get; set; }
    }
}
