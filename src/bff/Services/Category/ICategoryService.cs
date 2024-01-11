using BFF.Services.Category.DTO;

namespace BFF.Services.Category
{
    public interface ICategoryService
    {
        Task<DTO.Categories> GetAll();

        Task<DTO.Category> GetOne(Guid id);

        Task<DTO.Category> Create(CreateCategory item);

        Task Update(UpdateCategory item);

        Task Delete(Guid id);

    }
}
