using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UpdateProcurementProgress;

public class UpdateProcurementProgressCommandHandler
    : IRequestHandler<UpdateProcurementProgressCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateProcurementProgressCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        UpdateProcurementProgressCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == request.AdminUserId, cancellationToken);

        if (admin is null)
            throw new NotFoundException(nameof(admin), request.AdminUserId);

        if (admin.Role.Name != "Admin")
            throw new DomainException("Only Admin users can update procurement progress.");

        var procurementRequest = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (procurementRequest is null)
            throw new NotFoundException(nameof(procurementRequest), request.Id);

        switch (request.NewStatus)
        {
            case ProcurementStatus.InOrderByAdmin:
                procurementRequest.SetInOrder();
                break;
            case ProcurementStatus.OrderReceived:
                procurementRequest.SetOrderReceived();
                break;
            case ProcurementStatus.Completed:
                procurementRequest.SetCompleted();
                break;
            default:
                throw new DomainException($"Status '{request.NewStatus}' is not a valid progress update.");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, $"Procurement request progress updated to '{request.NewStatus}'.");
    }
}
