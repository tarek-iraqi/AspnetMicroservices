using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
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