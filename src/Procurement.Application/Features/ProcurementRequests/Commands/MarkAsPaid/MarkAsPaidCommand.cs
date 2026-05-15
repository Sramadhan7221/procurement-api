using MediatR;
using Microsoft.AspNetCore.Http;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.MarkAsPaid;

public record MarkAsPaidCommand : IRequest<Result<MarkAsPaidResult>>
{
    public Guid PaymentId { get; init; }
    public Guid AdminUserId { get; init; }
    public string PaymentReference { get; init; } = string.Empty;
    public IFormFile? PaymentProofFile { get; init; }
}

public record MarkAsPaidResult(DateTime PaidAt, string PaymentReference);
