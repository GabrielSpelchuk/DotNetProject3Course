using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.BLL.Dtos.OrderItemDtos;

namespace Project.BLL.Dtos
{
    public class OrderDtos
    {
        public record OrderDto(int OrderId, int CustomerId, int? SupplierId, string Status, DateTime CreatedAt, List<OrderItemDto> Items);
        public record CreateOrderDto(int CustomerId, int SupplierId, List<OrderItemDto> Items);
    }
}
