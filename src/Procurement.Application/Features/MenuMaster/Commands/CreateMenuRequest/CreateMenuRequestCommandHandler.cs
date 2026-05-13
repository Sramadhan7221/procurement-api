using MediatR;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.MenuMaster.Commands.CreateMenuRequest;

public class CreateMenuRequestCommandHandler
    : IRequestHandler<CreateMenuRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateMenuRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateMenuRequestCommand request,
        CancellationToken cancellationToken)
    {
        var menu = new Menu
        {
            Name         = request.Name,
            Description  = request.Description,
            Url          = request.Url,
            Icon         = request.Icon,
            Sequence     = request.Sequence,
            ParentMenuId = request.ParentMenuId
        };

        _context.Menus.Add(menu);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(menu.Id, "Menu created successfully.");
    }
}
