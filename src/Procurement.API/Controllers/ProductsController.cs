using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Features.ProductMaster.Commands;
using Procurement.Application.Features.ProductMaster.Commands.DeleteProduct;
using Procurement.Application.Features.ProductMaster.Commands.UpdateProductRequest;
using Procurement.Application.Features.ProductMaster.Queries.GetProductDatatable;
using Procurement.Application.Features.ProductMaster.Queries.GetProductsFiltered;

namespace Procurement.API.Controllers;

[Route("api/products")]
public class ProductsController : BaseApiController
{
    public ProductsController(ISender sender) : base(sender) { }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromForm] CreateProductRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return ApiCreated(result);
    }

    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateProductRequestCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { Id = id }, cancellationToken);
        return ApiOk(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new DeleteProductCommand(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpPost("datatable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Datatable(
        [FromBody] ProductDatatableRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetProductDatatableQuery(request), cancellationToken);
        return ApiOk(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] string? sku,
        [FromQuery] Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new GetProductsFilteredQuery { SKU = sku, CategoryId = categoryId },
            cancellationToken);
        return ApiOk(result);
    }
}
