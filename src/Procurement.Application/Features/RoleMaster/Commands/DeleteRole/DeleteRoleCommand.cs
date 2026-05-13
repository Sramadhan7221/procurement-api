using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMaster.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<Result<Guid>>;
