using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesListWithEvents
{
    public class GetCategoriesListWithEventsQueryHandler : IRequestHandler<GetCategoriesListWithEventsQuery, CategoryEventListVmResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesListWithEventsQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryEventListVmResponse> Handle(GetCategoriesListWithEventsQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var take = request.PageSize;
            var list = await _categoryRepository.GetCategoriesWithEvents(request.IncludeHistory, skip, take);
            var totalItems = await _categoryRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);
            var result = _mapper.Map<List<CategoryEventListVm>>(list);
            return new CategoryEventListVmResponse
            {
                Result = result,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
            };
            }
    }
}
