using Jiwebapi.Catalog.History.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jiwebapi.Catalog.History;

public class PublicHistoryService
{
    private readonly IMongoCollection<PublicEntry> _collection;
    
    public PublicHistoryService(
        IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(
            databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            databaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<PublicEntry>(
            "PublicEntries");
    }
    
    public async Task<List<PublicEntry>> GetAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<PublicEntry?> GetAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(PublicEntry newEntry) =>
        await _collection.InsertOneAsync(newEntry);

    public async Task UpdateAsync(string id, PublicEntry updatedEntry) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, updatedEntry);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);
}