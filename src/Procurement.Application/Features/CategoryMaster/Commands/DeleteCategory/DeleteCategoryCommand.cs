using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result<Guid>>;
