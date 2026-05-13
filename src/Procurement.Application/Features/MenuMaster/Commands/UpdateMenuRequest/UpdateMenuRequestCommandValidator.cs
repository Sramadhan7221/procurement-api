using FluentValidation;

namespace Procurement.Application.Features.MenuMaster.Commands.UpdateMenuRequest;

public class UpdateMenuRequestCommandValidator : AbstractValidator<UpdateMenuRequestCommand>
{
    public UpdateMenuRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Menu ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.Url)
            .MaximumLength(200).WithMessage("Url must not exceed 200 characters.")
            .When(x => x.Url is not null);

        RuleFor(x => x.Icon)
            .MaximumLength(100).WithMessage("Icon must not exceed 100 characters.")
            .When(x => x.Icon is not null);

        RuleFor(x => x.Sequence)
            .GreaterThanOrEqualTo(0).WithMessage("Sequence must be a non-negative number.");
    }
}
