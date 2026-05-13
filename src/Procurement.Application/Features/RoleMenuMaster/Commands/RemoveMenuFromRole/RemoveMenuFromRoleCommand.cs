using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMenuMaster.Commands.RemoveMenuFromRole;

public record RemoveMenuFromRoleCommand : IRequest<Result<Guid>>
{
    public Guid RoleId { get; init; }
    public Guid MenuId { get; init; }
}
