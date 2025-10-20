namespace Project.BLL.Dtos.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public int SupplierId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}