using MediatR;
using PeopleAPI.Commands;
using PeopleAPI.Models;
using PeopleAPI.Repositories;

namespace PeopleAPI.Handlers
{
    public class CreatePersonHandler: IRequestHandler<CreatePersonCommand, PersonDetails>
    {
        private readonly IPersonRepository _repository;

        public CreatePersonHandler(IPersonRepository repository)
        {
            _repository = repository;
        }
        public async Task<PersonDetails> Handle(CreatePersonCommand command, CancellationToken cancellationToken)
        {
            var details = new PersonDetails()
            {
                Name = command.Name,
                Email = command.Email,
                Address = command.Address,
                Age = command.Age
            };

            return await _repository.AddPersonAsync(details);
        }
    }
}
