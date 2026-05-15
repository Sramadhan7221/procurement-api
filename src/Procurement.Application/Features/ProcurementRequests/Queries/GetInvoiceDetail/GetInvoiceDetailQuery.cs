using MediatR;
using Procurement.Application.Common;
using Procurement.Domain.Enums;
using Procurement.Domain.Services;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetInvoiceDetail;

public record GetInvoiceDetailQuery(Guid ProcurementId) : IRequest<Result<InvoiceDetailDto>>;

public record InvoiceDetailDto(
    Guid Id,
    Guid ProcurementId,
    string VendorInvoiceNumber,
    DateOnly VendorInvoiceDate,
    string FilePath,
    Guid UploadedByUserId,
    string UploadedByName,
    DateTime UploadedAt,
    InvoiceMatchingStatus MatchingStatus,
    string? DisputeNote,
    Guid? VerifiedByUserId,
    string? VerifiedByName,
    DateTime? VerifiedAt,
    IReadOnlyList<InvoiceMatchingLineDto> MatchingLines);

public record InvoiceMatchingLineDto(
    string ItemName,
    decimal OrderedQty,
    decimal ReceivedQty,
    decimal InvoicedQty,
    decimal UnitPrice,
    decimal InvoicedUnitPrice,
    bool DiscrepancyFlag,
    string? DiscrepancyType);
