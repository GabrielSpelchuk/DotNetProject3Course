namespace Project.Domain.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
}