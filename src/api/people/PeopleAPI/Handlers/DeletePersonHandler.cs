using MediatR;
using PeopleAPI.Commands;
using PeopleAPI.Repositories;

namespace PeopleAPI.Handlers
{
    public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, int>
    {
        private readonly IPersonRepository _repository;

        public DeletePersonHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
        {
            var details = await _repository.GetPersonByIdAsync(command.Id);
            if (details == null)
                return default;

            return await _repository.DeletePersonAsync(details.Id);
        }
    }
}