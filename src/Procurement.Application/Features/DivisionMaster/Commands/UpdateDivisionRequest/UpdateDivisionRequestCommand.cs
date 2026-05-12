using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.DivisionMaster.Commands.UpdateDivisionRequest;

public record UpdateDivisionRequestCommand : IRequest<Result<Guid>>
{
    public Guid Id { get;set; }
    public string Name { get; set; } = string.Empty;
}