using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ManagerReviewProcurementRequest;

public record ManagerReviewProcurementRequestCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public Guid ManagerUserId { get; init; }
    public bool IsApproved { get; init; }
    public string? Comment { get; init; }
}
