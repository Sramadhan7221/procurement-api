namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

public class ProcurementItemDto
{
    public Guid Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
