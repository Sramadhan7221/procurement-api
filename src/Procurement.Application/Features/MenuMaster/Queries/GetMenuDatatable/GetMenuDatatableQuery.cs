using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuDatatable;

public record GetMenuDatatableQuery(DatatableRequest Request)
    : IRequest<Result<DatatableResponse<MenuViewModel>>>;
