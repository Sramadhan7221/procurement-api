using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Commands.ApproveProcurementRequest;

public record ApproveProcurementRequestCommand(Guid Id) : IRequest<Result<bool>>;
