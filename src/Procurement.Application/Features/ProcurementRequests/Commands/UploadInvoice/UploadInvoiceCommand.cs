using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Common;
using Procurement.Domain.Enums;
using Procurement.Domain.Services;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UploadInvoice;

public record UploadInvoiceCommand : IRequest<Result<UploadInvoiceResult>>
{
    public Guid ProcurementId { get; init; }
    public Guid UploadedByUserId { get; init; }
    public string VendorInvoiceNumber { get; init; } = string.Empty;
    public DateOnly VendorInvoiceDate { get; init; }
    public IFormFile InvoiceFile { get; init; } = null!;
    public IReadOnlyList<InvoiceItemInputDto> Items { get; init; } = [];
}

public record InvoiceItemInputDto(Guid ProcurementItemId, decimal InvoicedQuantity, decimal InvoiceUnitPrice);

public record UploadInvoiceResult(
    Guid InvoiceId,
    InvoiceMatchingStatus MatchingStatus,
    IReadOnlyList<DiscrepancyDetail> Discrepancies);
