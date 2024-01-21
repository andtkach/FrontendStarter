using System;
using System.Threading.Tasks;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Domain.Common;
using System.Text.Json;
using StackExchange.Redis;

namespace Jiwebapi.Catalog.Cache.Storage;

public class SimpleStorageService : ISimpleStorageService
{
    private readonly IConnectionMultiplexer _redis;
    
    public SimpleStorageService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public async Task<string> Set(string value)
    {
        var item = new StorageItem()
        {
            Value = value
        };
        
        var db = _redis.GetDatabase();
        var itemStr = JsonSerializer.Serialize(item);
        await db.StringSetAsync(item.Id, itemStr, TimeSpan.FromMinutes(1));
        return item.GetId();
    }

    public async Task<StorageItem> Get(string id)
    {
        var db = _redis.GetDatabase();

        var itemStr = await db.StringGetAsync($"{StorageItem.Name}{StorageItem.S}{id}");

        if (!string.IsNullOrEmpty(itemStr))
        {
            return JsonSerializer.Deserialize<StorageItem>(itemStr);
        }
            
        throw new ArgumentException($"{nameof(id)} is null");
    }

    public async Task Remove(string id)
    {
        var db = _redis.GetDatabase();
        await db.KeyDeleteAsync($"{StorageItem.Name}{StorageItem.S}{id}");
    }
}