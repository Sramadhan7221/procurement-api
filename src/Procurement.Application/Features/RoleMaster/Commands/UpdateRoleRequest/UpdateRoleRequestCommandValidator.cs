using FluentValidation;

namespace Procurement.Application.Features.RoleMaster.Commands.UpdateRoleRequest;

public class UpdateRoleRequestCommandValidator : AbstractValidator<UpdateRoleRequestCommand>
{
    public UpdateRoleRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
    }
}
