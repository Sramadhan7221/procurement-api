using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.DivisionMaster.Commands.DeleteDivision;

public class DeleteDivisionCommandHandler
    : IRequestHandler<DeleteDivisionCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteDivisionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        DeleteDivisionCommand request,
        CancellationToken cancellationToken)
    {
        var Division = await _context.Divisions
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (Division is null)
            throw new NotFoundException(nameof(Division), request.Id);

        Division.IsDeleted = true;
        Division.DeletedAt = DateTime.UtcNow;

        _context.Divisions.Update(Division);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(Division.Id, "Division deleted successfully.");
    }
}
