using MediatR;
using Microsoft.EntityFrameworkCore;
using Procurement.Application.Common;
using Procurement.Application.Interfaces;
using Procurement.Domain.Entities;
using Procurement.Domain.Enums;
using Procurement.Domain.Exceptions;

namespace Procurement.Application.Features.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileService _fileService;

    public CreateInvoiceCommandHandler(IApplicationDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Result<Guid>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var procurementRequest = await _context.ProcurementRequests
            .Include(r => r.Invoice)
            .FirstOrDefaultAsync(r => r.Id == request.ProcurementRequestId, cancellationToken);

        if (procurementRequest is null)
            throw new NotFoundException(nameof(ProcurementRequest), request.ProcurementRequestId);

        if (procurementRequest.Status != ProcurementStatus.ApproveByAdmin)
            return Result<Guid>.Failure(
                $"Invoice can only be created for requests with status 'ApproveByAdmin'. Current status: {procurementRequest.Status}.");

        if (procurementRequest.Invoice is not null)
            return Result<Guid>.Failure("An invoice already exists for this procurement request.");

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            string? attachmentPath = null;
            if (request.AttachmentFile is not null)
                attachmentPath = await _fileService.UploadFileAsync(request.AttachmentFile, "invoices");

            var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            var invoice = new Invoice
            {
                ProcurementRequestId = request.ProcurementRequestId,
                InvoiceNumber = invoiceNumber,
                Amount = request.Amount,
                PaymentDate = request.PaymentDate,
                Notes = request.Notes,
                AttachmentPath = attachmentPath
            };

            _context.Invoices.Add(invoice);
            procurementRequest.SetInOrder();

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Result<Guid>.Success(invoice.Id, "Invoice created successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
