using FluentValidation;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApproveProcurementRequest;

public class ApproveProcurementRequestCommandValidator : AbstractValidator<ApproveProcurementRequestCommand>
{
    public ApproveProcurementRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
