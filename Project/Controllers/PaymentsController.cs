using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Orders;
using Project.BLL.Services;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll(CancellationToken ct)
        {
            var payments = await _paymentService.GetAllAsync(ct);
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetById(int id, CancellationToken ct)
        {
            var payment = await _paymentService.GetByIdAsync(id, ct);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreatePaymentDto dto, CancellationToken ct)
        {
            var newId = await _paymentService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
    }
}