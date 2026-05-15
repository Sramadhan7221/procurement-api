using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ResolveInvoiceDispute;

public class ResolveInvoiceDisputeCommandValidator : AbstractValidator<ResolveInvoiceDisputeCommand>
{
    public ResolveInvoiceDisputeCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("InvoiceId is required.");

        RuleFor(x => x.AdminUserId)
            .NotEmpty().WithMessage("AdminUserId is required.");

        RuleFor(x => x.Resolution)
            .IsInEnum().WithMessage("Resolution must be Accept or Reject.");

        RuleFor(x => x.Note)
            .MaximumLength(1000).WithMessage("Note must not exceed 1000 characters.");
    }
}
