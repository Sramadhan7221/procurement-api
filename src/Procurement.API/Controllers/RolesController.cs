using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.RoleMaster.Commands.CreateRoleRequest;
using Procurement.Application.Features.RoleMaster.Commands.DeleteRole;
using Procurement.Application.Features.RoleMaster.Commands.UpdateRoleRequest;
using Procurement.Application.Features.RoleMaster.Queries.GetRoleById;
using Procurement.Application.Features.RoleMaster.Queries.GetRoleDatatable;
using Procurement.Application.Features.RoleMenuMaster.Commands.AssignMenuToRole;
using Procurement.Application.Features.RoleMenuMaster.Commands.RemoveMenuFromRole;
using Procurement.Application.Features.RoleMenuMaster.Queries.GetMenusByRoleId;

namespace Procurement.API.Controllers;

[Route("api/roles")]
public class RolesController : BaseApiController
{
    public RolesController(ISender sender) : base(sender) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleRequestCommand command,
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
        var result = await Sender.Send(new GetRoleByIdQuery(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRoleRequestCommand command,
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
        var result = await Sender.Send(new DeleteRoleCommand(id), cancellationToken);
        return ApiOk(result);
    }

    [HttpPost("datatable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Datatable(
        [FromBody] DatatableRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetRoleDatatableQuery(request), cancellationToken);
        return ApiOk(result);
    }

    [HttpGet("{roleId:guid}/menus")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMenus(
        Guid roleId,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetMenusByRoleIdQuery(roleId), cancellationToken);
        return ApiOk(result);
    }

    [HttpPost("{roleId:guid}/menus")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignMenu(
        Guid roleId,
        [FromBody] AssignMenuToRoleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command with { RoleId = roleId }, cancellationToken);
        return ApiCreated(result);
    }

    [HttpDelete("{roleId:guid}/menus/{menuId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMenu(
        Guid roleId,
        Guid menuId,
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(
            new RemoveMenuFromRoleCommand { RoleId = roleId, MenuId = menuId },
            cancellationToken);
        return ApiOk(result);
    }
}
