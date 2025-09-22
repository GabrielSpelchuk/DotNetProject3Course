using Microsoft.AspNetCore.Mvc;
using Project.DAL.Entities;
using Project.DAL.Repositories.Interfaces;
using Project.DAL.Repositories;
using static Project.BLL.Dtos.CustomerDtos;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IDbConnectionFactory _connFactory;
    public CustomersController(IDbConnectionFactory connFactory) { _connFactory = connFactory; }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        await using var uow = new UnitOfWork(_connFactory);
        await uow.BeginTransactionAsync();
        try
        {
            var cust = new Customer { Name = dto.Name, Email = dto.Email };
            var id = await uow.Customers.CreateAsync(cust, ct);
            await uow.CommitAsync();
            var created = await uow.Customers.GetByIdAsync(id, ct);
            return CreatedAtAction(nameof(Get), new { id = id }, new CustomerDto(created.CustomerId, created.Name, created.Email, created.CreatedAt));
        }
        catch (Exception ex)
        {
            await uow.RollbackAsync();
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> Get(int id, CancellationToken ct)
    {
        await using var uow = new UnitOfWork(_connFactory);
        await uow.BeginTransactionAsync();
        var cust = await uow.Customers.GetByIdAsync(id, ct);
        if (cust == null) return NotFound();
        return new CustomerDto(cust.CustomerId, cust.Name, cust.Email, cust.CreatedAt);
    }
}
