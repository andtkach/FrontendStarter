using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace PeopleAPI.Services.Cache
{
    internal static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(
            this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(
            this IDistributedCache cache,
            string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null) return default(T);

            var dataResponse = JsonSerializer.Deserialize<T>(jsonData);

            return dataResponse;
        }
    }
}
