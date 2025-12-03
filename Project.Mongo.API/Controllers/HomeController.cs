using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.Mongo.Application.Commands.CreateReview;
using Project.Mongo.Application.Queries.GetReviewsByProduct;

namespace Project.Mongo.API.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewCommand cmd, CancellationToken ct)
        {
            var id = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetByProduct), new { productId = cmd.ProductId }, new { id });
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetByProduct(int productId, int page = 1, int pageSize = 20, CancellationToken ct = default)
        {
            var result = await _mediator.Send(new GetReviewsByProductQuery(productId, page, pageSize), ct);
            return Ok(result);
        }

        [HttpGet("{productId:int}/stats")]
        public async Task<IActionResult> GetStats(int productId, [FromServices] Project.Mongo.Infrastructure.Mongo.ReviewRepository repo, CancellationToken ct)
        {
            var (avg, count) = await repo.GetStatsAsync(productId, ct);
            return Ok(new { average = avg, count });
        }
    }
}
