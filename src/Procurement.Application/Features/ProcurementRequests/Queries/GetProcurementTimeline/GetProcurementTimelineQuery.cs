using MediatR;
using Procurement.Application.Common;
using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementTimeline;

public record GetProcurementTimelineQuery(Guid ProcurementId) : IRequest<Result<List<AuditTrailDto>>>;

public record AuditTrailDto(
    DateTime OccurredAt,
    string ActorName,
    string ActorRole,
    string Action,
    string? FromStatus,
    string ToStatus,
    string? Comment,
    object? Metadata);
