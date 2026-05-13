using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.MenuMaster.Commands.UpdateMenuRequest;

public class UpdateMenuRequestCommandHandler
    : IRequestHandler<UpdateMenuRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateMenuRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        UpdateMenuRequestCommand request,
        CancellationToken cancellationToken)
    {
        var menu = await _context.Menus
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (menu is null)
            throw new NotFoundException($"Menu with ID {request.Id} not found.", nameof(menu));

        menu.Name         = request.Name;
        menu.Description  = request.Description;
        menu.Url          = request.Url;
        menu.Icon         = request.Icon;
        menu.Sequence     = request.Sequence;
        menu.ParentMenuId = request.ParentMenuId;
        menu.UpdatedAt    = DateTime.UtcNow;

        _context.Menus.Update(menu);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(menu.Id, "Menu updated successfully.");
    }
}
