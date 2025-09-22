using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(Order order, CancellationToken ct);
        Task<Order> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId, CancellationToken ct);
        Task AddItemsAsync(int orderId, IEnumerable<OrderItem> items, CancellationToken ct);
    }
}
