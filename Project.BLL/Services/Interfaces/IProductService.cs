using Project.BLL.Dtos.Products;
using Project.BLL.Query;

namespace Project.BLL.Services.Interfaces;

public interface IProductService
{
    Task<int> CreateAsync(CreateProductDto dto, CancellationToken ct);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
}