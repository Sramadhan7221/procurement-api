using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.DivisionMaster.Queries.GetDivisionById;

public record GetDivisionByIdQuery(Guid Id) : IRequest<Result<DivisionDetailDto>>;
