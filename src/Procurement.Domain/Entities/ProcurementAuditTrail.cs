using Procurement.Domain.Enums;

namespace Procurement.Domain.Entities;

public class ProcurementAuditTrail
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProcurementId { get; set; }
    public Guid ActorUserId { get; set; }
    public string ActorName { get; set; } = string.Empty;
    public string ActorRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public ProcurementStatus? FromStatus { get; set; }
    public ProcurementStatus ToStatus { get; set; }
    public string? Comment { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string? Metadata { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
}
