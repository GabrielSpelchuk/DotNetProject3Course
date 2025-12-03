using MediatR;

namespace Project.Mongo.Application.Commands.CreateReview
{
    public record CreateReviewCommand(int ProductId, string Email, int Rating, string Comment) : IRequest<string>;
}
