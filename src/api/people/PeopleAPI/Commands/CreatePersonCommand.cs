using MediatR;
using PeopleAPI.Models;

namespace PeopleAPI.Commands
{
    public class CreatePersonCommand : IRequest<PersonDetails>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }

        public CreatePersonCommand(string name, string email, string address, int age)
        {
            Name = name;
            Email = email;
            Address = address;
            Age = age;
        }
    }
}
