using Jiwebapi.Catalog.Api.BackgroundServices;
using Jiwebapi.Catalog.Api.Utility.Extensions;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.CreateCateogry;
using Jiwebapi.Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail;
using Jiwebapi.Catalog.Application.Features.Events.Commands.CreateEvent;
using Jiwebapi.Catalog.Application.Features.Events.Commands.DeleteEvent;
using Jiwebapi.Catalog.Application.Features.Events.Commands.UpdateEvent;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventsList;
using Jiwebapi.Catalog.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jiwebapi.Catalog.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ContentCacheProcessingChannel _contentCacheProcessingChannel;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IMediator mediator, ILogger<EventsController> logger,
            ContentCacheProcessingChannel contentCacheProcessingChannel)
        {
            _mediator = mediator;
            this._logger = logger;
            this._contentCacheProcessingChannel = contentCacheProcessingChannel;
        }

        [HttpGet(Name = "GetAllEvents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<EventListVm>>> GetAllEvents()
        {
            var dtos = await _mediator.Send(new GetEventsListQuery());
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetEventById")]
        public async Task<ActionResult<EventDetailVm>> GetEventById(Guid id)
        {
            var getEventDetailQuery = new GetEventDetailQuery() { Id = id, UseCache = true };
            var result = await _mediator.Send(getEventDetailQuery);
            HttpContext.AddResponseMeta(result);
            return Ok(result.Data);
        }

        [HttpPost(Name = "AddEvent")]
        public async Task<ActionResult<CreateEventCommandResponse>> Create([FromBody] CreateEventCommand createEventCommand)
        {

            var result = await _mediator.Send(createEventCommand);
            if (result.Success)
            {
                await this._contentCacheProcessingChannel.ProcessContentAsync(result.Event.EventId.ToString(), Constants.EventPrefix);
            }
            return Ok(result);
        }

        [HttpPut(Name = "UpdateEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateEventCommand updateEventCommand)
        {
            await _mediator.Send(updateEventCommand);
            await this._contentCacheProcessingChannel.ProcessContentAsync(updateEventCommand.EventId.ToString(), Constants.EventPrefix);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteEvent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleteEventCommand = new DeleteEventCommand() { EventId = id };
            await _mediator.Send(deleteEventCommand);
            await this._contentCacheProcessingChannel.ProcessContentAsync(id.ToString(), Constants.EventPrefix);
            return NoContent();
        }
    }
}
