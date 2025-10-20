namespace Project.Domain.Entities;

public class Supplier
{
    public int SupplierId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public decimal Rating { get; set; }
}