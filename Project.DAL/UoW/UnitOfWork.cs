using Microsoft.EntityFrameworkCore.Storage;
using Project.Domain.Entities;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Repositories;

namespace Project.DAL.Uow
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AppDbContext _ctx;
        private IDbContextTransaction? _tx;

        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Customer> Customers { get; }
        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<OrderItem> OrderItems { get; }
        public IGenericRepository<Payment> Payments { get; }
        public IGenericRepository<Supplier> Suppliers { get; }

        public UnitOfWork(AppDbContext ctx)
        {
            _ctx = ctx;

            Products = new EfGenericRepository<Product>(_ctx);
            Customers = new EfGenericRepository<Customer>(_ctx);
            Orders = new EfGenericRepository<Order>(_ctx);
            OrderItems = new EfGenericRepository<OrderItem>(_ctx);
            Payments = new EfGenericRepository<Payment>(_ctx);
            Suppliers = new EfGenericRepository<Supplier>(_ctx);
        }

        public async Task BeginTransactionAsync(CancellationToken ct)
        {
            _tx = await _ctx.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            await _ctx.SaveChangesAsync(ct);
            if (_tx != null)
            {
                await _tx.CommitAsync(ct);
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            if (_tx != null)
            {
                await _tx.RollbackAsync(ct);
                await _tx.DisposeAsync();
                _tx = null;
            }
        }

        public IDbConnection GetDbConnection()
        {
            return _ctx.Database.GetDbConnection();
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_tx != null)
                await _tx.DisposeAsync();
            await _ctx.DisposeAsync();
        }
    }
}