using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.RoleMaster.Queries.GetRoleDatatable;

public class GetRoleDatatableQueryHandler
    : IRequestHandler<GetRoleDatatableQuery, Result<DatatableResponse<RoleViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Domain.Entities.Role>, bool, IOrderedQueryable<Domain.Entities.Role>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = (q, asc) => asc ? q.OrderBy(r => r.Name) : q.OrderByDescending(r => r.Name),
        };

    private readonly IApplicationDbContext _context;

    public GetRoleDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<RoleViewModel>>> Handle(
        GetRoleDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req = request.Request;
        var query = _context.Roles.AsNoTracking();

        var recordsTotal = await query.CountAsync(cancellationToken);

        var searchValue = req.Search?.Value?.Trim();
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(r => r.Name.Contains(searchValue));
        }

        var recordsFiltered = await query.CountAsync(cancellationToken);

        query = ApplySort(query, req);

        var data = await query
            .Skip(req.Start)
            .Take(req.Length > 0 ? req.Length : 10)
            .Select(r => new RoleViewModel
            {
                Id   = r.Id,
                Name = r.Name
            })
            .ToListAsync(cancellationToken);

        var response = new DatatableResponse<RoleViewModel>
        {
            Draw            = req.Draw,
            RecordsTotal    = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data            = data
        };

        return Result<DatatableResponse<RoleViewModel>>.Success(response);
    }

    private static IQueryable<Domain.Entities.Role> ApplySort(
        IQueryable<Domain.Entities.Role> query,
        DatatableRequest req)
    {
        if (req.Order.Count == 0 || req.Columns.Count == 0)
            return query.OrderBy(r => r.Name);

        var orderInfo  = req.Order[0];
        var columnName = req.Columns.ElementAtOrDefault(orderInfo.Column)?.Data ?? string.Empty;
        var ascending  = !string.Equals(orderInfo.Dir, "desc", StringComparison.OrdinalIgnoreCase);

        return SortMap.TryGetValue(columnName, out var sorter)
            ? sorter(query, ascending)
            : query.OrderBy(r => r.Name);
    }
}
