using Project.BLL.Dtos.Orders;

namespace Project.BLL.Dtos.Customers;

public class UpdateOrderDto
{
    public string? Status { get; set; }
    public List<UpdateOrderItemDto>? Items { get; set; }
}