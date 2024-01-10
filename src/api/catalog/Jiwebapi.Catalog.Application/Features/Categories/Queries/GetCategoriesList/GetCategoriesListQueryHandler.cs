using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Domain.Entities;
using MediatR;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoriesList
{
    public class GetCategoriesListQueryHandler : IRequestHandler<GetCategoriesListQuery, CategoryListVmResponse>
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoriesListQueryHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryListVmResponse> Handle(GetCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            var take = request.PageSize;
            var allCategories = (await _categoryRepository.ListAllAsync()).OrderBy(x => x.CategoryId).Skip(skip).Take(take);
            var totalItems = await _categoryRepository.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / request.PageSize);
            var result = _mapper.Map<List<CategoryListVm>>(allCategories);
            return new CategoryListVmResponse
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
