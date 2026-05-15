using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ConfirmGoodsReceipt;

public record ConfirmGoodsReceiptCommand : IRequest<Result<Guid>>
{
    public Guid ProcurementId { get; init; }
    public Guid ReceivedByUserId { get; init; }
    public IFormFile? DeliveryOrderFile { get; init; }
    public string? Notes { get; init; }
    public IReadOnlyList<GoodsReceiptItemDto> Items { get; init; } = [];
}

public record GoodsReceiptItemDto(Guid ProcurementItemId, decimal ReceivedQuantity);
