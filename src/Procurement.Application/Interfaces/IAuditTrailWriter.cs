using Procurement.Domain.Enums;

namespace Procurement.Application.Interfaces;

public interface IAuditTrailWriter
{
    Task WriteAsync(
        Guid procurementId,
        Guid actorUserId,
        string actorName,
        string actorRole,
        string action,
        ProcurementStatus? fromStatus,
        ProcurementStatus toStatus,
        string? comment = null,
        object? metadata = null,
        CancellationToken cancellationToken = default);
}
