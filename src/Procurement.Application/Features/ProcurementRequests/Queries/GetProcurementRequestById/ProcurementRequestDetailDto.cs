using Procurement.Domain.Enums;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

public class ProcurementRequestDetailDto
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

    public string? ManagerComment { get; set; }
    public Guid? ManagerReviewedByUserId { get; set; }
    public string? ManagerReviewedByName { get; set; }

    public string? AdminComment { get; set; }
    public Guid? AdminReviewedByUserId { get; set; }
    public string? AdminReviewedByName { get; set; }

    public List<ProcurementItemDto> Items { get; set; } = new();
}
