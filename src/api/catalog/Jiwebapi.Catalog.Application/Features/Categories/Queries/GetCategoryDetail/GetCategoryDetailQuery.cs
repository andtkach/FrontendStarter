using Jiwebapi.Catalog.Application.Models;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail
{
    public class GetCategoryDetailQuery: IRequest<BaseVmResponse>
    {
        public Guid Id { get; set; }
        public bool UseCache { get; set; }
    }
}
