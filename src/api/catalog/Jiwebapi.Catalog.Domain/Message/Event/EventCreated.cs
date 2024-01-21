namespace Jiwebapi.Catalog.Domain.Message.Event
{
    public class EventCreated : BaseEvent
    {
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
