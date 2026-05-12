using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.DivisionMaster.Commands;

public record CreateDivisionRequestCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
}