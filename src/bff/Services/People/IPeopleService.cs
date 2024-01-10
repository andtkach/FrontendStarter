using BFF.Services.People.DTO;

namespace BFF.Services.People
{
    public interface IPeopleService
    {
        Task<List<PersonDetails>> GetAll();

        Task<PersonDetails> GetOne(int id);

        Task<PersonDetails> Create(PersonDetails item);

        Task Update(PersonDetails item);

        Task Delete(int id);
    }
}
