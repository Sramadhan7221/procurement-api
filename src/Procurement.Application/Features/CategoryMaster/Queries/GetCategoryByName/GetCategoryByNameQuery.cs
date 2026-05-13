using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryByName;

public record GetCategoryByNameQuery(string Name) : IRequest<Result<CategoryDetailDto>>;
