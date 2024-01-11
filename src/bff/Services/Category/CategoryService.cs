using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BFF.Extensions;
using BFF.Services.Category.DTO;

namespace BFF.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentUserService.Token);
        }

        public async Task<DTO.Categories> GetAll()
        {
            HttpResponseMessage response = await _client.GetAsync("api/category/all");
            var result = await response.ReadContentAs<CategoriesResponse>();

            var data = new DTO.Categories()
            {
                Result = new List<DTO.Category>(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
            };

            foreach (var item in result.Result)
            {
                data.Result.Add(new DTO.Category()
                {
                    Id = item.CategoryId,
                    Name = item.Name
                });
            }

            return data;
        }

        public async Task<DTO.Category> GetOne(Guid id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/category/{id}");
            var result = await response.ReadContentAs<CategoryResponse>();
            return new DTO.Category()
            {
                Id = result.CategoryId,
                Name = result.Name
            };
        }

        public async Task<DTO.Category> Create(CreateCategory item)
        {
            var createItem = new CreateCategoryRequest()
            {
                Name = item.Name
            };

            using StringContent content = new(
                JsonSerializer.Serialize(createItem),
                Encoding.UTF8,
                "application/json");

            var resultMessage = await _client.PostAsync($"api/category", content);
            var result = await resultMessage.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in create method");
            }

            return new DTO.Category()
            {
                Id = result.Category.CategoryId,
                Name = result.Category.Name
            };
        }

        public async Task Update(UpdateCategory item)
        {
            var updateItem = new UpdateCategoryRequest()
            {
                CategoryId = item.Id,
                Name = item.Name
            };

            using StringContent content = new(
                JsonSerializer.Serialize(updateItem),
                Encoding.UTF8,
                "application/json");

            await _client.PutAsync($"api/category", content);
        }

        public async Task Delete(Guid id)
        {
            await _client.DeleteAsync($"api/category/{id}");
        }
    }
}
