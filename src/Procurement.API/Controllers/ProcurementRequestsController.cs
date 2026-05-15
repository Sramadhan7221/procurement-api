using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.ProcurementRequests.Commands.AdminReviewProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.ApprovePayment;
using Procurement.Application.Features.ProcurementRequests.Commands.ConfirmGoodsReceipt;
using Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.ManagerReviewProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.MarkAsPaid;
using Procurement.Application.Features.ProcurementRequests.Commands.PlaceOrder;
using Procurement.Application.Features.ProcurementRequests.Commands.ResolveInvoiceDispute;
using Procurement.Application.Features.ProcurementRequests.Commands.UpdateProcurementProgress;
using Procurement.Application.Features.ProcurementRequests.Commands.UploadInvoice;
using Procurement.Application.Features.ProcurementRequests.Commands.VerifyInvoice;
using Procurement.Application.Features.ProcurementRequests.Queries.GetGoodsReceipt;
using Procurement.Application.Features.ProcurementRequests.Queries.GetInvoiceDetail;
using Procurement.Application.Features.ProcurementRequests.Queries.GetPaymentDetail;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestDatatable;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementTimeline;
using Procurement.Application.Features.ProcurementRequests.Queries.GetPurchaseOrder;

namespace Procurement.API.Controllers;

[Route("api/procurement-requests")]
public class ProcurementRequestsController : BaseApiController
{
    public ProcurementRequestsController(ISender sender) : base(sender) { }

    /// <summary>
    /// Admin and Manager only. Returns a paginated datatable of procurement requests.
    /// </summary>
    [HttpPost("datatable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Datatable(
        [FromBody] GetProcurementRequestDatatableQuery query,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Staff only. Creates a new procurement request with status "Request Created".
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProcurementRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiCreatedAt(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Get procurement request detail by ID (all roles).
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetProcurementRequestByIdQuery(id), cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager only. Approve or reject a "Request Created" procurement request with an optional comment.
    /// </summary>
    [HttpPut("{id:guid}/manager-review")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ManagerReview(
        Guid id,
        [FromBody] ManagerReviewProcurementRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { Id = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Admin only. Approve or reject a Manager-approved procurement request with an optional comment.
    /// </summary>
    [HttpPut("{id:guid}/admin-review")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> AdminReview(
        Guid id,
        [FromBody] AdminReviewProcurementRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { Id = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Admin only. Advance procurement progress through: InOrderByAdmin → OrderReceived → Completed.
    /// </summary>
    [HttpPut("{id:guid}/progress")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateProgress(
        Guid id,
        [FromBody] UpdateProcurementProgressCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { Id = id }, cancellationToken);
        return ApiOk(result);
    }

    // ─── Invoice & Payment Flow ────────────────────────────────────────────────

    /// <summary>
    /// Admin only. Generate a Purchase Order for an Admin-approved request (status 4 → 6).
    /// </summary>
    [HttpPost("{id:guid}/place-order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> PlaceOrder(
        Guid id,
        [FromBody] PlaceOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { ProcurementId = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Staff only. Confirm goods receipt with optional delivery order file (status 6 → 7).
    /// </summary>
    [HttpPost("{id:guid}/goods-receipt")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ConfirmGoodsReceipt(
        Guid id,
        [FromForm] ConfirmGoodsReceiptCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { ProcurementId = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Admin only. Upload vendor invoice PDF and run 3-way matching (status 7 → 9 or 10).
    /// </summary>
    [HttpPost("{id:guid}/invoice")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UploadInvoice(
        Guid id,
        [FromForm] UploadInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { ProcurementId = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Admin only. Accept or reject a disputed invoice (status 10 → 9 on accept).
    /// </summary>
    [HttpPut("{id:guid}/invoice/dispute/resolve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ResolveInvoiceDispute(
        Guid id,
        [FromBody] ResolveInvoiceDisputeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager only. Verify the uploaded invoice (status 9 → 11).
    /// </summary>
    [HttpPut("{id:guid}/invoice/verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> VerifyInvoice(
        Guid id,
        [FromBody] VerifyInvoiceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager only. Approve payment for a verified invoice (status 11 → 12).
    /// </summary>
    [HttpPost("{id:guid}/payment/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ApprovePayment(
        Guid id,
        [FromBody] ApprovePaymentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { ProcurementId = id }, cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Admin only. Mark payment as paid and complete the procurement (status 12 → 8).
    /// </summary>
    [HttpPut("{id:guid}/payment/mark-paid")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> MarkAsPaid(
        Guid id,
        [FromForm] MarkAsPaidCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiOk(result);
    }

    // ─── Timeline & Detail Queries ─────────────────────────────────────────────

    /// <summary>
    /// Manager and Admin. Returns full audit timeline for a procurement request.
    /// </summary>
    [HttpGet("{id:guid}/timeline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTimeline(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetProcurementTimelineQuery(id), cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager and Admin. Returns the purchase order for a procurement request.
    /// </summary>
    [HttpGet("{id:guid}/purchase-order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPurchaseOrder(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetPurchaseOrderQuery(id), cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager and Admin. Returns the goods receipt for a procurement request.
    /// </summary>
    [HttpGet("{id:guid}/goods-receipt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGoodsReceipt(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetGoodsReceiptQuery(id), cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager and Admin. Returns invoice detail with 3-way matching comparison table.
    /// </summary>
    [HttpGet("{id:guid}/invoice")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoiceDetail(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetInvoiceDetailQuery(id), cancellationToken);
        return ApiOk(result);
    }

    /// <summary>
    /// Manager and Admin. Returns payment detail including proof of payment.
    /// </summary>
    [HttpGet("{id:guid}/payment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentDetail(Guid id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetPaymentDetailQuery(id), cancellationToken);
        return ApiOk(result);
    }
}
