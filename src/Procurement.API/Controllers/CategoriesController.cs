using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.CategoryMaster.Commands;
using Procurement.Application.Features.CategoryMaster.Commands.UpdateCategoryRequest;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryById;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryByName;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryDatatable;

namespace Procurement.API.Controllers;

[Route("api/Categories")]
public class CategoriesController : BaseApiController
{
    public CategoriesController(ISender sender) : base(sender) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequestCommand command,
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
        var result = await Sender.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(
        [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetCategoryByNameQuery(name), cancellationToken);
        return ApiOk(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCategoryRequestCommand command,
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
        var result = await Sender.Send(new GetCategoryDatatableQuery(request), cancellationToken);
        return ApiOk(result);
    }
}
