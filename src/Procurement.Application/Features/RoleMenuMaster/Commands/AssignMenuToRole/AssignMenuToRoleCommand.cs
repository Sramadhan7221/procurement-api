using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMenuMaster.Commands.AssignMenuToRole;

public record AssignMenuToRoleCommand : IRequest<Result<Guid>>
{
    public Guid RoleId { get; init; }
    public Guid MenuId { get; init; }
}
