using FluentValidation;

namespace Procurement.Application.Features.DivisionMaster.Commands;

public class CreateDivisionRequestCommandValidator : AbstractValidator<CreateDivisionRequestCommand>
{
    public CreateDivisionRequestCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");
    }
}  