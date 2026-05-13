using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryDetailDto>>;
