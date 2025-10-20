namespace Project.Domain.Entities;

public class ProductDetail
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string? Description { get; set; }
    public string? ShippingTime { get; set; }
    public string? ReturnPolicy { get; set; }
}