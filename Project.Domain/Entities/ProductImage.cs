namespace Project.Domain.Entities;

public class ProductImage
{
    public int ImageId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}