using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UploadInvoice;

public class UploadInvoiceCommandValidator : AbstractValidator<UploadInvoiceCommand>
{
    private static readonly string[] AllowedPdfContentTypes = ["application/pdf"];
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    public UploadInvoiceCommandValidator()
    {
        RuleFor(x => x.ProcurementId)
            .NotEmpty().WithMessage("ProcurementId is required.");

        RuleFor(x => x.UploadedByUserId)
            .NotEmpty().WithMessage("UploadedByUserId is required.");

        RuleFor(x => x.VendorInvoiceNumber)
            .NotEmpty().WithMessage("VendorInvoiceNumber is required.")
            .MaximumLength(100).WithMessage("VendorInvoiceNumber must not exceed 100 characters.");

        RuleFor(x => x.VendorInvoiceDate)
            .NotEmpty().WithMessage("VendorInvoiceDate is required.");

        RuleFor(x => x.InvoiceFile)
            .NotNull().WithMessage("Invoice file is required.")
            .Must(f => f.Length <= MaxFileSizeBytes)
                .WithMessage("Invoice file must not exceed 10MB.")
            .Must(f => AllowedPdfContentTypes.Contains(f.ContentType))
                .WithMessage("Invoice file must be a PDF.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("At least one invoice item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProcurementItemId)
                .NotEmpty().WithMessage("ProcurementItemId is required.");

            item.RuleFor(i => i.InvoicedQuantity)
                .GreaterThan(0).WithMessage("InvoicedQuantity must be greater than 0.");

            item.RuleFor(i => i.InvoiceUnitPrice)
                .GreaterThan(0).WithMessage("InvoiceUnitPrice must be greater than 0.");
        });
    }
}
