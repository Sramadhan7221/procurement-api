using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.AdminReviewProcurementRequest;

public record AdminReviewProcurementRequestCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public Guid AdminUserId { get; init; }
    public bool IsApproved { get; init; }
    public string? Comment { get; init; }
}
