using MediatR;
using PeopleAPI.Models;
using PeopleAPI.Queries;
using PeopleAPI.Repositories;
using PeopleAPI.Services.Cache;

namespace PeopleAPI.Handlers
{
    public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDetails>
    {
        private readonly IPersonRepository _repository;
        private readonly IPersonCache _cache;

        public GetPersonByIdHandler(IPersonRepository repository, IPersonCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<PersonDetails> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
        {
            var person = await _cache.GetPersonByIdAsync(query.Id);
            if (person != null)
            {
                return person;
            }

            person = await _repository.GetPersonByIdAsync(query.Id);
            await _cache.SetPerson(query.Id, person);

            return person;
        }
    }
}
