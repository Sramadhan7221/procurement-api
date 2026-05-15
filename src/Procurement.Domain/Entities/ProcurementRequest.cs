using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Domain.Entities;

public class ProcurementRequest : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public ProcurementStatus Status { get; set; } = ProcurementStatus.RequestCreated;
    public Guid CreatedByUserId { get; set; }

    public string? ManagerComment { get; set; }
    public Guid? ManagerReviewedByUserId { get; set; }

    public string? AdminComment { get; set; }
    public Guid? AdminReviewedByUserId { get; set; }

    public User CreatedBy { get; set; } = null!;
    public User? ManagerReviewedBy { get; set; }
    public User? AdminReviewedBy { get; set; }
    public ICollection<ProcurementItem> Items { get; set; } = new List<ProcurementItem>();
    public Invoice? Invoice { get; set; }

    public void ManagerApprove(Guid managerId, string? comment = null)
    {
        if (Status != ProcurementStatus.RequestCreated)
            throw new DomainException($"Only requests with status 'Request Created' can be reviewed by Manager. Current status: {Status}.");
        ManagerReviewedByUserId = managerId;
        ManagerComment = comment;
        Status = ProcurementStatus.ApproveByManager;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ManagerReject(Guid managerId, string? comment = null)
    {
        if (Status != ProcurementStatus.RequestCreated)
            throw new DomainException($"Only requests with status 'Request Created' can be reviewed by Manager. Current status: {Status}.");
        ManagerReviewedByUserId = managerId;
        ManagerComment = comment;
        Status = ProcurementStatus.RejectByManager;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdminApprove(Guid adminId, string? comment = null)
    {
        if (Status != ProcurementStatus.ApproveByManager)
            throw new DomainException($"Only requests approved by Manager can be reviewed by Admin. Current status: {Status}.");
        AdminReviewedByUserId = adminId;
        AdminComment = comment;
        Status = ProcurementStatus.ApproveByAdmin;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AdminReject(Guid adminId, string? comment = null)
    {
        if (Status != ProcurementStatus.ApproveByManager)
            throw new DomainException($"Only requests approved by Manager can be reviewed by Admin. Current status: {Status}.");
        AdminReviewedByUserId = adminId;
        AdminComment = comment;
        Status = ProcurementStatus.RejectByAdmin;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetInOrder()
    {
        if (Status != ProcurementStatus.ApproveByAdmin)
            throw new DomainException($"Only Admin-approved requests can be set to 'In Order'. Current status: {Status}.");
        Status = ProcurementStatus.InOrderByAdmin;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOrderReceived()
    {
        if (Status != ProcurementStatus.InOrderByAdmin)
            throw new DomainException($"Only 'In Order By Admin' requests can be marked as 'Order Received'. Current status: {Status}.");
        Status = ProcurementStatus.OrderReceived;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCompleted()
    {
        if (Status != ProcurementStatus.OrderReceived)
            throw new DomainException($"Only 'Order Received' requests can be marked as 'Completed'. Current status: {Status}.");
        Status = ProcurementStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PlaceOrder()
    {
        if (Status != ProcurementStatus.ApproveByAdmin)
            throw new DomainException($"Only Admin-approved requests can have a Purchase Order placed. Current status: {Status}.");
        Status = ProcurementStatus.InOrderByAdmin;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ConfirmGoodsReceipt()
    {
        if (Status != ProcurementStatus.InOrderByAdmin)
            throw new DomainException($"Only 'In Order By Admin' requests can have goods receipt confirmed. Current status: {Status}.");
        Status = ProcurementStatus.OrderReceived;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UploadInvoice()
    {
        if (Status != ProcurementStatus.OrderReceived)
            throw new DomainException($"Only 'Order Received' requests can have an invoice uploaded. Current status: {Status}.");
        Status = ProcurementStatus.InvoiceUploaded;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DisputeInvoice()
    {
        if (Status != ProcurementStatus.InvoiceUploaded)
            throw new DomainException($"Only 'Invoice Uploaded' requests can be set to disputed. Current status: {Status}.");
        Status = ProcurementStatus.InvoiceDisputed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ResolveInvoiceDispute()
    {
        if (Status != ProcurementStatus.InvoiceDisputed)
            throw new DomainException($"Only 'Invoice Disputed' requests can have their dispute resolved. Current status: {Status}.");
        Status = ProcurementStatus.InvoiceUploaded;
        UpdatedAt = DateTime.UtcNow;
    }

    public void VerifyInvoice()
    {
        if (Status != ProcurementStatus.InvoiceUploaded)
            throw new DomainException($"Only 'Invoice Uploaded' requests can have the invoice verified. Current status: {Status}.");
        Status = ProcurementStatus.InvoiceVerified;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApprovePayment()
    {
        if (Status != ProcurementStatus.InvoiceVerified)
            throw new DomainException($"Only 'Invoice Verified' requests can have payment approved. Current status: {Status}.");
        Status = ProcurementStatus.PaymentProcessed;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CompleteProcurement()
    {
        if (Status != ProcurementStatus.PaymentProcessed)
            throw new DomainException($"Only 'Payment Processed' requests can be marked as completed. Current status: {Status}.");
        Status = ProcurementStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }
}
