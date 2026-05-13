using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuById;

public class GetMenuByIdQueryHandler
    : IRequestHandler<GetMenuByIdQuery, Result<MenuDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMenuByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MenuDetailDto>> Handle(
        GetMenuByIdQuery request,
        CancellationToken cancellationToken)
    {
        var menu = await _context.Menus
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (menu is null)
            throw new NotFoundException(nameof(menu), request.Id);

        var dto = new MenuDetailDto
        {
            Id           = menu.Id,
            Name         = menu.Name,
            Description  = menu.Description,
            Url          = menu.Url,
            Icon         = menu.Icon,
            Sequence     = menu.Sequence,
            ParentMenuId = menu.ParentMenuId
        };

        return Result<MenuDetailDto>.Success(dto, "Menu retrieved successfully.");
    }
}
