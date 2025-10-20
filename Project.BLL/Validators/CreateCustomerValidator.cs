using FluentValidation;
using Project.BLL.Dtos.Customers;

namespace Project.BLL.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ім’я клієнта є обов’язковим")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email є обов’язковим")
            .EmailAddress().WithMessage("Невірний формат email");
    }
}