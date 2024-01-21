using Microsoft.Extensions.Caching.Distributed;
using PeopleAPI.Models;

namespace PeopleAPI.Services.Cache
{
    public class PersonCache : IPersonCache
    {
        private readonly IDistributedCache _cache;

        public PersonCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<PersonDetails> GetPersonByIdAsync(int id)
        {
            string recordKey = $"person-{id}";
            return await _cache.GetRecordAsync<PersonDetails>(recordKey);
        }

        public async Task<bool> SetPerson(int id, PersonDetails details)
        {
            string recordKey = $"person-{id}";
            await _cache.SetRecordAsync(recordKey, details);
            return true;
        }
    }
}
