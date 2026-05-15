using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.PlaceOrder;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<PlaceOrderResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;

    public PlaceOrderCommandHandler(IApplicationDbContext context, IAuditTrailWriter auditTrail)
    {
        _context = context;
        _auditTrail = auditTrail;
    }

    public async Task<Result<PlaceOrderResult>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.AdminUserId, cancellationToken);

        if (admin is null)
            throw new NotFoundException(nameof(User), request.AdminUserId);

        if (admin.Role.Name != "Admin")
            throw new DomainException("Only Admin users can place purchase orders.");

        var procurement = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementId);

        var fromStatus = procurement.Status;
        procurement.PlaceOrder();

        var poNumber = await GeneratePoNumber(cancellationToken);
        var purchaseOrder = new PurchaseOrder
        {
            PoNumber = poNumber,
            ProcurementId = request.ProcurementId,
            GeneratedByUserId = request.AdminUserId,
            GeneratedAt = DateTime.UtcNow,
            SentToVendor = false,
        };

        _context.PurchaseOrders.Add(purchaseOrder);

        await _auditTrail.WriteAsync(
            request.ProcurementId,
            request.AdminUserId,
            admin.Name,
            admin.Role.Name,
            "Purchase Order Placed",
            fromStatus,
            ProcurementStatus.InOrderByAdmin,
            metadata: new { poNumber },
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<PlaceOrderResult>.Success(
            new PlaceOrderResult(poNumber, purchaseOrder.Id),
            "Purchase Order placed successfully.");
    }

    private async Task<string> GeneratePoNumber(CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var todayStr = today.ToString("yyyyMMdd");

        var count = await _context.PurchaseOrders
            .CountAsync(po => po.GeneratedAt.Date == today, cancellationToken);

        return $"PO-{todayStr}-{(count + 1):D4}";
    }
}
