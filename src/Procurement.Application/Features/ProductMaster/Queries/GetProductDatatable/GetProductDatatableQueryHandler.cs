using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;

namespace Procurement.Application.Features.ProductMaster.Queries.GetProductDatatable;

public class GetProductDatatableQueryHandler
    : IRequestHandler<GetProductDatatableQuery, Result<ProductDatatableResponse<ProductViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Product>, bool, IOrderedQueryable<Product>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["sku"]       = (q, asc) => asc ? q.OrderBy(p => p.SKU)       : q.OrderByDescending(p => p.SKU),
            ["name"]      = (q, asc) => asc ? q.OrderBy(p => p.Name)      : q.OrderByDescending(p => p.Name),
            ["baseprice"] = (q, asc) => asc ? q.OrderBy(p => p.BasePrice) : q.OrderByDescending(p => p.BasePrice),
            ["createdat"] = (q, asc) => asc ? q.OrderBy(p => p.CreatedAt) : q.OrderByDescending(p => p.CreatedAt),
        };

    private readonly IApplicationDbContext _context;

    public GetProductDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductDatatableResponse<ProductViewModel>>> Handle(
        GetProductDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req = request.Request;
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(req.SearchTerm))
        {
            var term = req.SearchTerm.Trim();
            query = query.Where(p => p.SKU.Contains(term) || p.Name.Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        query = ApplySort(query, req.SortColumn, req.SortDirection);

        var pageNumber = req.PageNumber < 1 ? 1 : req.PageNumber;
        var pageSize = req.PageSize < 1 ? 10 : req.PageSize;

        var data = await (
            from p in query.Skip((pageNumber - 1) * pageSize).Take(pageSize)
            join c in _context.Categories on p.CategoryId equals c.Id
            join v in _context.Vendors on p.VendorId equals v.Id into vendors
            from v in vendors.DefaultIfEmpty()
            select new ProductViewModel
            {
                Id           = p.Id,
                SKU          = p.SKU,
                Name         = p.Name,
                CategoryName = c.Name,
                BasePrice    = p.BasePrice,
                UoM          = p.UoM,
                MetaData     = p.MetaData,
                VendorName   = v.Name,
                CreatedAt    = p.CreatedAt
            }
        ).ToListAsync(cancellationToken);

        var response = new ProductDatatableResponse<ProductViewModel>
        {
            TotalCount  = totalCount,
            PageNumber  = pageNumber,
            PageSize    = pageSize,
            Data        = data
        };

        return Result<ProductDatatableResponse<ProductViewModel>>.Success(response);
    }

    private static IQueryable<Product> ApplySort(
        IQueryable<Product> query,
        string? sortColumn,
        string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortColumn))
            return query.OrderBy(p => p.Name);

        var ascending = !string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return SortMap.TryGetValue(sortColumn, out var sorter)
            ? sorter(query, ascending)
            : query.OrderBy(p => p.Name);
    }
}
