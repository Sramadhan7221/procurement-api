using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.DivisionMaster.Queries.GetDivisionDatatable;

public class GetDivisionDatatableQueryHandler
    : IRequestHandler<GetDivisionDatatableQuery, Result<DatatableResponse<DivisionViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Domain.Entities.Division>, bool, IOrderedQueryable<Domain.Entities.Division>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]         = (q, asc) => asc ? q.OrderBy(v => v.Name)         : q.OrderByDescending(v => v.Name),
        };

    private readonly IApplicationDbContext _context;

    public GetDivisionDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<DivisionViewModel>>> Handle(
        GetDivisionDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req = request.Request;
        var query = _context.Divisions.AsNoTracking();

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
            .Select(v => new DivisionViewModel
            {
                Id           = v.Id,
                Name         = v.Name
            })
            .ToListAsync(cancellationToken);

        var response = new DatatableResponse<DivisionViewModel>
        {
            Draw            = req.Draw,
            RecordsTotal    = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data            = data
        };

        return Result<DatatableResponse<DivisionViewModel>>.Success(response);
    }

    private static IQueryable<Domain.Entities.Division> ApplySort(
        IQueryable<Domain.Entities.Division> query,
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
