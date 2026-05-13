using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Common;

namespace Procurement.Application.Features.Invoices.Commands.CreateInvoice;

public record CreateInvoiceCommand : IRequest<Result<Guid>>
{
    public Guid ProcurementRequestId { get; init; }
    public decimal Amount { get; init; }
    public DateTime PaymentDate { get; init; }
    public string? Notes { get; init; }
    public IFormFile? AttachmentFile { get; init; }
}
