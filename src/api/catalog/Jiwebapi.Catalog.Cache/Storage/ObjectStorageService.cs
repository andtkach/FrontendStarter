using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Domain.Common;
using StackExchange.Redis;

namespace Jiwebapi.Catalog.Cache.Storage;

public class ObjectStorageService: IObjectStorageService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly string _nameSet = "Objects";

    public ObjectStorageService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public async Task<StorageObject> CreateItem(StorageObject item)
    {
        if (item == null)
        {
            throw new ArgumentException($"{nameof(item)} is null");
        }

        var db = _redis.GetDatabase();

        var itemStr = JsonSerializer.Serialize(item);

        await db.HashSetAsync(this._nameSet, new HashEntry[]
        {
            new HashEntry(item.Id, itemStr)
        });

        return item;
    }

    public async Task<StorageObject> GetItemById(string id)
    {
        var db = _redis.GetDatabase();

        var itemStr = await db.HashGetAsync(this._nameSet, $"{StorageObject.Name}{StorageObject.S}{id}");

        if (!string.IsNullOrEmpty(itemStr))
        {
            return JsonSerializer.Deserialize<StorageObject>(itemStr);
        }
            
        return null;
    }

    public async Task<IEnumerable<StorageObject>> GetAllItems()
    {
        var db = _redis.GetDatabase();

        var completeSet = await db.HashGetAllAsync(this._nameSet);
            
        if (completeSet.Length > 0)
        {
            var obj = Array.ConvertAll(completeSet, val => 
                JsonSerializer.Deserialize<StorageObject>(val.Value)).ToList();
            return obj;   
        }
            
        return null;
    }

    public async Task<StorageObject> UpdateItem(StorageObject item)
    {
        if (item == null)
        {
            throw new ArgumentException($"{nameof(item)} is null");
        }
            
        var db = _redis.GetDatabase();
        item.Id = $"{StorageObject.Name}{StorageObject.S}{item.Id}";
            
        var existingItemStr = await db.HashGetAsync(this._nameSet, item.Id);

        if (string.IsNullOrEmpty(existingItemStr))
        {
            return null;
        }
            
        await db.HashDeleteAsync(this._nameSet, item.Id);
        var itemStr = JsonSerializer.Serialize(item);
        await db.HashSetAsync(this._nameSet, new HashEntry[]
        {
            new HashEntry(item.Id, itemStr)
        });

        return item;
    }

    public async Task DeleteItem(string id)
    {
        var db = _redis.GetDatabase();
        await db.HashDeleteAsync(this._nameSet, $"{StorageObject.Name}{StorageObject.S}{id}");
    }
}