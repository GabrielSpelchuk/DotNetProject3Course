namespace Project.BLL.Dtos.Orders;

public class PaymentDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}