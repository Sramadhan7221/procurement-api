using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMaster.Queries.GetRoleById;

public class GetRoleByIdQueryHandler
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoleByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<RoleDetailDto>> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(role), request.Id);

        var dto = new RoleDetailDto
        {
            Id   = role.Id,
            Name = role.Name
        };

        return Result<RoleDetailDto>.Success(dto, "Role retrieved successfully.");
    }
}
