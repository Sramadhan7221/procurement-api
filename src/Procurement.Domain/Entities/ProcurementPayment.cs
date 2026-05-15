namespace Procurement.Domain.Entities;

public class ProcurementPayment : BaseEntity
{
    public Guid ProcurementId { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid ApprovedByUserId { get; set; }
    public DateTime ApprovedAt { get; set; }
    public Guid? PaidByUserId { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    public string? PaymentProofFilePath { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
    public ProcurementInvoice Invoice { get; set; } = null!;
    public User ApprovedBy { get; set; } = null!;
    public User? PaidBy { get; set; }
}
