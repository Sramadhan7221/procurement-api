using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.DivisionMaster.Queries.GetDivisionById;

public class GetDivisionByIdQueryHandler
    : IRequestHandler<GetDivisionByIdQuery, Result<DivisionDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDivisionByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<DivisionDetailDto>> IRequestHandler<GetDivisionByIdQuery, Result<DivisionDetailDto>>.Handle(
        GetDivisionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var Division = await _context.Divisions
            .FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);

        if (Division is null)
            throw new NotFoundException(nameof(Division), request.Id);

        var DivisionDetail = new DivisionDetailDto
        {
            Name = Division.Name
        };

        return Result<DivisionDetailDto>.Success(DivisionDetail, "Division retrieved successfully.");
    }
}
