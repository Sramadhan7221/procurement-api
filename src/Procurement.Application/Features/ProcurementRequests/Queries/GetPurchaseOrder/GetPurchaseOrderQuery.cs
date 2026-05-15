using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetPurchaseOrder;

public record GetPurchaseOrderQuery(Guid ProcurementId) : IRequest<Result<PurchaseOrderDto>>;

public record PurchaseOrderDto(
    Guid Id,
    string PoNumber,
    Guid ProcurementId,
    Guid GeneratedByUserId,
    string GeneratedByName,
    DateTime GeneratedAt,
    string? FilePath,
    bool SentToVendor,
    DateTime? SentAt,
    IReadOnlyList<PurchaseOrderItemDto> Items);

public record PurchaseOrderItemDto(
    Guid Id,
    string ItemName,
    int Quantity,
    string UoM,
    decimal UnitPrice,
    decimal SubTotal);
