using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.DivisionMaster.Commands;
using Procurement.Application.Features.DivisionMaster.Commands.UpdateDivisionRequest;
using Procurement.Application.Features.DivisionMaster.Queries.GetDivisionById;
using Procurement.Application.Features.DivisionMaster.Queries.GetDivisionDatatable;

namespace Procurement.API.Controllers;

[Route("api/Divisions")]
public class DivisionsController : BaseApiController
{
    public DivisionsController(ISender sender) : base(sender) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDivisionRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiCreated(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetDivisionByIdQuery(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateDivisionRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiOk(result);
    }

    [HttpPost("datatable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Datatable(
        [FromBody] DatatableRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetDivisionDatatableQuery(request), cancellationToken);
        return ApiOk(result);
    }
}
