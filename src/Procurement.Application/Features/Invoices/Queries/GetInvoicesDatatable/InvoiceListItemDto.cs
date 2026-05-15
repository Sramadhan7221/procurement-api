namespace Procurement.Application.Features.Invoices.Queries.GetInvoicesDatatable;

public class InvoiceListItemDto
{
    public Guid Id { get; set; }
    public Guid ProcurementRequestId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentPath { get; set; }
    public DateTime CreatedAt { get; set; }
}
