using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMaster.Queries.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IRequest<Result<RoleDetailDto>>;
