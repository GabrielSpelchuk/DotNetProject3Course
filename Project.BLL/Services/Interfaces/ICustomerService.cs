using Project.BLL.Dtos.Customers;

namespace Project.BLL.Services.Interfaces;

public interface ICustomerService
{
    Task<int> CreateAsync(CreateCustomerDto dto, CancellationToken ct);
    Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct);
}