using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;

public record CreateProcurementRequestCommand : IRequest<Result<Guid>>
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid CreatedByUserId { get; init; }
    public List<CreateProcurementItemDto> Items { get; init; } = new();
}
