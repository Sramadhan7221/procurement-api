using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Exceptions;
using System.Text.Json;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementTimeline;

public class GetProcurementTimelineQueryHandler
    : IRequestHandler<GetProcurementTimelineQuery, Result<List<AuditTrailDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProcurementTimelineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<AuditTrailDto>>> Handle(
        GetProcurementTimelineQuery request,
        CancellationToken cancellationToken)
    {
        var exists = await _context.ProcurementRequests
            .AnyAsync(r => r.Id == request.ProcurementId, cancellationToken);

        if (!exists)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementId);

        var raw = await _context.ProcurementAuditTrails
            .Where(a => a.ProcurementId == request.ProcurementId)
            .OrderBy(a => a.OccurredAt)
            .ToListAsync(cancellationToken);

        var entries = raw.Select(a => new AuditTrailDto(
                a.OccurredAt,
                a.ActorName,
                a.ActorRole,
                a.Action,
                a.FromStatus.HasValue ? a.FromStatus.Value.ToString() : null,
                a.ToStatus.ToString(),
                a.Comment,
                a.Metadata is not null ? JsonSerializer.Deserialize<object>(a.Metadata) : null))
            .ToList();

        return Result<List<AuditTrailDto>>.Success(entries);
    }
}
