using API.Controllers;
using API.DTOs;
using BFF.Services;
using BFF.Services.Auth;
using BFF.Services.Category;
using BFF.Services.Category.DTO;
using BFF.Services.People;
using BFF.Services.People.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

[Authorize]
public class PeopleController : BaseApiController
{
    private readonly IPeopleService _peopleService;
    
    public PeopleController(IPeopleService peopleService)
    {
        _peopleService = peopleService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<PersonDetails>>> GetAll()
    {
        var result = await _peopleService.GetAll();
        return result;
    }

    [HttpGet("one/{id:int}")]
    public async Task<ActionResult<PersonDetails>> GetOne(int id)
    {
        var result = await _peopleService.GetOne(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<PersonDetails>> Create(PersonDetails request)
    {
        var result = await _peopleService.Create(request);
        return Ok(result);
    }

    [HttpPut()]
    public async Task<ActionResult> Update(PersonDetails request)
    {
        await _peopleService.Update(request);
        return Ok(request);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _peopleService.Delete(id);
        return NoContent();
    }
}