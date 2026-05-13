using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.ProductMaster.Queries.GetProductsFiltered;

public class GetProductsFilteredQueryHandler
    : IRequestHandler<GetProductsFilteredQuery, Result<IList<ProductFilteredDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsFilteredQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IList<ProductFilteredDto>>> Handle(
        GetProductsFilteredQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SKU))
            query = query.Where(p => p.SKU == request.SKU);

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);

        var data = await query
            .OrderBy(p => p.Name)
            .Select(p => new ProductFilteredDto
            {
                Id         = p.Id,
                SKU        = p.SKU,
                Name       = p.Name,
                CategoryId = p.CategoryId,
                BasePrice  = p.BasePrice,
                UoM        = p.UoM,
                MetaData   = p.MetaData,
                VendorId   = p.VendorId,
                CreatedAt  = p.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<IList<ProductFilteredDto>>.Success(data);
    }
}
