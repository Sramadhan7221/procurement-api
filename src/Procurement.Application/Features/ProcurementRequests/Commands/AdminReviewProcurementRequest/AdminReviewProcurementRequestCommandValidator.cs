using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.AdminReviewProcurementRequest;

public class AdminReviewProcurementRequestCommandValidator
    : AbstractValidator<AdminReviewProcurementRequestCommand>
{
    public AdminReviewProcurementRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Procurement request Id is required.");

        RuleFor(x => x.AdminUserId)
            .NotEmpty().WithMessage("AdminUserId is required.");

        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");
    }
}
