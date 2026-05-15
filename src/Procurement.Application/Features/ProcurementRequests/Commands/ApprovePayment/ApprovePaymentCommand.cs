using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApprovePayment;

public record ApprovePaymentCommand : IRequest<Result<Guid>>
{
    public Guid ProcurementId { get; init; }
    public Guid InvoiceId { get; init; }
    public Guid ManagerUserId { get; init; }
}
