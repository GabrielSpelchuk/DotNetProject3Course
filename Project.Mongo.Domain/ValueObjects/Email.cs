using MongoDB.Bson.Serialization.Attributes;

namespace Project.Mongo.Domain.ValueObjects
{
    public class Email
    {
        [BsonElement("value")]
        public string Value { get; private set; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException("Invalid email format", nameof(value));
            Value = value;
        }

        private Email() { }
        public override string ToString() => Value;
    }
}
