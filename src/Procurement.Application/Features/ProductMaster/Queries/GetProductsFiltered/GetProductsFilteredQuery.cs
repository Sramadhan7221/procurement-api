using MediatR;
using Procurement.Application.Common;

namespace Procurement.Application.Features.ProductMaster.Queries.GetProductsFiltered;

public record GetProductsFilteredQuery : IRequest<Result<IList<ProductFilteredDto>>>
{
    public string? SKU { get; init; }
    public Guid? CategoryId { get; init; }
}
