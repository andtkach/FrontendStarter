using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BFF.Extensions;
using BFF.Services.Category.DTO;
using BFF.Services.People.DTO;

namespace BFF.Services.People
{
    public class PeopleService : IPeopleService
    {
        private readonly HttpClient _client;
        private readonly ICurrentUserService _currentUserService;

        public PeopleService(HttpClient client, ICurrentUserService currentUserService)
        {
            _client = client;
            _currentUserService = currentUserService;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentUserService.Token);
        }

        public async Task<List<PersonDetails>> GetAll()
        {
            HttpResponseMessage response = await _client.GetAsync("api/people");
            response.EnsureSuccessStatusCode();
            return await response.ReadContentAs<List<PersonDetails>>();
        }

        public async Task<PersonDetails> GetOne(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/people/{id}");
            response.EnsureSuccessStatusCode();
            return await response.ReadContentAs<PersonDetails>();
        }

        public async Task<PersonDetails> Create(PersonDetails item)
        {
            using StringContent content = new(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync($"api/people", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PersonDetails>();
            if (result == null)
            {
                throw new InvalidOperationException("Error in create method");
            }

            return result;
        }

        public async Task Update(PersonDetails item)
        {
            using StringContent content = new(
                JsonSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PutAsync($"api/people", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(int id)
        {
            var response = await _client.DeleteAsync($"api/people/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
