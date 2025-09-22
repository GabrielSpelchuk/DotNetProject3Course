using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
