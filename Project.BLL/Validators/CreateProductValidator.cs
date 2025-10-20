using FluentValidation;
using Project.BLL.Dtos.Products;

namespace Project.BLL.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Назва продукту обов’язкова")
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Ціна не може бути від’ємною");
    }
}