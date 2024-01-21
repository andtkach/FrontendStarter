using MediatR;

namespace PeopleAPI.Commands
{
    public class DeletePersonCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}

