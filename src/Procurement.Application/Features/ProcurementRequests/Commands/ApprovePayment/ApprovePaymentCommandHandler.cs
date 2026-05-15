using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApprovePayment;

public class ApprovePaymentCommandHandler : IRequestHandler<ApprovePaymentCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;

    public ApprovePaymentCommandHandler(IApplicationDbContext context, IAuditTrailWriter auditTrail)
    {
        _context = context;
        _auditTrail = auditTrail;
    }

    public async Task<Result<Guid>> Handle(ApprovePaymentCommand request, CancellationToken cancellationToken)
    {
        var manager = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.ManagerUserId, cancellationToken);

        if (manager is null)
            throw new NotFoundException(nameof(User), request.ManagerUserId);

        if (manager.Role.Name != "Manager")
            throw new DomainException("Only Manager users can approve payments.");

        var procurement = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementId);

        if (procurement.Status != ProcurementStatus.InvoiceVerified)
            throw new DomainException($"Payment can only be approved for 'Invoice Verified' requests. Current status: {procurement.Status}.");

        var invoice = await _context.ProcurementInvoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId && i.ProcurementId == request.ProcurementId, cancellationToken);

        if (invoice is null)
            throw new NotFoundException(nameof(ProcurementInvoice), request.InvoiceId);

        var fromStatus = procurement.Status;
        procurement.ApprovePayment();

        var payment = new ProcurementPayment
        {
            ProcurementId = request.ProcurementId,
            InvoiceId = request.InvoiceId,
            ApprovedByUserId = request.ManagerUserId,
            ApprovedAt = DateTime.UtcNow,
        };

        _context.ProcurementPayments.Add(payment);

        await _auditTrail.WriteAsync(
            request.ProcurementId,
            request.ManagerUserId,
            manager.Name,
            manager.Role.Name,
            "Payment Approved",
            fromStatus,
            ProcurementStatus.PaymentProcessed,
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(payment.Id, "Payment approved successfully.");
    }
}
