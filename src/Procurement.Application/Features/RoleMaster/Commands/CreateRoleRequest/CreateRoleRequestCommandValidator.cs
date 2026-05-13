using FluentValidation;

namespace Procurement.Application.Features.RoleMaster.Commands.CreateRoleRequest;

public class CreateRoleRequestCommandValidator : AbstractValidator<CreateRoleRequestCommand>
{
    public CreateRoleRequestCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
    }
}
