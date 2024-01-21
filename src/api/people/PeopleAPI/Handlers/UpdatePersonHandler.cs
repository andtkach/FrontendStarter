using MediatR;
using PeopleAPI.Commands;
using PeopleAPI.Repositories;

namespace PeopleAPI.Handlers
{
    public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, int>
    {
        private readonly IPersonRepository _repository;

        public UpdatePersonHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(UpdatePersonCommand command, CancellationToken cancellationToken)
        {
            var details = await _repository.GetPersonByIdAsync(command.Id);
            if (details == null)
                return default;

            details.Name = command.Name;
            details.Email = command.Email;
            details.Address = command.Address;
            details.Age = command.Age;

            return await _repository.UpdatePersonAsync(details);
        }
    }
}
