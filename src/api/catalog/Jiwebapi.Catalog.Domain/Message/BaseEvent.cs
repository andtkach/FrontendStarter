namespace Jiwebapi.Catalog.Domain.Message
{
    public class BaseEvent
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;

        public string? DataTraceId { get; set; }
    }
}
