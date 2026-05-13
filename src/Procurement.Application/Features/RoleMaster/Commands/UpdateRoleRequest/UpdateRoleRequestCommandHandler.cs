using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMaster.Commands.UpdateRoleRequest;

public class UpdateRoleRequestCommandHandler
    : IRequestHandler<UpdateRoleRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateRoleRequestCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (role is null)
            throw new NotFoundException($"Role with ID {request.Id} not found.", nameof(role));

        role.Name = request.Name;

        _context.Roles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(role.Id, "Role updated successfully.");
    }
}
