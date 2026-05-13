using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.MenuMaster.Commands.DeleteMenu;

public class DeleteMenuCommandHandler
    : IRequestHandler<DeleteMenuCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeleteMenuCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        DeleteMenuCommand request,
        CancellationToken cancellationToken)
    {
        var menu = await _context.Menus
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (menu is null)
            throw new NotFoundException(nameof(menu), request.Id);

        menu.IsDeleted = true;
        menu.DeletedAt = DateTime.UtcNow;

        _context.Menus.Update(menu);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(menu.Id, "Menu deleted successfully.");
    }
}
