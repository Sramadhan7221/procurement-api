using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ManagerReviewProcurementRequest;

public class ManagerReviewProcurementRequestCommandHandler
    : IRequestHandler<ManagerReviewProcurementRequestCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public ManagerReviewProcurementRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        ManagerReviewProcurementRequestCommand request,
        CancellationToken cancellationToken)
    {
        var manager = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.ManagerUserId, cancellationToken);

        if (manager is null)
            throw new NotFoundException(nameof(manager), request.ManagerUserId);

        if (manager.Role.Name != "Manager")
            throw new DomainException("Only Manager users can review procurement requests at this stage.");

        var procurementRequest = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (procurementRequest is null)
            throw new NotFoundException(nameof(procurementRequest), request.Id);

        if (request.IsApproved)
        {
            procurementRequest.ManagerApprove(request.ManagerUserId, request.Comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true, "Procurement request approved by Manager.");
        }
        else
        {
            procurementRequest.ManagerReject(request.ManagerUserId, request.Comment);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true, "Procurement request rejected by Manager.");
        }
    }
}
