using Jiwebapi.Catalog.Domain.Message;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jiwebapi.Catalog.History.Entity;

public class PrivateEntry
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;


    [BsonElement("User")]
    public string? User { get; set; } = null!;

    
    [BsonElement("Action")]
    public string Action { get; set; } = null!;

    
    [BsonElement("Data")]
    public string? Data { get; set; }

    [BsonElement("DataTraceId")]
    public string? DataTraceId { get; set; } = null!;
}