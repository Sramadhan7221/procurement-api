using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestDatatable;

public class ProcurementRequestListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public ProcurementStatus Status { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string RequestDate { get; set; } = "-";
}
