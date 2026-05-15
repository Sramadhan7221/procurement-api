using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetGoodsReceipt;

public class GetGoodsReceiptQueryHandler : IRequestHandler<GetGoodsReceiptQuery, Result<GoodsReceiptDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGoodsReceiptQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GoodsReceiptDto>> Handle(GetGoodsReceiptQuery request, CancellationToken cancellationToken)
    {
        var receipt = await _context.GoodsReceipts
            .Include(gr => gr.ReceivedBy)
            .Include(gr => gr.Items)
                .ThenInclude(i => i.ProcurementItem)
            .FirstOrDefaultAsync(gr => gr.ProcurementId == request.ProcurementId, cancellationToken);

        if (receipt is null)
            throw new NotFoundException("GoodsReceipt", $"procurement {request.ProcurementId}");

        var items = receipt.Items
            .Select(i => new GoodsReceiptItemDto(
                i.Id,
                i.ProcurementItemId,
                i.ProcurementItem.ItemName,
                i.OrderedQuantity,
                i.ReceivedQuantity,
                i.IsPartialReceipt))
            .ToList();

        var dto = new GoodsReceiptDto(
            receipt.Id,
            receipt.ProcurementId,
            receipt.ReceivedByUserId,
            receipt.ReceivedBy.Name,
            receipt.ReceivedAt,
            receipt.DeliveryOrderFile,
            receipt.Notes,
            items.AsReadOnly());

        return Result<GoodsReceiptDto>.Success(dto);
    }
}
