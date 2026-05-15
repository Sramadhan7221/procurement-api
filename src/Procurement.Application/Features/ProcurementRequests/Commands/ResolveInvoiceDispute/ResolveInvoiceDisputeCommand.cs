using MediatR;
using Procurement.Application.Common;
using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ResolveInvoiceDispute;

public record ResolveInvoiceDisputeCommand : IRequest<Result<InvoiceMatchingStatus>>
{
    public Guid InvoiceId { get; init; }
    public Guid AdminUserId { get; init; }
    public DisputeResolution Resolution { get; init; }
    public string? Note { get; init; }
}

public enum DisputeResolution { Accept, Reject }
