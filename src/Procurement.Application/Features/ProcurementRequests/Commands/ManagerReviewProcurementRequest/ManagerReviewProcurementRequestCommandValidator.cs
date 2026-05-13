using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ManagerReviewProcurementRequest;

public class ManagerReviewProcurementRequestCommandValidator
    : AbstractValidator<ManagerReviewProcurementRequestCommand>
{
    public ManagerReviewProcurementRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Procurement request Id is required.");

        RuleFor(x => x.ManagerUserId)
            .NotEmpty().WithMessage("ManagerUserId is required.");

        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");
    }
}
