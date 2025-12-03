using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Mongo.Domain.Common
{
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
    }
}
