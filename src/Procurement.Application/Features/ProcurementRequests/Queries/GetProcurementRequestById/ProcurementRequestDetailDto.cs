namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

public class ProcurementRequestDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<ProcurementItemDto> Items { get; set; } = new();
}
