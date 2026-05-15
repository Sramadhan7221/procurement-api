using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.Users.Queries.GetUserList;

public class GetUserListQueryHandler
    : IRequestHandler<GetUserListQuery, Result<List<UserDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUserListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    async Task<Result<List<UserDto>>> IRequestHandler<GetUserListQuery, Result<List<UserDto>>>.Handle(
        GetUserListQuery request,
        CancellationToken cancellationToken)
    {
        var UserList = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.IsDeleted == false)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                RoleName = u.Role.Name,
                Role = u.RoleId
            })
            .ToListAsync(cancellationToken);

        return Result<List<UserDto>>.Success(UserList, "Users retrieved successfully.");
    }
}