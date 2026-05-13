using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;

namespace Procurement.Application.Features.Invoices.Queries.GetInvoicesDatatable;

public class GetInvoicesDatatableQueryHandler
    : IRequestHandler<GetInvoicesDatatableQuery, DatatableResponse<InvoiceListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInvoicesDatatableQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DatatableResponse<InvoiceListItemDto>> Handle(
        GetInvoicesDatatableQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Invoices
            .Where(i => !i.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(i =>
                i.InvoiceNumber.ToLower().Contains(search) ||
                (i.Notes != null && i.Notes.ToLower().Contains(search)));
        }

        var totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip(request.Start)
            .Take(request.Length > 0 ? request.Length : 10)
            .Select(i => new InvoiceListItemDto
            {
                Id = i.Id,
                ProcurementRequestId = i.ProcurementRequestId,
                InvoiceNumber = i.InvoiceNumber,
                Amount = i.Amount,
                PaymentDate = i.PaymentDate,
                Notes = i.Notes,
                AttachmentPath = i.AttachmentPath,
                CreatedAt = i.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new DatatableResponse<InvoiceListItemDto>
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };
    }
}
