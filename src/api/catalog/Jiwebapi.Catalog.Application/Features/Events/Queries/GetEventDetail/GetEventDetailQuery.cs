using Jiwebapi.Catalog.Application.Models;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQuery: IRequest<BaseVmResponse>
    {
        public Guid Id { get; set; }
        public bool UseCache { get; set; }
    }
}
