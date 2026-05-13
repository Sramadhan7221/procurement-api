using FluentValidation;

namespace Procurement.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".pdf"];
    private const long MaxFileSizeBytes = 2 * 1024 * 1024;

    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.ProcurementRequestId)
            .NotEmpty().WithMessage("ProcurementRequestId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.PaymentDate)
            .NotEmpty().WithMessage("PaymentDate is required.");

        When(x => x.AttachmentFile != null, () =>
        {
            RuleFor(x => x.AttachmentFile!)
                .Must(f => f.Length <= MaxFileSizeBytes)
                .WithMessage("File size must not exceed 2MB.")
                .Must(f => AllowedExtensions.Contains(
                    Path.GetExtension(f.FileName).ToLowerInvariant()))
                .WithMessage("Only .jpg, .jpeg, .png, and .pdf files are allowed.");
        });
    }
}
