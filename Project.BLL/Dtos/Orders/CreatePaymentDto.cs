namespace Project.BLL.Dtos.Orders;

public class CreatePaymentDto
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
}