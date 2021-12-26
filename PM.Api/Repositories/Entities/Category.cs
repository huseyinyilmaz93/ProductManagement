using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PM.Api.Repositories.Base;

namespace PM.Api.Repositories.MongoEntities;

public class Category : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonIgnoreIfDefault] 
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

}

