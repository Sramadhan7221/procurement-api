using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.DivisionMaster.Commands.UpdateDivisionRequest;

public class UpdateDivisionRequestCommandHandler
    : IRequestHandler<UpdateDivisionRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateDivisionRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateDivisionRequestCommand request,
        CancellationToken cancellationToken)
    {
        var Division = await _context.Divisions
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if(Division == null)
        {
            throw new NotFoundException($"Division with ID {request.Id} not found.", nameof(Division));
        }
        
        Division.Name = request.Name;

        _context.Divisions.Update(Division);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(Division.Id, "Division updated successfully.");
    }
}