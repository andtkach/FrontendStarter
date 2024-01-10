using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail;
using Jiwebapi.Catalog.Application.Models;
using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Api.BackgroundServices
{
    public class RequestCacheService : BackgroundService
    {
        private readonly ILogger<RequestCacheService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ContentCacheProcessingChannel _contentCacheProcessingChannel;
        
        public RequestCacheService(ILogger<RequestCacheService> logger, IServiceProvider serviceProvider,
            IMapper mapper, ContentCacheProcessingChannel contentCacheProcessingChannel)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            this._mapper = mapper;
            this._contentCacheProcessingChannel = contentCacheProcessingChannel;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"RequestCacheService started");

            await foreach (var data in this._contentCacheProcessingChannel.ReadAllAsync(stoppingToken))
            {
                try 
                {
                    _logger.LogInformation($"Delivered {data} for cache invalidation");

                    var dataArray = data.Split("|");
                    if (dataArray.Length != 2)
                    {
                        _logger.LogError($"Invalid data format {data}");
                        continue;
                    }

                    var dataName = dataArray[0];
                    var itemId = dataArray[1];
                    
                    using var scope = _serviceProvider.CreateScope();

                    switch (dataName)
                    {
                        case Constants.CategoryPrefix:
                            await this.ProcessCategory(scope, itemId);
                            break;
                        case Constants.EventPrefix:
                            await this.ProcessEvent(scope, itemId);
                            break;
                        default:
                            _logger.LogError($"Invalid data name {dataName}");
                            continue;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error while executing RequestCacheService. {e}", e);
                }
            }
        }

        private async Task ProcessCategory(IServiceScope scope, string itemId)
        {
            var categoryService = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Category>>();
            var cacheService = scope.ServiceProvider.GetRequiredService<IContentCache>();

            var item = await categoryService.GetByIdAsync(Guid.Parse(itemId));

            if (item != null)
            {
                _logger.LogInformation($"Found {item.Name} item");
                var itemDetailDto = this._mapper.Map<CategoryDetailVm>(item);
                string key = $"{Constants.ContentCachePrefix}_{Constants.CategoryPrefix}_{item.CategoryId}";
                await cacheService.Add(key, itemDetailDto, cacheService.ContentCacheSeconds);
                _logger.LogInformation($"Cached category item: {item.CategoryId} with a key {key}");
            }
            else
            {
                string key = $"{Constants.ContentCachePrefix}_{Constants.CategoryPrefix}_{itemId}";
                await cacheService.Remove(key);
                _logger.LogInformation($"Removed category item: {itemId} with a key {key}");
            }
        }

        private async Task ProcessEvent(IServiceScope scope, string itemId)
        {
            var eventService = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Event>>();
            var cacheService = scope.ServiceProvider.GetRequiredService<IContentCache>();

            var item = await eventService.GetByIdAsync(Guid.Parse(itemId));

            if (item != null)
            {
                _logger.LogInformation($"Found {item.Name} item");
                var itemDetailDto = this._mapper.Map<EventDetailVm>(item);
                string key = $"{Constants.ContentCachePrefix}_{Constants.EventPrefix}_{item.EventId}";
                await cacheService.Add(key, itemDetailDto, cacheService.ContentCacheSeconds);
                _logger.LogInformation($"Cached event item: {item.EventId} with a key {key}");
            }
            else
            {
                string key = $"{Constants.ContentCachePrefix}_{Constants.EventPrefix}_{itemId}";
                await cacheService.Remove(key);
                _logger.LogInformation($"Removed event item: {itemId} with a key {key}");
            }
        }
    }
}