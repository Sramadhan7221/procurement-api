using FluentValidation;

namespace Procurement.Application.Features.DivisionMaster.Commands.UpdateDivisionRequest;

public class UpdateDivisionRequestCommandValidator : AbstractValidator<UpdateDivisionRequestCommand>
{
    public UpdateDivisionRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Division ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}  