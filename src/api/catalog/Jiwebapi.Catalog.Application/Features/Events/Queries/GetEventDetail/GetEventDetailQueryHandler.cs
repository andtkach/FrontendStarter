using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Exceptions;
using Jiwebapi.Catalog.Application.Models;
using Jiwebapi.Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail
{
    public class GetEventDetailQueryHandler : IRequestHandler<GetEventDetailQuery, BaseVmResponse>
    {
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IContentCache _contentCache;
        private readonly ILogger<GetEventDetailQueryHandler> _logger;

        public GetEventDetailQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository, IAsyncRepository<Category> categoryRepository,
            IContentCache contentCache, ILogger<GetEventDetailQueryHandler> logger)
        {
            this._mapper = mapper;
            this._eventRepository = eventRepository;
            this._categoryRepository = categoryRepository;
            this._contentCache = contentCache;
            this._logger = logger;
        }

        public async Task<BaseVmResponse> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();

            if (request.UseCache)
            {
                var cachedData = await this._contentCache.Get<EventDetailVm>($"{Constants.ContentCachePrefix}_{Constants.EventPrefix}_{request.Id}");
                if (cachedData != null)
                {
                    this._logger.LogInformation($"GetEventDetailQueryHandler [{request.Id}] provided from cache");
                    sw.Stop();
                    return new BaseVmResponse()
                    {
                        Data = cachedData,
                        Meta = $"d-s=c;el={sw.ElapsedMilliseconds}"
                    };
                }
            }

            var item = await _eventRepository.GetByIdAsync(request.Id);
            if (item == null)
            {
                throw new NotFoundException(nameof(Event), request.Id);
            }

            var eventDetailDto = _mapper.Map<EventDetailVm>(item);
            
            var category = await _categoryRepository.GetByIdAsync(item.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(nameof(Category), item.CategoryId);
            }
            eventDetailDto.Category = _mapper.Map<CategoryDto>(category);

            if (request.UseCache)
            {
                await this._contentCache.Add($"{Constants.ContentCachePrefix}_{Constants.EventPrefix}_{request.Id}", eventDetailDto, this._contentCache.ContentCacheSeconds);
            }

            this._logger.LogInformation($"GetEventDetailQuery [{request.Id}] provided from database");

            sw.Stop();
            return new BaseVmResponse()
            {
                Data = eventDetailDto,
                Meta = $"d-s=d;el={sw.ElapsedMilliseconds}"
            };
        }
    }
}
