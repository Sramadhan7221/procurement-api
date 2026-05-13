using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuById;

public record GetMenuByIdQuery(Guid Id) : IRequest<Result<MenuDetailDto>>;
