using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.PlaceOrder;

public record PlaceOrderCommand : IRequest<Result<PlaceOrderResult>>
{
    public Guid ProcurementId { get; init; }
    public Guid AdminUserId { get; init; }
}

public record PlaceOrderResult(string PoNumber, Guid PurchaseOrderId);
