using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.BLL.Dtos.OrderDtos;

namespace Project.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct);
        Task<OrderDto> GetOrderAsync(int orderId, CancellationToken ct);
        Task ConfirmOrderAsync(int orderId, CancellationToken ct);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId, CancellationToken ct);
    }
}
