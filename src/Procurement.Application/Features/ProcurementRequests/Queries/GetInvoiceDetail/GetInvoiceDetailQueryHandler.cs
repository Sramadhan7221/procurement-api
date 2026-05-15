using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;
using Procurement.Domain.Services;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetInvoiceDetail;

public class GetInvoiceDetailQueryHandler : IRequestHandler<GetInvoiceDetailQuery, Result<InvoiceDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInvoiceDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<InvoiceDetailDto>> Handle(GetInvoiceDetailQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.ProcurementInvoices
            .Include(i => i.UploadedBy)
            .Include(i => i.VerifiedBy)
            .Include(i => i.Items)
                .ThenInclude(ii => ii.ProcurementItem)
            .FirstOrDefaultAsync(i => i.ProcurementId == request.ProcurementId, cancellationToken);

        if (invoice is null)
            throw new NotFoundException("ProcurementInvoice", $"procurement {request.ProcurementId}");

        var goodsReceipt = await _context.GoodsReceipts
            .Include(gr => gr.Items)
            .FirstOrDefaultAsync(gr => gr.ProcurementId == request.ProcurementId, cancellationToken);

        var matchingLines = invoice.Items.Select(invoiceItem =>
        {
            var procItem = invoiceItem.ProcurementItem;
            var receiptItem = goodsReceipt?.Items.FirstOrDefault(ri => ri.ProcurementItemId == invoiceItem.ProcurementItemId);
            var receivedQty = receiptItem?.ReceivedQuantity ?? 0m;

            var matchingInput = new MatchingInput(
                procItem.Id,
                procItem.ItemName,
                procItem.Quantity,
                receivedQty,
                invoiceItem.InvoicedQuantity,
                procItem.UnitPrice,
                invoiceItem.InvoiceUnitPrice);

            var result = ThreeWayMatchingService.Match([matchingInput]);
            var hasDiscrepancy = !result.IsMatched;
            var discrepancyType = hasDiscrepancy ? result.Discrepancies[0].DiscrepancyType.ToString() : null;

            return new InvoiceMatchingLineDto(
                procItem.ItemName,
                procItem.Quantity,
                receivedQty,
                invoiceItem.InvoicedQuantity,
                procItem.UnitPrice,
                invoiceItem.InvoiceUnitPrice,
                hasDiscrepancy,
                discrepancyType);
        }).ToList();

        var dto = new InvoiceDetailDto(
            invoice.Id,
            invoice.ProcurementId,
            invoice.VendorInvoiceNumber,
            invoice.VendorInvoiceDate,
            invoice.FilePath,
            invoice.UploadedByUserId,
            invoice.UploadedBy.Name,
            invoice.UploadedAt,
            invoice.MatchingStatus,
            invoice.DisputeNote,
            invoice.VerifiedByUserId,
            invoice.VerifiedBy?.Name,
            invoice.VerifiedAt,
            matchingLines.AsReadOnly());

        return Result<InvoiceDetailDto>.Success(dto);
    }
}
