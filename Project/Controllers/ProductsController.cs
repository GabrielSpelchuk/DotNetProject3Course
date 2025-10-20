using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Products;
using Project.BLL.Query;
using Project.BLL.Services;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] QueryParams qp, CancellationToken ct)
        {
            var products = await _productService.GetAllAsync(qp, ct);
            return Ok(products);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            var newId = await _productService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken ct)
        {
            try
            {
                await _productService.UpdateAsync(id, dto, ct);
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
                await _productService.DeleteAsync(id, ct);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}