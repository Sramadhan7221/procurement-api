using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.RoleMaster.Queries.GetRoleDatatable;

public record GetRoleDatatableQuery(DatatableRequest Request)
    : IRequest<Result<DatatableResponse<RoleViewModel>>>;
