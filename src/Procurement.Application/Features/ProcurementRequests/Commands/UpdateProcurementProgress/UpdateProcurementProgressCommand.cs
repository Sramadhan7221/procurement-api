using MediatR;
using Procurement.Application.Common;
using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Commands.UpdateProcurementProgress;

public record UpdateProcurementProgressCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public Guid AdminUserId { get; init; }
    public ProcurementStatus NewStatus { get; init; }
}
