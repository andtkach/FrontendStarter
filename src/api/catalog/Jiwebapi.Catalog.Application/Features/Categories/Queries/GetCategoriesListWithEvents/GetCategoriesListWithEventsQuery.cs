using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesListWithEvents
{
    public class GetCategoriesListWithEventsQuery: IRequest<CategoryEventListVmResponse>
    {
        public bool IncludeHistory { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
