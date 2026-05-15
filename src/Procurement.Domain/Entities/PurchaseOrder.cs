namespace Procurement.Domain.Entities;

public class PurchaseOrder : BaseEntity
{
    public string PoNumber { get; set; } = string.Empty;
    public Guid ProcurementId { get; set; }
    public Guid GeneratedByUserId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string? FilePath { get; set; }
    public bool SentToVendor { get; set; }
    public DateTime? SentAt { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
    public User GeneratedBy { get; set; } = null!;
}
