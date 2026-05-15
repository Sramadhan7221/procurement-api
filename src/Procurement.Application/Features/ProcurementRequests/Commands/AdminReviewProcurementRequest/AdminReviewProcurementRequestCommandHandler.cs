using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.AdminReviewProcurementRequest;

public class AdminReviewProcurementRequestCommandHandler
    : IRequestHandler<AdminReviewProcurementRequestCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public AdminReviewProcurementRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        AdminReviewProcurementRequestCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.AdminUserId, cancellationToken);

        if (admin is null)
            throw new NotFoundException(nameof(admin), request.AdminUserId);

        if (admin.Role.Name != "Admin")
            throw new DomainException("Only Admin users can review Manager-approved procurement requests.");

        var procurementRequest = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (procurementRequest is null)
            throw new NotFoundException(nameof(procurementRequest), request.Id);

        if (request.IsApproved)
        {
            procurementRequest.AdminApprove(request.AdminUserId, request.Comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true, "Procurement request approved by Admin.");
        }
        else
        {
            procurementRequest.AdminReject(request.AdminUserId, request.Comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true, "Procurement request rejected by Admin.");
        }
    }
}
