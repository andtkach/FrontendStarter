namespace Jiwebapi.Catalog.Domain.Message.Category
{
    public class CategoryDeleted : BaseEvent
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
