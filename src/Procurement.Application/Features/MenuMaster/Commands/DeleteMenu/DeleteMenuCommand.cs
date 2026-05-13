using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.MenuMaster.Commands.DeleteMenu;

public record DeleteMenuCommand(Guid Id) : IRequest<Result<Guid>>;
