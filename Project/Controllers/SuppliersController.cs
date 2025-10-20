using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Suppliers;
using Project.BLL.Query;
using Project.BLL.Services;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SuppliersController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAll([FromQuery] QueryParams qp, CancellationToken ct)
        {
            var suppliers = await _supplierService.GetAllAsync(qp, ct);
            return Ok(suppliers);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetById(int id, CancellationToken ct)
        {
            var supplier = await _supplierService.GetByIdAsync(id, ct);

            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateSupplierDto dto, CancellationToken ct)
        {
            var newId = await _supplierService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto, CancellationToken ct)
        {
            try
            {
                await _supplierService.UpdateAsync(id, dto, ct);
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
                await _supplierService.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}