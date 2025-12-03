using MongoDB.Bson.Serialization.Attributes;

namespace Project.Mongo.Domain.Entities
{
    public class Rating
    {
        [BsonElement("value")]
        public int Value { get; private set; }

        public Rating(int value)
        {
            if (value < 1 || value > 5) throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }
        private Rating() { }
    }
}
