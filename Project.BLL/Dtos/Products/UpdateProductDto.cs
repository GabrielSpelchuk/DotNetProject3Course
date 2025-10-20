namespace Project.BLL.Dtos.Products;

public class UpdateProductDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public bool? InStock { get; set; }
}