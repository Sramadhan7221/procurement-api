using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ConfirmGoodsReceipt;

public class ConfirmGoodsReceiptCommandHandler : IRequestHandler<ConfirmGoodsReceiptCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;
    private readonly IFileService _fileService;

    public ConfirmGoodsReceiptCommandHandler(
        IApplicationDbContext context,
        IAuditTrailWriter auditTrail,
        IFileService fileService)
    {
        _context = context;
        _auditTrail = auditTrail;
        _fileService = fileService;
    }

    public async Task<Result<Guid>> Handle(ConfirmGoodsReceiptCommand request, CancellationToken cancellationToken)
    {
        var staff = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.ReceivedByUserId, cancellationToken);

        if (staff is null)
            throw new NotFoundException(nameof(User), request.ReceivedByUserId);

        var procurement = await _context.ProcurementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == request.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementId);

        var fromStatus = procurement.Status;
        procurement.ConfirmGoodsReceipt();

        string? deliveryOrderPath = null;
        if (request.DeliveryOrderFile is not null)
        {
            var folder = $"procurement/{request.ProcurementId}/goods-receipt";
            deliveryOrderPath = await _fileService.UploadFileAsync(request.DeliveryOrderFile, folder);
        }

        var receipt = new GoodsReceipt
        {
            ProcurementId = request.ProcurementId,
            ReceivedByUserId = request.ReceivedByUserId,
            ReceivedAt = DateTime.UtcNow,
            DeliveryOrderFile = deliveryOrderPath,
            Notes = request.Notes,
        };

        foreach (var itemDto in request.Items)
        {
            var procItem = procurement.Items.FirstOrDefault(i => i.Id == itemDto.ProcurementItemId);
            if (procItem is null)
                throw new NotFoundException(nameof(ProcurementItem), itemDto.ProcurementItemId);

            receipt.Items.Add(new GoodsReceiptItem
            {
                ProcurementItemId = itemDto.ProcurementItemId,
                OrderedQuantity = procItem.Quantity,
                ReceivedQuantity = itemDto.ReceivedQuantity,
                IsPartialReceipt = itemDto.ReceivedQuantity < procItem.Quantity,
            });
        }

        _context.GoodsReceipts.Add(receipt);

        await _auditTrail.WriteAsync(
            request.ProcurementId,
            request.ReceivedByUserId,
            staff.Name,
            staff.Role.Name,
            "Goods Received",
            fromStatus,
            ProcurementStatus.OrderReceived,
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(receipt.Id, "Goods receipt confirmed successfully.");
    }
}
