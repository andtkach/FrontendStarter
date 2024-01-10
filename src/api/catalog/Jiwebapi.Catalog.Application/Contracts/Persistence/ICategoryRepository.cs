using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Application.Contracts.Persistence
{
    public interface ICategoryRepository : IAsyncRepository<Category>
    {
        Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents, int skip, int take);
    }
}
