using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Application.Contracts.Persistence
{
    public interface IEventRepository : IAsyncRepository<Event>
    {
        Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate);
    }
}
