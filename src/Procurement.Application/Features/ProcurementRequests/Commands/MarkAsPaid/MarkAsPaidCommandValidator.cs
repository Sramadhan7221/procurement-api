using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.MarkAsPaid;

public class MarkAsPaidCommandValidator : AbstractValidator<MarkAsPaidCommand>
{
    private static readonly string[] AllowedContentTypes =
        ["application/pdf", "image/jpeg", "image/png"];
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    public MarkAsPaidCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("PaymentId is required.");

        RuleFor(x => x.AdminUserId)
            .NotEmpty().WithMessage("AdminUserId is required.");

        RuleFor(x => x.PaymentReference)
            .NotEmpty().WithMessage("PaymentReference is required.")
            .MaximumLength(200).WithMessage("PaymentReference must not exceed 200 characters.");

        When(x => x.PaymentProofFile is not null, () =>
        {
            RuleFor(x => x.PaymentProofFile!.Length)
                .LessThanOrEqualTo(MaxFileSizeBytes).WithMessage("Payment proof file must not exceed 10MB.");

            RuleFor(x => x.PaymentProofFile!.ContentType)
                .Must(ct => AllowedContentTypes.Contains(ct))
                .WithMessage("Payment proof must be PDF, JPG, or PNG.");
        });
    }
}
