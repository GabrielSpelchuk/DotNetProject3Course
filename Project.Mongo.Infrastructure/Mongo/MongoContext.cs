using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project.Mongo.Domain.Entities;

namespace Project.Mongo.Infrastructure.Mongo
{
    public class MongoContext
    {
        private readonly IMongoDatabase _db;
        public MongoContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<Review> Reviews => _db.GetCollection<Review>("reviews");
    }
}
