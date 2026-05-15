using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.VerifyInvoice;

public class VerifyInvoiceCommandValidator : AbstractValidator<VerifyInvoiceCommand>
{
    public VerifyInvoiceCommandValidator()
    {
        RuleFor(x => x.InvoiceId)
            .NotEmpty().WithMessage("InvoiceId is required.");

        RuleFor(x => x.ManagerUserId)
            .NotEmpty().WithMessage("ManagerUserId is required.");
    }
}
