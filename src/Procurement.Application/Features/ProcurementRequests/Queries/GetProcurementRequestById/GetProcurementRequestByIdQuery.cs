using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

public record GetProcurementRequestByIdQuery(Guid Id) : IRequest<Result<ProcurementRequestDetailDto>>;
