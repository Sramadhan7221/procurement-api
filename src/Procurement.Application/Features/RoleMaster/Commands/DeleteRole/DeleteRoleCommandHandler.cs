using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMaster.Commands.DeleteRole;

public class DeleteRoleCommandHandler
    : IRequestHandler<DeleteRoleCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            throw new NotFoundException(nameof(role), request.Id);

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(role.Id, "Role deleted successfully.");
    }
}
