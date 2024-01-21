using System.Diagnostics;
using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Exceptions;
using Jiwebapi.Catalog.Application.Models;
using Jiwebapi.Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail
{
    public class GetCategoryDetailQueryHandler : IRequestHandler<GetCategoryDetailQuery, BaseVmResponse>
    {
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryDetailQueryHandler> _logger;
        private readonly IContentCache _contentCache;

        public GetCategoryDetailQueryHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository, ILogger<GetCategoryDetailQueryHandler> logger,
            IContentCache contentCache)
        {
            this._mapper = mapper;
            this._categoryRepository = categoryRepository;
            this._logger = logger;
            this._contentCache = contentCache;
        }

        public async Task<BaseVmResponse> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (request.UseCache)
            {
                var cachedData = await this._contentCache.Get<CategoryDetailVm>($"{Constants.ContentCachePrefix}_{Constants.CategoryPrefix}_{request.Id}");
                if (cachedData != null)
                {
                    this._logger.LogInformation($"GetCategoryDetailQueryHandler [{request.Id}] provided from cache");
                    sw.Stop();
                    return new BaseVmResponse()
                    {
                        Data = cachedData,
                        Meta = $"d-s=c;el={sw.ElapsedMilliseconds}"
                    };
                }
            }

            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            var categoryDetailDto = _mapper.Map<CategoryDetailVm>(category);

            if (request.UseCache)
            {
                await this._contentCache.Add($"{Constants.ContentCachePrefix}_{Constants.CategoryPrefix}_{request.Id}", categoryDetailDto, this._contentCache.ContentCacheSeconds);
            }

            this._logger.LogInformation($"GetCategoryDetailQueryHandler [{request.Id}] provided from database");
            
            sw.Stop();
            return new BaseVmResponse()
            {
                Data = categoryDetailDto,
                Meta = $"d-s=d;el={sw.ElapsedMilliseconds}"
            };
        }
    }
}
