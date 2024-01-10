using BFF.Services.Category.DTO;

namespace BFF.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoriesResponse> GetAll();

        Task<CategoryResponse> GetOne(Guid id);

        Task<CreateCategoryResponse> Create(CreateCategoryRequest item);

        Task Update(UpdateCategoryRequest item);

        Task Delete(Guid id);

    }
}
