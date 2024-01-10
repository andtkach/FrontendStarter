using Jiwebapi.Catalog.History.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jiwebapi.Catalog.History;

public class PrivateHistoryService
{
    private readonly IMongoCollection<PrivateEntry> _collection;
    
    public PrivateHistoryService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<PrivateEntry>(
            "PrivateEntries");
    }
    
    public async Task<List<PrivateEntry>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<PrivateEntry?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(PrivateEntry newEntry) =>
        await _collection.InsertOneAsync(newEntry);

    public async Task UpdateAsync(string id, PrivateEntry updatedEntry) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updatedEntry);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}