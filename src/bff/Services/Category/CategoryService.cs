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

        public async Task<CategoriesResponse> GetAll()
        {
            HttpResponseMessage response = await _client.GetAsync("api/category/all");
            return await response.ReadContentAs<CategoriesResponse>();
        }

        public async Task<CategoryResponse> GetOne(Guid id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/category/{id}");
            return await response.ReadContentAs<CategoryResponse>();
        }

        public async Task<CreateCategoryResponse> Create(CreateCategoryRequest item)
        {
            using StringContent content = new(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var resultMessage = await _client.PostAsync($"api/category", content);
            var result = await resultMessage.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in create method");
            }

            return result;
        }

        public async Task Update(UpdateCategoryRequest item)
        {
            using StringContent content = new(
                JsonSerializer.Serialize(item),
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
