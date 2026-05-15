using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.VerifyInvoice;

public class VerifyInvoiceCommandHandler : IRequestHandler<VerifyInvoiceCommand, Result<VerifyInvoiceResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;

    public VerifyInvoiceCommandHandler(IApplicationDbContext context, IAuditTrailWriter auditTrail)
    {
        _context = context;
        _auditTrail = auditTrail;
    }

    public async Task<Result<VerifyInvoiceResult>> Handle(VerifyInvoiceCommand request, CancellationToken cancellationToken)
    {
        var manager = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.ManagerUserId, cancellationToken);

        if (manager is null)
            throw new NotFoundException(nameof(User), request.ManagerUserId);

        if (manager.Role.Name != "Manager")
            throw new DomainException("Only Manager users can verify invoices.");

        var invoice = await _context.ProcurementInvoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice is null)
            throw new NotFoundException(nameof(ProcurementInvoice), request.InvoiceId);

        var procurement = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == invoice.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), invoice.ProcurementId);

        var fromStatus = procurement.Status;
        procurement.VerifyInvoice();

        var verifiedAt = DateTime.UtcNow;
        invoice.VerifiedByUserId = request.ManagerUserId;
        invoice.VerifiedAt = verifiedAt;

        await _auditTrail.WriteAsync(
            invoice.ProcurementId,
            request.ManagerUserId,
            manager.Name,
            manager.Role.Name,
            "Invoice Verified",
            fromStatus,
            ProcurementStatus.InvoiceVerified,
            cancellationToken: cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<VerifyInvoiceResult>.Success(
            new VerifyInvoiceResult(invoice.Id, verifiedAt),
            "Invoice verified successfully.");
    }
}
