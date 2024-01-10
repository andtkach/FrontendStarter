using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jiwebapi.Catalog.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetCategoriesWithEvents(bool includePassedEvents, int skip, int take)
        {
            var allCategories = await _dbContext.Categories
                .Include(x => x.Events)
                .OrderBy(x => x.CategoryId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            
            if(!includePassedEvents)
            {
                allCategories.ForEach(p =>
                {
                    if (p.Events != null) p.Events.ToList().RemoveAll(c => c.Date < DateTime.Today);
                });
            }
            return allCategories;
        }
    }
}
