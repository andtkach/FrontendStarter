using Jiwebapi.Catalog.Application.Responses;

namespace Jiwebapi.Catalog.Application.Features.Events.Commands.CreateEvent
{
    public class CreateEventCommandResponse: BaseResponse
    {
        public CreateEventCommandResponse(): base()
        {

        }

        public CreateEventDto Event { get; set; } = default!;
    }
}