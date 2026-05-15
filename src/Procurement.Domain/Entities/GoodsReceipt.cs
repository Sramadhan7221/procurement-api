namespace Procurement.Domain.Entities;

public class GoodsReceipt : BaseEntity
{
    public Guid ProcurementId { get; set; }
    public Guid ReceivedByUserId { get; set; }
    public DateTime ReceivedAt { get; set; }
    public string? DeliveryOrderFile { get; set; }
    public string? Notes { get; set; }

    public ProcurementRequest ProcurementRequest { get; set; } = null!;
    public User ReceivedBy { get; set; } = null!;
    public ICollection<GoodsReceiptItem> Items { get; set; } = new List<GoodsReceiptItem>();
}
