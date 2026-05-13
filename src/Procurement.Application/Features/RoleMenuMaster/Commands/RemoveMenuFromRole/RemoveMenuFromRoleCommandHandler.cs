using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMenuMaster.Commands.RemoveMenuFromRole;

public class RemoveMenuFromRoleCommandHandler
    : IRequestHandler<RemoveMenuFromRoleCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public RemoveMenuFromRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        RemoveMenuFromRoleCommand request,
        CancellationToken cancellationToken)
    {
        var roleMenu = await _context.RoleMenus
            .FirstOrDefaultAsync(
                rm => rm.RoleId == request.RoleId && rm.MenuId == request.MenuId,
                cancellationToken);

        if (roleMenu is null)
            throw new NotFoundException("RoleMenu assignment", $"RoleId={request.RoleId}, MenuId={request.MenuId}");

        roleMenu.IsDeleted = true;
        roleMenu.DeletedAt = DateTime.UtcNow;

        _context.RoleMenus.Update(roleMenu);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(roleMenu.Id, "Menu removed from role successfully.");
    }
}
