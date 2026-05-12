namespace Procurement.Domain.Entities;

public class ProcurementItem : BaseEntity
{
    public Guid ProcurementRequestId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal SubTotal => Quantity * UnitPrice;

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
}
