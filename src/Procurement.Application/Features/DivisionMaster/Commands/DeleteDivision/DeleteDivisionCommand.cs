using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.DivisionMaster.Commands.DeleteDivision;

public record DeleteDivisionCommand(Guid Id) : IRequest<Result<Guid>>;
