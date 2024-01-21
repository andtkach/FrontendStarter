using API.Controllers;
using API.DTOs;
using BFF.Services;
using BFF.Services.Auth;
using BFF.Services.Category;
using BFF.Services.Category.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

[Authorize]
public class CategoryController : BaseApiController
{
    private readonly ICategoryService _categoryService;
    
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<Categories>> GetAll()
    {
        var result = await _categoryService.GetAll();
        return result;
    }

    [HttpGet("one/{id:guid}")]
    public async Task<ActionResult<Category>> GetOne(Guid id)
    {
        var result = await _categoryService.GetOne(id);
        return Ok(result);
    }

    [HttpPost()]
    public async Task<ActionResult<Category>> Create(CreateCategory request)
    {
        var result = await _categoryService.Create(request);
        return Ok(result);
    }

    [HttpPut()]
    public async Task<ActionResult<Category>> Update(UpdateCategory request)
    {
        await _categoryService.Update(request);
        return Ok(request);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _categoryService.Delete(id);
        return NoContent();
    }
}