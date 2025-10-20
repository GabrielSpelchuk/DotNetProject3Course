using System.Linq.Expressions;

namespace Project.DAL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id, CancellationToken ct);
        Task<T?> GetByIdWithIncludesAsync(int id, CancellationToken ct, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct);
        IQueryable<T> Query();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct);
        void Update(T entity);
        void Remove(T entity);
    }
}