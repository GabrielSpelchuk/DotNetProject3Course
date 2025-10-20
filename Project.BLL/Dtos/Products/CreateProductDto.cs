namespace Project.BLL.Dtos.Products;

public class CreateProductDto
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}