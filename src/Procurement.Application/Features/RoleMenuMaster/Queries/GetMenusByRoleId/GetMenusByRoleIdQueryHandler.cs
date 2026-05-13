using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.RoleMenuMaster.Queries.GetMenusByRoleId;

public class GetMenusByRoleIdQueryHandler
    : IRequestHandler<GetMenusByRoleIdQuery, Result<List<RoleMenuViewModel>>>
{
    private readonly IApplicationDbContext _context;

    public GetMenusByRoleIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<RoleMenuViewModel>>> Handle(
        GetMenusByRoleIdQuery request,
        CancellationToken cancellationToken)
    {
        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

        if (!roleExists)
            throw new NotFoundException("Role", request.RoleId);

        var menus = await _context.RoleMenus
            .AsNoTracking()
            .Where(rm => rm.RoleId == request.RoleId)
            .Select(rm => new RoleMenuViewModel
            {
                MenuId       = rm.Menu.Id,
                Name         = rm.Menu.Name,
                Url          = rm.Menu.Url,
                Icon         = rm.Menu.Icon,
                Sequence     = rm.Menu.Sequence,
                ParentMenuId = rm.Menu.ParentMenuId
            })
            .OrderBy(m => m.Sequence)
            .ToListAsync(cancellationToken);

        return Result<List<RoleMenuViewModel>>.Success(menus, "Menus retrieved successfully.");
    }
}
