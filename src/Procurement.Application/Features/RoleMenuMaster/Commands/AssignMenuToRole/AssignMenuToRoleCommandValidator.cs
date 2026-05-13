using FluentValidation;

namespace Procurement.Application.Features.RoleMenuMaster.Commands.AssignMenuToRole;

public class AssignMenuToRoleCommandValidator : AbstractValidator<AssignMenuToRoleCommand>
{
    public AssignMenuToRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.");

        RuleFor(x => x.MenuId)
            .NotEmpty().WithMessage("Menu ID is required.");
    }
}
