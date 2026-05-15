using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;
using Procurement.Domain.Services;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UploadInvoice;

public class UploadInvoiceCommandHandler : IRequestHandler<UploadInvoiceCommand, Result<UploadInvoiceResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;
    private readonly IFileService _fileService;
    private readonly decimal _priceTolerance;

    public UploadInvoiceCommandHandler(
        IApplicationDbContext context,
        IAuditTrailWriter auditTrail,
        IFileService fileService,
        IConfiguration configuration)
    {
        _context = context;
        _auditTrail = auditTrail;
        _fileService = fileService;
        _priceTolerance = configuration.GetValue<decimal>("ThreeWayMatching:PriceTolerance", 0m);
    }

    public async Task<Result<UploadInvoiceResult>> Handle(UploadInvoiceCommand request, CancellationToken cancellationToken)
    {
        var uploader = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UploadedByUserId, cancellationToken);

        if (uploader is null)
            throw new NotFoundException(nameof(User), request.UploadedByUserId);

        var procurement = await _context.ProcurementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == request.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementId);

        var goodsReceipt = await _context.GoodsReceipts
            .Include(gr => gr.Items)
            .FirstOrDefaultAsync(gr => gr.ProcurementId == request.ProcurementId, cancellationToken);

        if (goodsReceipt is null)
            throw new DomainException("Goods receipt must be confirmed before uploading an invoice.");

        var fromStatus = procurement.Status;
        procurement.UploadInvoice();

        var folder = $"procurement/{request.ProcurementId}/invoice";
        var filePath = await _fileService.UploadFileAsync(request.InvoiceFile, folder);

        var invoice = new ProcurementInvoice
        {
            ProcurementId = request.ProcurementId,
            VendorInvoiceNumber = request.VendorInvoiceNumber,
            VendorInvoiceDate = request.VendorInvoiceDate,
            FilePath = filePath,
            UploadedByUserId = request.UploadedByUserId,
            UploadedAt = DateTime.UtcNow,
            MatchingStatus = InvoiceMatchingStatus.Pending,
        };

        foreach (var itemDto in request.Items)
        {
            invoice.Items.Add(new ProcurementInvoiceItem
            {
                ProcurementItemId = itemDto.ProcurementItemId,
                InvoicedQuantity = itemDto.InvoicedQuantity,
                InvoiceUnitPrice = itemDto.InvoiceUnitPrice,
            });
        }

        var matchingInputs = BuildMatchingInputs(request, procurement, goodsReceipt);
        var matchingResult = ThreeWayMatchingService.Match(matchingInputs, _priceTolerance);

        invoice.MatchingStatus = matchingResult.IsMatched
            ? InvoiceMatchingStatus.Matched
            : InvoiceMatchingStatus.Disputed;

        _context.ProcurementInvoices.Add(invoice);

        await _auditTrail.WriteAsync(
            request.ProcurementId,
            request.UploadedByUserId,
            uploader.Name,
            uploader.Role.Name,
            "Invoice Uploaded",
            fromStatus,
            ProcurementStatus.InvoiceUploaded,
            metadata: new { invoiceNumber = request.VendorInvoiceNumber },
            cancellationToken: cancellationToken);

        if (!matchingResult.IsMatched)
        {
            procurement.DisputeInvoice();

            await _auditTrail.WriteAsync(
                request.ProcurementId,
                request.UploadedByUserId,
                uploader.Name,
                uploader.Role.Name,
                "Invoice Disputed",
                ProcurementStatus.InvoiceUploaded,
                ProcurementStatus.InvoiceDisputed,
                comment: "3-way matching discrepancies detected.",
                cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<UploadInvoiceResult>.Success(
            new UploadInvoiceResult(invoice.Id, invoice.MatchingStatus, matchingResult.Discrepancies),
            "Invoice uploaded successfully.");
    }

    private static IEnumerable<MatchingInput> BuildMatchingInputs(
        UploadInvoiceCommand request,
        ProcurementRequest procurement,
        GoodsReceipt goodsReceipt)
    {
        foreach (var invoiceItem in request.Items)
        {
            var procItem = procurement.Items.FirstOrDefault(i => i.Id == invoiceItem.ProcurementItemId);
            if (procItem is null) continue;

            var receiptItem = goodsReceipt.Items.FirstOrDefault(i => i.ProcurementItemId == invoiceItem.ProcurementItemId);
            var receivedQty = receiptItem?.ReceivedQuantity ?? 0m;

            yield return new MatchingInput(
                procItem.Id,
                procItem.ItemName,
                procItem.Quantity,
                receivedQty,
                invoiceItem.InvoicedQuantity,
                procItem.UnitPrice,
                invoiceItem.InvoiceUnitPrice);
        }
    }
}
