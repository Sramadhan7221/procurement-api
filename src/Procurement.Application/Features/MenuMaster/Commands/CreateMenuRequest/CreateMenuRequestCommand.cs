using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.MenuMaster.Commands.CreateMenuRequest;

public record CreateMenuRequestCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Url { get; init; }
    public string? Icon { get; init; }
    public int Sequence { get; init; }
    public Guid? ParentMenuId { get; init; }
}
