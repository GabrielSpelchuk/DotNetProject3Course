using MongoDB.Driver;
using Project.Mongo.Domain.Entities;

namespace Project.Mongo.Infrastructure.Mongo
{
    public class ReviewRepository
    {
        private readonly MongoContext _ctx;
        public ReviewRepository(MongoContext ctx) => _ctx = ctx;

        public async Task AddAsync(Review review, CancellationToken ct)
            => await _ctx.Reviews.InsertOneAsync(review, cancellationToken: ct);

        public async Task<IEnumerable<Review>> GetByProductAsync(int productId, int page, int pageSize, CancellationToken ct)
        {
            var skip = (page - 1) * pageSize;
            var cursor = await _ctx.Reviews.Aggregate()
                .Match(r => r.ProductId == productId)
                .SortByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync(ct);
            return cursor;
        }

        public async Task<(double average, long count)> GetStatsAsync(int productId, CancellationToken ct)
        {
            var pipeline = _ctx.Reviews.Aggregate()
                .Match(r => r.ProductId == productId)
                .Group(r => r.ProductId, g => new { Avg = g.Average(x => x.Rating.Value), Count = g.LongCount() });

            var res = await pipeline.FirstOrDefaultAsync(ct);
            if (res == null) return (0.0, 0);
            return (res.Avg, res.Count);
        }
    }
}
