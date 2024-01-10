using AutoMapper;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Persistence;
using Jiwebapi.Catalog.Application.Features.Categories.Queries.GetCategoryDetail;
using Jiwebapi.Catalog.Application.Features.Events.Queries.GetEventDetail;
using Jiwebapi.Catalog.Application.Models;
using Jiwebapi.Catalog.Domain.Entities;

namespace Jiwebapi.Catalog.Api.BackgroundServices
{
    public class TimeCacheService : BackgroundService
    {
        private readonly ILogger<TimeCacheService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public TimeCacheService(ILogger<TimeCacheService> logger, IServiceProvider serviceProvider,
            IMapper mapper)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            this._mapper = mapper;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"TimeCacheService started");

            int refreshIntervalSeconds = Constants.DefaultContentCacheSeconds;
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var dt = DateTime.UtcNow;
                    _logger.LogInformation($"TimeCacheService populating cache at {dt}", dt);

                    using var scope = _serviceProvider.CreateScope();
                    var categoryProvider = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Category>>();
                    var eventProvider = scope.ServiceProvider.GetRequiredService<IAsyncRepository<Event>>();
                    var cacheService = scope.ServiceProvider.GetRequiredService<IContentCache>();
                    refreshIntervalSeconds = cacheService.ContentCacheSeconds;

                    await this.CacheCategories(categoryProvider, cacheService);
                    await this.CacheEvents(eventProvider, cacheService);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error while executing TimeCacheService. {e}", e);
                }

                int timeToSleep = refreshIntervalSeconds > 10 ? refreshIntervalSeconds - 9 : refreshIntervalSeconds; 
                _logger.LogInformation($"TimeCacheService sleep for {timeToSleep} seconds", timeToSleep);
                await Task.Delay(TimeSpan.FromSeconds(timeToSleep), stoppingToken);
            }
        }

        private async Task CacheCategories(IAsyncRepository<Category> categoryProvider, IContentCache cacheService)
        {
            var allCategories = await categoryProvider.ListAllAsync();
            _logger.LogInformation($"Found {allCategories.Count} categories");

            foreach (var category in allCategories)
            {
                var itemDetailDto = this._mapper.Map<CategoryDetailVm>(category);
                string key = $"{Constants.ContentCachePrefix}_{Constants.CategoryPrefix}_{category.CategoryId}";
                await cacheService.Add(key, itemDetailDto, cacheService.ContentCacheSeconds);
                _logger.LogInformation($"Cached category item: {category.CategoryId} with a key {key}");
            }
        }

        private async Task CacheEvents(IAsyncRepository<Event> eventProvider, IContentCache cacheService)
        {
            var allEvents = await eventProvider.ListAllAsync();
            _logger.LogInformation($"Found {allEvents.Count} events");

            foreach (var item in allEvents)
            {
                var itemDetailDto = this._mapper.Map<EventDetailVm>(item);
                string key = $"{Constants.ContentCachePrefix}_{Constants.EventPrefix}_{item.EventId}";
                await cacheService.Add(key, itemDetailDto, cacheService.ContentCacheSeconds);
                _logger.LogInformation($"Cached event item: {item.EventId} with a key {key}");
            }
        }
    }
}