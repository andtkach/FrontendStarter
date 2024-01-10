using Jiwebapi.Catalog.Api.BackgroundServices;
using Jiwebapi.Catalog.Api.Utility.Extensions;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesList;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesListWithEvents;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail;
using Jiwebapi.Catalog.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jiwebapi.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ContentCacheProcessingChannel _contentCacheProcessingChannel;

        public CategoryController(IMediator mediator, ContentCacheProcessingChannel contentCacheProcessingChannel)
        {
            this._mediator = mediator;
            this._contentCacheProcessingChannel = contentCacheProcessingChannel;
        }

        [HttpGet("all", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryListVmResponse>> GetAllCategories([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var dtos = await _mediator.Send(new GetCategoriesListQuery()
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10,
            });
            return Ok(dtos);
        }

        [HttpGet("allwithevents", Name = "GetCategoriesWithEvents")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryEventListVmResponse>> GetCategoriesWithEvents([FromQuery] bool includeHistory, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            GetCategoriesListWithEventsQuery getCategoriesListWithEventsQuery = new GetCategoriesListWithEventsQuery() { 
                IncludeHistory = includeHistory, 
                PageNumber = pageNumber ?? 1, 
                PageSize = pageSize ?? 10,
            };

            var dtos = await _mediator.Send(getCategoriesListWithEventsQuery);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDetailVm>> GetCategoryById(Guid id)
        {
            var getCategoryDetailQuery = new GetCategoryDetailQuery() { Id = id, UseCache = true };
            var result = await _mediator.Send(getCategoryDetailQuery);
            HttpContext.AddResponseMeta(result);
            return Ok(result.Data);
        }

        [HttpPost(Name = "AddCategory")]
        public async Task<ActionResult<CreateCategoryCommandResponse>> Create([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            var response = await _mediator.Send(createCategoryCommand);
            if (response.Success)
            {
                await this._contentCacheProcessingChannel.ProcessContentAsync(response.Category.CategoryId.ToString(), Constants.CategoryPrefix);
            }
            
            return Ok(response);
        }

        [HttpPut(Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand updateCategoryCommand)
        {
            await _mediator.Send(updateCategoryCommand);
            await this._contentCacheProcessingChannel.ProcessContentAsync(updateCategoryCommand.CategoryId.ToString(), Constants.CategoryPrefix);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleteCategoryCommand = new DeleteCategoryCommand() { CategoryId = id };
            await _mediator.Send(deleteCategoryCommand);
            await this._contentCacheProcessingChannel.ProcessContentAsync(id.ToString(), Constants.CategoryPrefix);
            return NoContent();
        }
    }
}
