using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMaster.Commands.UpdateRoleRequest;

public record UpdateRoleRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
