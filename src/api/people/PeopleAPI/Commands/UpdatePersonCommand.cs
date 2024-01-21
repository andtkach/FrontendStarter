using MediatR;

namespace PeopleAPI.Commands
{
    public class UpdatePersonCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }

        public UpdatePersonCommand(int id, string name, string email, string address, int age)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            Age = age;
        }
    }
}
