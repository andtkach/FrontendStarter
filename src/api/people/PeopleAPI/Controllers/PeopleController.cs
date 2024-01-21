using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleAPI.Commands;
using PeopleAPI.Models;
using PeopleAPI.Queries;

namespace PeopleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeopleController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<List<PersonDetails>> GetPersonListAsync()
        {
            var people = await _mediator.Send(new GetPersonListQuery());
            return people;
        }

        [HttpGet("{id}")]
        public async Task<PersonDetails> GetPersonByIdAsync(int id)
        {
            var details = await _mediator.Send(new GetPersonByIdQuery() { Id = id });
            return details;
        }

        [HttpPost]
        public async Task<PersonDetails> AddPersonAsync(PersonDetails details)
        {
            var result = await _mediator.Send(new CreatePersonCommand(
                details.Name,
                details.Email,
                details.Address,
                details.Age));
            return result;
        }

        [HttpPut]
        public async Task<int> UpdatePersonAsync(PersonDetails details)
        {
            var isUpdated = await _mediator.Send(new UpdatePersonCommand(
               details.Id,
               details.Name,
               details.Email,
               details.Address,
               details.Age));
            return isUpdated;
        }

        [HttpDelete("{id}")]
        public async Task<int> DeletePersonAsync(int id)
        {
            return await _mediator.Send(new DeletePersonCommand() { Id = id });
        }
    }
}