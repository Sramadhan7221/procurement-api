using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetPurchaseOrder;

public class GetPurchaseOrderQueryHandler : IRequestHandler<GetPurchaseOrderQuery, Result<PurchaseOrderDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPurchaseOrderQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PurchaseOrderDto>> Handle(GetPurchaseOrderQuery request, CancellationToken cancellationToken)
    {
        var po = await _context.PurchaseOrders
            .Include(p => p.GeneratedBy)
            .Include(p => p.ProcurementRequest)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(p => p.ProcurementId == request.ProcurementId, cancellationToken);

        if (po is null)
            throw new NotFoundException("PurchaseOrder", $"procurement {request.ProcurementId}");

        var items = po.ProcurementRequest.Items
            .Select(i => new PurchaseOrderItemDto(i.Id, i.ItemName, i.Quantity, i.UoM, i.UnitPrice, i.SubTotal))
            .ToList();

        var dto = new PurchaseOrderDto(
            po.Id,
            po.PoNumber,
            po.ProcurementId,
            po.GeneratedByUserId,
            po.GeneratedBy.Name,
            po.GeneratedAt,
            po.FilePath,
            po.SentToVendor,
            po.SentAt,
            items.AsReadOnly());

        return Result<PurchaseOrderDto>.Success(dto);
    }
}
