using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.Invoices.Queries.GetInvoiceById;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, Result<InvoiceDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInvoiceByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<InvoiceDetailDto>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
            throw new NotFoundException(nameof(invoice), request.Id);

        var dto = new InvoiceDetailDto
        {
            Id = invoice.Id,
            ProcurementRequestId = invoice.ProcurementRequestId,
            InvoiceNumber = invoice.InvoiceNumber,
            Amount = invoice.Amount,
            PaymentDate = invoice.PaymentDate,
            Notes = invoice.Notes,
            AttachmentPath = invoice.AttachmentPath,
            CreatedAt = invoice.CreatedAt,
            UpdatedAt = invoice.UpdatedAt
        };

        return Result<InvoiceDetailDto>.Success(dto);
    }
}
