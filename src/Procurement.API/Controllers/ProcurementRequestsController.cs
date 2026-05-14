using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.ProcurementRequests.Commands.AdminReviewProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.ManagerReviewProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.UpdateProcurementProgress;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestDatatable;

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
}
