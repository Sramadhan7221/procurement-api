namespace Procurement.Domain.Entities;

public class ProcurementInvoiceItem : BaseEntity
{
    public Guid InvoiceId { get; set; }
    public Guid ProcurementItemId { get; set; }
    public decimal InvoicedQuantity { get; set; }
    public decimal InvoiceUnitPrice { get; set; }

    public ProcurementInvoice Invoice { get; set; } = null!;
    public ProcurementItem ProcurementItem { get; set; } = null!;
}
