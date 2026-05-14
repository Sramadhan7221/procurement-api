using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.Invoices.Commands.CreateInvoice;
using Procurement.Application.Features.Invoices.Queries.GetInvoiceById;
using Procurement.Application.Features.Invoices.Queries.GetInvoicesDatatable;

namespace Procurement.API.Controllers;

[Route("api/invoices")]
public class InvoicesController : BaseApiController
{
    public InvoicesController(ISender sender) : base(sender) { }

    /// <summary>
    /// Create an invoice with optional file attachment. Requires ProcurementRequest status = ApproveByAdmin.
    /// Transitions status to InOrderByAdmin atomically.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromForm] CreateInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiCreatedAt(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Get invoice by ID with full attachment URL.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetInvoiceByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return ApiOk(result);

        var dto = result.Data!;
        var attachmentUrl = dto.AttachmentPath is not null
            ? $"{Request.Scheme}://{Request.Host}{dto.AttachmentPath}"
            : null;

        var enriched = Result<object>.Success(new
        {
            dto.Id,
            dto.ProcurementRequestId,
            dto.InvoiceNumber,
            dto.Amount,
            dto.PaymentDate,
            dto.Notes,
            AttachmentUrl = attachmentUrl,
            dto.CreatedAt,
            dto.UpdatedAt
        }, result.Message);

        return ApiOk(enriched);
    }

    /// <summary>
    /// Paginated invoice list (DataTables-compatible).
    /// </summary>
    [HttpPost("datatable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Datatable(
        [FromBody] GetInvoicesDatatableQuery query,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var data = result.Data.Select(i => new
        {
            i.Id,
            i.ProcurementRequestId,
            i.InvoiceNumber,
            i.Amount,
            i.PaymentDate,
            i.Notes,
            AttachmentUrl = i.AttachmentPath is not null ? $"{baseUrl}{i.AttachmentPath}" : null,
            i.CreatedAt
        }).ToList();

        var enriched = Result<object>.Success(new
        {
            result.Draw,
            result.RecordsTotal,
            result.RecordsFiltered,
            Data = data
        });

        return ApiOk(enriched);
    }
}
