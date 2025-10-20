using FluentValidation;
using Project.BLL.Dtos.Orders;

namespace Project.BLL.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Потрібно вказати клієнта");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Замовлення не може бути порожнім");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("Потрібно вказати товар");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Кількість має бути більшою за 0");
        });
    }
}