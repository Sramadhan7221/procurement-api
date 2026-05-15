using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestDatatable;

public record GetProcurementRequestDatatableQuery : IRequest<Result<DatatableResponse<ProcurementRequestListItemDto>>>
{
    public Guid UserId { get; init; }
    public int Draw { get; init; }
    public int Start { get; init; }
    public int Length { get; init; }
    public string? Search { get; init; }
}
