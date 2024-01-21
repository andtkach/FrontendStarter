using MediatR;
using PeopleAPI.Models;

namespace PeopleAPI.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDetails>
    {
        public int Id { get; set; }
    }
}
