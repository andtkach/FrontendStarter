using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jiwebapi.Catalog.Persistence.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsEventNameAndDateUnique(string name, DateTime eventDate)
        {
            var matches = await _dbContext.Events.AnyAsync(e => e.Name.Equals(name) && e.Date.Date.Equals(eventDate.Date));
            return matches;
        }
    }
}
