using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.MarkAsPaid;

public class MarkAsPaidCommandHandler : IRequestHandler<MarkAsPaidCommand, Result<MarkAsPaidResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;
    private readonly IFileService _fileService;

    public MarkAsPaidCommandHandler(
        IApplicationDbContext context,
        IAuditTrailWriter auditTrail,
        IFileService fileService)
    {
        _context = context;
        _auditTrail = auditTrail;
        _fileService = fileService;
    }

    public async Task<Result<MarkAsPaidResult>> Handle(MarkAsPaidCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.AdminUserId, cancellationToken);

        if (admin is null)
            throw new NotFoundException(nameof(User), request.AdminUserId);

        if (admin.Role.Name != "Admin")
            throw new DomainException("Only Admin users can mark payments as paid.");

        var payment = await _context.ProcurementPayments
            .FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);

        if (payment is null)
            throw new NotFoundException(nameof(ProcurementPayment), request.PaymentId);

        var procurement = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == payment.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), payment.ProcurementId);

        var fromStatus = procurement.Status;
        procurement.CompleteProcurement();

        var paidAt = DateTime.UtcNow;
        payment.PaidByUserId = request.AdminUserId;
        payment.PaidAt = paidAt;
        payment.PaymentReference = request.PaymentReference;

        if (request.PaymentProofFile is not null)
        {
            var folder = $"procurement/{payment.ProcurementId}/payment-proof";
            payment.PaymentProofFilePath = await _fileService.UploadFileAsync(request.PaymentProofFile, folder);
        }

        await _auditTrail.WriteAsync(
            payment.ProcurementId,
            request.AdminUserId,
            admin.Name,
            admin.Role.Name,
            "Payment Completed",
            fromStatus,
            ProcurementStatus.Completed,
            metadata: new { paymentReference = request.PaymentReference },
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<MarkAsPaidResult>.Success(
            new MarkAsPaidResult(paidAt, request.PaymentReference),
            "Payment marked as paid and procurement completed.");
    }
}
