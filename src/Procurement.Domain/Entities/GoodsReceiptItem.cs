namespace Procurement.Domain.Entities;

public class GoodsReceiptItem : BaseEntity
{
    public Guid GoodsReceiptId { get; set; }
    public Guid ProcurementItemId { get; set; }
    public decimal OrderedQuantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public bool IsPartialReceipt { get; set; }

    public GoodsReceipt GoodsReceipt { get; set; } = null!;
    public ProcurementItem ProcurementItem { get; set; } = null!;
}
