using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.VendorMaster.Queries.GetVendorDatatable;

public class GetVendorDatatableQueryHandler
    : IRequestHandler<GetVendorDatatableQuery, Result<DatatableResponse<VendorViewModel>>>
{
    private static readonly Dictionary<string, Func<IQueryable<Domain.Entities.Vendor>, bool, IOrderedQueryable<Domain.Entities.Vendor>>> SortMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"]         = (q, asc) => asc ? q.OrderBy(v => v.Name)         : q.OrderByDescending(v => v.Name),
            ["contactemail"] = (q, asc) => asc ? q.OrderBy(v => v.ContactEmail) : q.OrderByDescending(v => v.ContactEmail),
            ["contactphone"] = (q, asc) => asc ? q.OrderBy(v => v.ContactPhone) : q.OrderByDescending(v => v.ContactPhone),
            ["address"]      = (q, asc) => asc ? q.OrderBy(v => v.Address)      : q.OrderByDescending(v => v.Address),
        };

    private readonly IApplicationDbContext _context;

    public GetVendorDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DatatableResponse<VendorViewModel>>> Handle(
        GetVendorDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var req = request.Request;
        var query = _context.Vendors.AsNoTracking();

        var recordsTotal = await query.CountAsync(cancellationToken);

        var searchValue = req.Search?.Value?.Trim();
        if (!string.IsNullOrEmpty(searchValue))
        {
            query = query.Where(v =>
                v.Name.Contains(searchValue) ||
                v.ContactEmail.Contains(searchValue) ||
                v.ContactPhone.Contains(searchValue) ||
                v.Address.Contains(searchValue));
        }

        var recordsFiltered = await query.CountAsync(cancellationToken);

        query = ApplySort(query, req);

        var data = await query
            .Skip(req.Start)
            .Take(req.Length > 0 ? req.Length : 10)
            .Select(v => new VendorViewModel
            {
                Id           = v.Id,
                Name         = v.Name,
                ContactEmail = v.ContactEmail,
                ContactPhone = v.ContactPhone,
                Address      = v.Address
            })
            .ToListAsync(cancellationToken);

        var response = new DatatableResponse<VendorViewModel>
        {
            Draw            = req.Draw,
            RecordsTotal    = recordsTotal,
            RecordsFiltered = recordsFiltered,
            Data            = data
        };

        return Result<DatatableResponse<VendorViewModel>>.Success(response);
    }

    private static IQueryable<Domain.Entities.Vendor> ApplySort(
        IQueryable<Domain.Entities.Vendor> query,
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
