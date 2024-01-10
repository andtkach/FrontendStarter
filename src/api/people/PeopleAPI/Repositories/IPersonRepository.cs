using PeopleAPI.Models;

namespace PeopleAPI.Repositories
{
    public interface IPersonRepository
    {
        public Task<List<PersonDetails>> GetPersonListAsync();
        public Task<PersonDetails> GetPersonByIdAsync(int id);
        public Task<PersonDetails> AddPersonAsync(PersonDetails details);
        public Task<int> UpdatePersonAsync(PersonDetails details);
        public Task<int> DeletePersonAsync(int id);
    }
}
