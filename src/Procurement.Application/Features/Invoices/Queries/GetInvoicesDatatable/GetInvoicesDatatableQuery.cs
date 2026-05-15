using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.Invoices.Queries.GetInvoicesDatatable;

public record GetInvoicesDatatableQuery : IRequest<DatatableResponse<InvoiceListItemDto>>
{
    public int Draw { get; init; }
    public int Start { get; init; }
    public int Length { get; init; }
    public string? Search { get; init; }
}
