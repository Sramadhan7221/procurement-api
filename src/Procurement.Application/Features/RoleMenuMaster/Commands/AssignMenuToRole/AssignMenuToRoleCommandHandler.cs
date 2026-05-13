using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMenuMaster.Commands.AssignMenuToRole;

public class AssignMenuToRoleCommandHandler
    : IRequestHandler<AssignMenuToRoleCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public AssignMenuToRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        AssignMenuToRoleCommand request,
        CancellationToken cancellationToken)
    {
        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

        if (!roleExists)
            throw new NotFoundException("Role", request.RoleId);

        var menuExists = await _context.Menus
            .AnyAsync(m => m.Id == request.MenuId, cancellationToken);

        if (!menuExists)
            throw new NotFoundException("Menu", request.MenuId);

        // Restore if previously soft-deleted, otherwise create new
        var existing = await _context.RoleMenus
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                rm => rm.RoleId == request.RoleId && rm.MenuId == request.MenuId,
                cancellationToken);

        if (existing is not null)
        {
            if (!existing.IsDeleted)
                return Result<Guid>.Failure("This menu is already assigned to the role.");

            existing.IsDeleted  = false;
            existing.DeletedAt  = null;
            existing.UpdatedAt  = DateTime.UtcNow;
            _context.RoleMenus.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(existing.Id, "Menu assigned to role successfully.");
        }

        var roleMenu = new RoleMenu
        {
            RoleId = request.RoleId,
            MenuId = request.MenuId
        };

        _context.RoleMenus.Add(roleMenu);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(roleMenu.Id, "Menu assigned to role successfully.");
    }
}
