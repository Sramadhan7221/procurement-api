namespace Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;

public class CreateProcurementItemDto
{
    public string ItemName { get; set; } = string.Empty;
    public string UoM { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
