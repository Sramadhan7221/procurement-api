using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.CategoryMaster.Queries.GetCategoryDatatable;

public class GetCategoryDatatableQueryHandler
    : IRequestHandler<GetCategoryDatatableQuery, Result<DatatableResponse<CategoryViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Domain.Entities.Category>, bool, IOrderedQueryable<Domain.Entities.Category>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]         = (q, asc) => asc ? q.OrderBy(v => v.Name)         : q.OrderByDescending(v => v.Name),
        };

    private readonly IApplicationDbContext _context;

    public GetCategoryDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<CategoryViewModel>>> Handle(
        GetCategoryDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req = request.Request;
        var query = _context.Categories.AsNoTracking();

        var recordsTotal = await query.CountAsync(cancellationToken);

        var searchValue = req.Search?.Value?.Trim();
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(v =>
                v.Name.Contains(searchValue) 
            );
        }

        var recordsFiltered = await query.CountAsync(cancellationToken);

        query = ApplySort(query, req);

        var data = await query
            .Skip(req.Start)
            .Take(req.Length > 0 ? req.Length : 10)
            .Select(v => new CategoryViewModel
            {
                Id           = v.Id,
                Name         = v.Name
            })
            .ToListAsync(cancellationToken);

        var response = new DatatableResponse<CategoryViewModel>
        {
            Draw            = req.Draw,
            RecordsTotal    = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data            = data
        };

        return Result<DatatableResponse<CategoryViewModel>>.Success(response);
    }

    private static IQueryable<Domain.Entities.Category> ApplySort(
        IQueryable<Domain.Entities.Category> query,
        DatatableRequest req)
    {
        if (req.Order.Count == 0 || req.Columns.Count == 0)
            return query.OrderBy(v => v.Name);

        var orderInfo  = req.Order[0];
        var columnName = req.Columns.ElementAtOrDefault(orderInfo.Column)?.Data ?? string.Empty;
        var ascending  = !string.Equals(orderInfo.Dir, "desc", StringComparison.OrdinalIgnoreCase);

        return SortMap.TryGetValue(columnName, out var sorter)
            ? sorter(query, ascending)
            : query.OrderBy(v => v.Name);
    }
}
