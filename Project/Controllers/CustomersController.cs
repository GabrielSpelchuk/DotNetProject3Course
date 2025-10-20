using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Customers;
using Project.BLL.Query;
using Project.BLL.Services;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll([FromQuery] QueryParams qp, CancellationToken ct)
        {
            var customers = await _customerService.GetAllAsync(qp, ct);
            return Ok(customers);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken ct)
        {
            var customer = await _customerService.GetByIdAsync(id, ct);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCustomerDto dto, CancellationToken ct)
        {
            var newId = await _customerService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto, CancellationToken ct)
        {
            try
            {
                await _customerService.UpdateAsync(id, dto, ct);
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
                await _customerService.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }