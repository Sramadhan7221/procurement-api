using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.ProcurementRequests.Commands.ApproveProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Commands.CreateProcurementRequest;
using Procurement.Application.Features.ProcurementRequests.Queries.GetProcurementRequestById;

namespace Procurement.API.Controllers;

[ApiController]
[Route("api/procurement-requests")]
public class ProcurementRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public ProcurementRequestsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProcurementRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data },
            new { id = result.Data, message = result.Message });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProcurementRequestByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Data);
    }

    [HttpPut("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Approve(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ApproveProcurementRequestCommand(id), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = result.Message });
    }
}
