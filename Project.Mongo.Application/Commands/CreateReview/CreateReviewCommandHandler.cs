using MediatR;
using Project.Mongo.Domain.Entities;
using Project.Mongo.Domain.ValueObjects;
using Project.Mongo.Infrastructure.Mongo;

namespace Project.Mongo.Application.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, string>
    {
        private readonly ReviewRepository _repo;
        public CreateReviewCommandHandler(ReviewRepository repo) => _repo = repo;

        public async Task<string> Handle(CreateReviewCommand request, CancellationToken ct)
        {
            var review = new Review
            {
                ProductId = request.ProductId,
                CustomerEmail = new Email(request.Email),
                Rating = new Rating(request.Rating),
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(review, ct);
            return review.Id;
        }
    }
}
