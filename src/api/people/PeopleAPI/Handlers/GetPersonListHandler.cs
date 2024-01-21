using MediatR;
using PeopleAPI.Models;
using PeopleAPI.Queries;
using PeopleAPI.Repositories;

namespace PeopleAPI.Handlers
{
    public class GetPersonListHandler :  IRequestHandler<GetPersonListQuery, List<PersonDetails>>
    {
        private readonly IPersonRepository _repository;

        public GetPersonListHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PersonDetails>> Handle(GetPersonListQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetPersonListAsync();
        }
    }
}
