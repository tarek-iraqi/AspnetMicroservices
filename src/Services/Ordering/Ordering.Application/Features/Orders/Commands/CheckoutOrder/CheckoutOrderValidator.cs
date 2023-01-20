using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderValidator : AbstractValidator<CheckoutOrderCommand>
{
	public CheckoutOrderValidator()
	{
		RuleFor(x => x.UserName)
			.NotEmpty()
			.WithMessage("{UserName} is required")
			.MaximumLength(50)
			.WithMessage("{UserName} has max length 50");

		RuleFor(x => x.EmailAddress)
			.NotEmpty()
			.WithMessage("{EmailAddress} is required");

		RuleFor(x => x.TotalPrice)
			.NotEmpty()
			.WithMessage("{TotalPrice} is required");
	}
}
