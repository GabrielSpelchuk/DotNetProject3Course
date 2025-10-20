using System.Data;
using Project.DAL.Repositories;
using Project.Domain.Entities;

namespace Project.DAL.Uow
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Supplier> Suppliers { get; }

        Task BeginTransactionAsync(CancellationToken ct);
        Task CommitAsync(CancellationToken ct);
        Task RollbackAsync(CancellationToken ct);
        IDbConnection GetDbConnection();
    }
}
