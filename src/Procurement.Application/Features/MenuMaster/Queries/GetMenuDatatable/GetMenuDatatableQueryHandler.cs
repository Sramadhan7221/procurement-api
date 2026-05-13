using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.MenuMaster.Queries.GetMenuDatatable;

public class GetMenuDatatableQueryHandler
    : IRequestHandler<GetMenuDatatableQuery, Result<DatatableResponse<MenuViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Domain.Entities.Menu>, bool, IOrderedQueryable<Domain.Entities.Menu>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]     = (q, asc) => asc ? q.OrderBy(m => m.Name)     : q.OrderByDescending(m => m.Name),
            ["sequence"] = (q, asc) => asc ? q.OrderBy(m => m.Sequence) : q.OrderByDescending(m => m.Sequence),
        };

    private readonly IApplicationDbContext _context;

    public GetMenuDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<MenuViewModel>>> Handle(
        GetMenuDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req   = request.Request;
        var query = _context.Menus.AsNoTracking();

        var recordsTotal = await query.CountAsync(cancellationToken);

        var searchValue = req.Search?.Value?.Trim();
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(m =>
                m.Name.Contains(searchValue) ||
                (m.Url != null && m.Url.Contains(searchValue)));
        }

        var recordsFiltered = await query.CountAsync(cancellationToken);

        query = ApplySort(query, req);

        var data = await query
            .Skip(req.Start)
            .Take(req.Length > 0 ? req.Length : 10)
            .Select(m => new MenuViewModel
            {
                Id           = m.Id,
                Name         = m.Name,
                Url          = m.Url,
                Icon         = m.Icon,
                Sequence     = m.Sequence,
                ParentMenuId = m.ParentMenuId
            })
            .ToListAsync(cancellationToken);

        var response = new DatatableResponse<MenuViewModel>
        {
            Draw            = req.Draw,
            RecordsTotal    = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data            = data
        };

        return Result<DatatableResponse<MenuViewModel>>.Success(response);
    }

    private static IQueryable<Domain.Entities.Menu> ApplySort(
        IQueryable<Domain.Entities.Menu> query,
        DatatableRequest req)
    {
        if (req.Order.Count == 0 || req.Columns.Count == 0)
            return query.OrderBy(m => m.Sequence);

        var orderInfo  = req.Order[0];
        var columnName = req.Columns.ElementAtOrDefault(orderInfo.Column)?.Data ?? string.Empty;
        var ascending  = !string.Equals(orderInfo.Dir, "desc", StringComparison.OrdinalIgnoreCase);

        return SortMap.TryGetValue(columnName, out var sorter)
            ? sorter(query, ascending)
            : query.OrderBy(m => m.Sequence);
    }
}
