using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetPaymentDetail;

public class GetPaymentDetailQueryHandler : IRequestHandler<GetPaymentDetailQuery, Result<PaymentDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPaymentDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaymentDetailDto>> Handle(GetPaymentDetailQuery request, CancellationToken cancellationToken)
    {
        var payment = await _context.ProcurementPayments
            .Include(p => p.ApprovedBy)
            .Include(p => p.PaidBy)
            .FirstOrDefaultAsync(p => p.ProcurementId == request.ProcurementId, cancellationToken);

        if (payment is null)
            throw new NotFoundException("ProcurementPayment", $"procurement {request.ProcurementId}");

        var dto = new PaymentDetailDto(
            payment.Id,
            payment.ProcurementId,
            payment.InvoiceId,
            payment.ApprovedByUserId,
            payment.ApprovedBy.Name,
            payment.ApprovedAt,
            payment.PaidByUserId,
            payment.PaidBy?.Name,
            payment.PaidAt,
            payment.PaymentReference,
            payment.PaymentProofFilePath);

        return Result<PaymentDetailDto>.Success(dto);
    }
}
