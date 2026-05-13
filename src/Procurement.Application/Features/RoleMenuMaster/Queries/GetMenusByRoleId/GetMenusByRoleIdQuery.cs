using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMenuMaster.Queries.GetMenusByRoleId;

public record GetMenusByRoleIdQuery(Guid RoleId) : IRequest<Result<List<RoleMenuViewModel>>>;
