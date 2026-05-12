using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApproveProcurementRequest;

public class ApproveProcurementRequestCommandHandler
    : IRequestHandler<ApproveProcurementRequestCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public ApproveProcurementRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(
        ApproveProcurementRequestCommand request,
        CancellationToken cancellationToken)
    {
        var procurementRequest = await _context.ProcurementRequests
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (procurementRequest is null)
            throw new NotFoundException(nameof(procurementRequest), request.Id);

        procurementRequest.Approve();

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, "Procurement request approved successfully.");
    }
}
