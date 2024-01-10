using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Application.Contracts.Persistence
{
    internal interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();

        Task<Category> GetCategoryById(string id);
        
        Task<Category> CreateCategoryItem(Category category);

        Task<bool> UpdateCategory(Category category);

        Task<bool> RemoveCategory(string categoryId);
        
        Task<bool> ExistsById(string itemId);
    }
}
