using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.VerifyInvoice;

public record VerifyInvoiceCommand : IRequest<Result<VerifyInvoiceResult>>
{
    public Guid InvoiceId { get; init; }
    public Guid ManagerUserId { get; init; }
}

public record VerifyInvoiceResult(Guid InvoiceId, DateTime VerifiedAt);
