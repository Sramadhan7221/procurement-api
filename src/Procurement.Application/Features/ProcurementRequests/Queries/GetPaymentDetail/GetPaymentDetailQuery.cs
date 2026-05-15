using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetPaymentDetail;

public record GetPaymentDetailQuery(Guid ProcurementId) : IRequest<Result<PaymentDetailDto>>;

public record PaymentDetailDto(
    Guid Id,
    Guid ProcurementId,
    Guid InvoiceId,
    Guid ApprovedByUserId,
    string ApprovedByName,
    DateTime ApprovedAt,
    Guid? PaidByUserId,
    string? PaidByName,
    DateTime? PaidAt,
    string? PaymentReference,
    string? PaymentProofFilePath);
