using System;
using System.Threading.Tasks;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Jiwebapi.Catalog.Cache.Cache
{
    public class ContentCache: IContentCache
    {
        private readonly IDistributedCache _memoryCache;

        public ContentCache(IDistributedCache memoryCache)
        {
            _memoryCache = memoryCache;
            this.ContentCacheSeconds = Constants.DefaultContentCacheSeconds;
        }

        public int ContentCacheSeconds { get; set; }

        public async Task Add<T>(string key, T item, int seconds = 60)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cache key could not be null");
            }

            var option = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromSeconds(seconds)
            };

            await _memoryCache.SetAsync<T>(key, item, option);
        }

        public async Task<T> Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cache key could not be null");
            }
            
            return await _memoryCache.GetAsync<T>(key);
        }

        public async Task Remove(string key)
        {
            await this._memoryCache.RemoveAsync(key);
        }
    }
}