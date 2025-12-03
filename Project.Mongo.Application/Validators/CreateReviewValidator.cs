using FluentValidation;
using Project.Mongo.Application.Commands.CreateReview;

namespace Project.Mongo.Application.Validators
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Comment).MaximumLength(1000);
            RuleFor(x => x.ProductId).GreaterThan(0);
        }
    }
}
