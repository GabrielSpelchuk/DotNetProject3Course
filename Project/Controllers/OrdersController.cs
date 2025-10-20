using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Customers;
using Project.BLL.Dtos.Orders;
using Project.BLL.Query;
using Project.BLL.Services;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll([FromQuery] QueryParams qp, CancellationToken ct)
        {
            var orders = await _orderService.GetAllAsync(qp, ct);
            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken ct)
        {
            var order = await _orderService.GetByIdAsync(id, ct);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
        {
            var newId = await _orderService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto dto, CancellationToken ct)
        {
            try
            {
                await _orderService.UpdateAsync(id, dto, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                await _orderService.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id, CancellationToken ct)
        {
            try
            {
                await _orderService.ConfirmAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }