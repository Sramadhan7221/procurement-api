using Procurement.Domain.Enums;

namespace Procurement.Domain.Entities;

public class ProcurementInvoice : BaseEntity
{
    public Guid ProcurementId { get; set; }
    public string VendorInvoiceNumber { get; set; } = string.Empty;
    public DateOnly VendorInvoiceDate { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public Guid UploadedByUserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public InvoiceMatchingStatus MatchingStatus { get; set; } = InvoiceMatchingStatus.Pending;
    public string? DisputeNote { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public DateTime? VerifiedAt { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
    public User UploadedBy { get; set; } = null!;
    public User? VerifiedBy { get; set; }
    public ICollection<ProcurementInvoiceItem> Items { get; set; } = new List<ProcurementInvoiceItem>();
}
