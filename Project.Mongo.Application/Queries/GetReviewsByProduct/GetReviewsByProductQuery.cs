using MediatR;
using Project.Mongo.Domain.Entities;

namespace Project.Mongo.Application.Queries.GetReviewsByProduct
{
    public record GetReviewsByProductQuery(int ProductId, int Page = 1, int PageSize = 20) : IRequest<IEnumerable<Review>>;
}
