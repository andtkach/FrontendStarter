namespace Jiwebapi.Catalog.Domain.Message.Event
{
    public class EventUpdated : BaseEvent
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
