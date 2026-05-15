namespace Procurement.Domain.Entities;

public class Invoice : BaseEntity
{
    public Guid ProcurementRequestId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
}
