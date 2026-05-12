using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.DivisionMaster.Commands;

public class CreateDivisionRequestCommandHandler
    : IRequestHandler<CreateDivisionRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateDivisionRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateDivisionRequestCommand request,
        CancellationToken cancellationToken)
    {
        var Division = new Domain.Entities.Division
        {
            Name = request.Name
        };

        _context.Divisions.Add(Division);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(Division.Id, "Division created successfully.");
    }
}