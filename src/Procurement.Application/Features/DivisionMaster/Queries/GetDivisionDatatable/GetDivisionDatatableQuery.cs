using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.DivisionMaster.Queries.GetDivisionDatatable;

public record GetDivisionDatatableQuery(DatatableRequest Request)
    : IRequest<Result<DatatableResponse<DivisionViewModel>>>;
