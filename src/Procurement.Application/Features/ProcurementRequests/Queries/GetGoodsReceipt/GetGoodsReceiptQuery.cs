using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetGoodsReceipt;

public record GetGoodsReceiptQuery(Guid ProcurementId) : IRequest<Result<GoodsReceiptDto>>;

public record GoodsReceiptDto(
    Guid Id,
    Guid ProcurementId,
    Guid ReceivedByUserId,
    string ReceivedByName,
    DateTime ReceivedAt,
    string? DeliveryOrderFile,
    string? Notes,
    IReadOnlyList<GoodsReceiptItemDto> Items);

public record GoodsReceiptItemDto(
    Guid Id,
    Guid ProcurementItemId,
    string ItemName,
    decimal OrderedQuantity,
    decimal ReceivedQuantity,
    bool IsPartialReceipt);
