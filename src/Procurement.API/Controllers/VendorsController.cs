using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.VendorMaster.Commands;
using Procurement.Application.Features.VendorMaster.Commands.UpdateVendorRequest;
using Procurement.Application.Features.VendorMaster.Queries.GetVendorById;
using Procurement.Application.Features.VendorMaster.Queries.GetVendorDatatable;

namespace Procurement.API.Controllers;

[Route("api/vendors")]
public class VendorsController : BaseApiController
{
    public VendorsController(ISender sender) : base(sender) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateVendorRequestCommand command,
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
        var result = await Sender.Send(new GetVendorByIdQuery(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateVendorRequestCommand command,
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
        var result = await Sender.Send(new GetVendorDatatableQuery(request), cancellationToken);
        return ApiOk(result);
    }
}
