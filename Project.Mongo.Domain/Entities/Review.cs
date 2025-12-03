using MongoDB.Bson.Serialization.Attributes;
using Project.Mongo.Domain.Common;
using Project.Mongo.Domain.ValueObjects;

namespace Project.Mongo.Domain.Entities
{
    public class Review : Entity
    {
        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("customerEmail")]
        public Email CustomerEmail { get; set; } = null!;

        [BsonElement("rating")]
        public Rating Rating { get; set; } = null!;

        [BsonElement("comment")]
        public string Comment { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
