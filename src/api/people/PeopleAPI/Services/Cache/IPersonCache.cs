using PeopleAPI.Models;

namespace PeopleAPI.Services.Cache
{
    public interface IPersonCache
    {
        Task<PersonDetails> GetPersonByIdAsync(int id);
        Task<bool> SetPerson(int id, PersonDetails details);
    }
}
