using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PM.Api.Repositories.Base;

namespace PM.Api.Repositories.MongoEntities;

public class Product : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
}

