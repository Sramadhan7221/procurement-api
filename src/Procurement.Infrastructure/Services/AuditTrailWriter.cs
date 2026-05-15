using System.Text.Json;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;

namespace Procurement.Infrastructure.Services;

public class AuditTrailWriter : IAuditTrailWriter
{
    private readonly IApplicationDbContext _context;

    public AuditTrailWriter(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task WriteAsync(
        Guid procurementId,
        Guid actorUserId,
        string actorName,
        string actorRole,
        string action,
        ProcurementStatus? fromStatus,
        ProcurementStatus toStatus,
        string? comment = null,
        object? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var entry = new ProcurementAuditTrail
        {
            ProcurementId = procurementId,
            ActorUserId = actorUserId,
            ActorName = actorName,
            ActorRole = actorRole,
            Action = action,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            Comment = comment,
            OccurredAt = DateTime.UtcNow,
            Metadata = metadata is not null ? JsonSerializer.Serialize(metadata) : null,
        };

        _context.ProcurementAuditTrails.Add(entry);
        return Task.CompletedTask;
    }
}
