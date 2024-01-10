using System.Text.Json;
using Jiwebapi.Catalog.API.IntegrationTests.Base;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesList;

namespace Jiwebapi.Catalog.API.IntegrationTests.Controllers
{

    public class CategoryControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CategoryControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsSuccessResult()
        {
            var client = _factory.GetAnonymousClient();

            var response = await client.GetAsync("/api/category/all");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<List<CategoryListVm>>(responseString);
            
            Assert.IsType<List<CategoryListVm>>(result);
            Assert.NotEmpty(result);
        }
    }
}
