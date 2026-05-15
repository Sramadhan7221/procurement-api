using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.PlaceOrder;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.ProcurementId)
            .NotEmpty().WithMessage("ProcurementId is required.");

        RuleFor(x => x.AdminUserId)
            .NotEmpty().WithMessage("AdminUserId is required.");
    }
}
