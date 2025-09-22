using Microsoft.AspNetCore.Mvc;
using Project.BLL.Services.Interfaces;
using static Project.BLL.Dtos.OrderDtos;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    public OrdersController(IOrderService service) { _service = service; }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        var id = await _service.CreateOrderAsync(dto, ct);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> Get(int id, CancellationToken ct)
    {
        var order = await _service.GetOrderAsync(id, ct);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> Confirm(int id, CancellationToken ct)
    {
        await _service.ConfirmOrderAsync(id, ct);
        return NoContent();
    }

    [HttpGet("customer/{customerId:int}")]
    public async Task<ActionResult> GetByCustomer(int customerId, CancellationToken ct)
    {
        var list = await _service.GetOrdersByCustomerAsync(customerId, ct);
        return Ok(list);
    }
}
