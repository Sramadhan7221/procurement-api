using MediatR;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.RoleMaster.Commands.CreateRoleRequest;

public class CreateRoleRequestCommandHandler
    : IRequestHandler<CreateRoleRequestCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleRequestCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
        CreateRoleRequestCommand request,
        CancellationToken cancellationToken)
    {
        var role = new Domain.Entities.Role
        {
            Name = request.Name
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(role.Id, "Role created successfully.");
    }
}
