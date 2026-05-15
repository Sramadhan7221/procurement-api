using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ResolveInvoiceDispute;

public class ResolveInvoiceDisputeCommandHandler
    : IRequestHandler<ResolveInvoiceDisputeCommand, Result<InvoiceMatchingStatus>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuditTrailWriter _auditTrail;

    public ResolveInvoiceDisputeCommandHandler(IApplicationDbContext context, IAuditTrailWriter auditTrail)
    {
        _context = context;
        _auditTrail = auditTrail;
    }

    public async Task<Result<InvoiceMatchingStatus>> Handle(
        ResolveInvoiceDisputeCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.AdminUserId, cancellationToken);

        if (admin is null)
            throw new NotFoundException(nameof(User), request.AdminUserId);

        if (admin.Role.Name != "Admin")
            throw new DomainException("Only Admin users can resolve invoice disputes.");

        var invoice = await _context.ProcurementInvoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice is null)
            throw new NotFoundException(nameof(ProcurementInvoice), request.InvoiceId);

        var procurement = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == invoice.ProcurementId, cancellationToken);

        if (procurement is null)
            throw new NotFoundException(nameof(ProcurementRequest), invoice.ProcurementId);

        invoice.DisputeNote = request.Note;

        if (request.Resolution == DisputeResolution.Accept)
        {
            invoice.MatchingStatus = InvoiceMatchingStatus.Matched;
            procurement.ResolveInvoiceDispute();

            await _auditTrail.WriteAsync(
                invoice.ProcurementId,
                request.AdminUserId,
                admin.Name,
                admin.Role.Name,
                "Invoice Dispute Resolved",
                ProcurementStatus.InvoiceDisputed,
                ProcurementStatus.InvoiceUploaded,
                comment: request.Note,
                cancellationToken: cancellationToken);
        }
        else
        {
            await _auditTrail.WriteAsync(
                invoice.ProcurementId,
                request.AdminUserId,
                admin.Name,
                admin.Role.Name,
                "Invoice Dispute Rejected",
                ProcurementStatus.InvoiceDisputed,
                ProcurementStatus.InvoiceDisputed,
                comment: request.Note,
                cancellationToken: cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<InvoiceMatchingStatus>.Success(
            invoice.MatchingStatus,
            $"Invoice dispute {(request.Resolution == DisputeResolution.Accept ? "accepted" : "rejected")}.");
    }
}
