using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Domain.Entities;

public class ProcurementRequest : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public ProcurementStatus Status { get; set; } = ProcurementStatus.Draft;
    public Guid CreatedByUserId { get; set; }

    public User CreatedBy { get; set; } = null!;
    public ICollection<ProcurementItem> Items { get; set; } = new List<ProcurementItem>();

    public void Submit()
    {
        if (Status != ProcurementStatus.Draft)
            throw new DomainException($"Only Draft requests can be submitted. Current status: {Status}.");
        Status = ProcurementStatus.Pending;
    }

    public void Approve()
    {
        if (Status != ProcurementStatus.Pending)
            throw new DomainException($"Only Pending requests can be approved. Current status: {Status}.");
        Status = ProcurementStatus.Approved;
    }

    public void Reject()
    {
        if (Status != ProcurementStatus.Pending)
            throw new DomainException($"Only Pending requests can be rejected. Current status: {Status}.");
        Status = ProcurementStatus.Rejected;
    }
}
