namespace Jiwebapi.Catalog.Domain.Message.Event
{
    public class EventDeleted : BaseEvent
    {
        public Guid EventId { get; set; }
    }
}
