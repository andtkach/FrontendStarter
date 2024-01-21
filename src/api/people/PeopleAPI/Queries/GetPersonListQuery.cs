using MediatR;
using PeopleAPI.Models;

namespace PeopleAPI.Queries
{
    public class GetPersonListQuery :  IRequest<List<PersonDetails>>
    {
    }
}
