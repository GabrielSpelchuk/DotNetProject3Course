using MediatR;
using Project.Mongo.Infrastructure.Mongo;


namespace Project.Mongo.Application.Queries.GetReviewsByProduct
{
    public class GetReviewsByProductQueryHandler : IRequestHandler<GetReviewsByProductQuery, IEnumerable<Project.Mongo.Domain.Entities.Review>>
    {
        private readonly ReviewRepository _repo;
        public GetReviewsByProductQueryHandler(ReviewRepository repo) => _repo = repo;

        public async Task<IEnumerable<Project.Mongo.Domain.Entities.Review>> Handle(GetReviewsByProductQuery request, CancellationToken ct)
        {
            return await _repo.GetByProductAsync(request.ProductId, request.Page, request.PageSize, ct);
        }
    }
}
