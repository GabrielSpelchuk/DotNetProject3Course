namespace Project.Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    public ProductDetail? Detail { get; set; }
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}