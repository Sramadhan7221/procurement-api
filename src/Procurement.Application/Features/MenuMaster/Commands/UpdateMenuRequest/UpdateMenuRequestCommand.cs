using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.MenuMaster.Commands.UpdateMenuRequest;

public record UpdateMenuRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int Sequence { get; set; }
    public Guid? ParentMenuId { get; set; }
}
