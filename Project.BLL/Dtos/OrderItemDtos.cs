using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos
{
    public class OrderItemDtos
    {
        public record OrderItemDto(int ProductId, int Quantity, decimal UnitPrice);
    }
}
