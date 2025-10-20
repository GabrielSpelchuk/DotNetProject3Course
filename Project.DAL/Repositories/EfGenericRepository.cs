using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Project.DAL.Repositories
{
    public class EfGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _ctx;
        protected readonly DbSet<T> _db;

        public EfGenericRepository(AppDbContext ctx)
        {
            _ctx = ctx;
            _db = ctx.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken ct) => await _db.AddAsync(entity, ct);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct) =>
            await _db.AddRangeAsync(entities, ct);
        
        public IQueryable<T> Query() => _db.AsQueryable();
        public void Remove(T entity) => _db.Remove(entity);
        public void Update(T entity) => _db.Update(entity);

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct) =>
            await _db.FindAsync(new object[] { id }, ct);

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct) => await _db.ToListAsync(ct);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct) =>
            await _db.Where(predicate).ToListAsync(ct);

        public async Task<T?> GetByIdWithIncludesAsync(int id, CancellationToken ct,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> q = _db;
            foreach (var inc in includes) q = q.Include(inc);
            var key = _ctx.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.First();
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Equal(Expression.PropertyOrField(param, key.Name),
                Expression.Constant(Convert.ChangeType(id, key.ClrType)));
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return await q.FirstOrDefaultAsync(lambda, ct);
        }
    }
}