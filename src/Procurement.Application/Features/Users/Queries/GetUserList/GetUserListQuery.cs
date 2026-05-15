using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.Users.Queries.GetUserList;

public record GetUserListQuery() : IRequest<Result<List<UserDto>>>;
