using Microsoft.EntityFrameworkCore;
using PeopleAPI.Data;
using PeopleAPI.Models;

namespace PeopleAPI.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DbContextClass _dbContext;

        public PersonRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PersonDetails> AddPersonAsync(PersonDetails details)
        {
            var result = _dbContext.People.Add(details);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<int> DeletePersonAsync(int id)
        {
            var filteredData = _dbContext.People.FirstOrDefault(x => x.Id == id);
            if (filteredData != null)
            {
                _dbContext.People.Remove(filteredData);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<PersonDetails> GetPersonByIdAsync(int id)
        {
            return await _dbContext.People.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<PersonDetails>> GetPersonListAsync()
        {
            return await _dbContext.People.ToListAsync();
        }

        public async Task<int> UpdatePersonAsync(PersonDetails details)
        {
            _dbContext.People.Update(details);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
