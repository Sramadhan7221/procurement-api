using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.Invoices.Queries.GetInvoiceById;

public record GetInvoiceByIdQuery(Guid Id) : IRequest<Result<InvoiceDetailDto>>;
