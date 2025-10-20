using Project.BLL.Dtos.Customers;
using Project.BLL.Dtos.Orders;
using Project.BLL.Query;


namespace Project.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync(QueryParams qp, CancellationToken ct);
        Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<int> CreateAsync(CreateOrderDto dto, CancellationToken ct);
        Task UpdateAsync(int id, UpdateOrderDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task ConfirmOrderAsync(int orderId, CancellationToken ct);
    }
}
