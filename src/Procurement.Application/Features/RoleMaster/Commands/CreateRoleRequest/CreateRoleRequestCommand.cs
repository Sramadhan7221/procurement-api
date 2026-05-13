using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMaster.Commands.CreateRoleRequest;

public record CreateRoleRequestCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
}
