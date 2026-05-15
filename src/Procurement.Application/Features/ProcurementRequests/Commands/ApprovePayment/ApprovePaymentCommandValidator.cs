using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApprovePayment;

public class ApprovePaymentCommandValidator : AbstractValidator<ApprovePaymentCommand>
{
    public ApprovePaymentCommandValidator()
    {
        RuleFor(x => x.ProcurementId)
            .NotEmpty().WithMessage("ProcurementId is required.");

        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("InvoiceId is required.");

        RuleFor(x => x.ManagerUserId)
            .NotEmpty().WithMessage("ManagerUserId is required.");
    }
}
