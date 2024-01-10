using Jiwebapi.Catalog.Domain.Common;

namespace Jiwebapi.Catalog.Domain.Entities
{
    public class Log: AuditableEntity, IIdentifiableEntity
    {
        public Guid LogId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string? DataTraceId { get; set; }
    }
}
