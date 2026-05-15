using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.Application.Common;
using Procurement.Application.Features.CategoryMaster.Commands;
using Procurement.Application.Features.CategoryMaster.Commands.UpdateCategoryRequest;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryById;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryByName;
using Procurement.Application.Features.CategoryMaster.Queries.GetCategoryDatatable;
using Procurement.Application.Features.Users.Queries.GetUserList;

namespace Procurement.API.Controllers;

[Route("api/Users")]
public class UsersController : BaseApiController
{
    public UsersController(ISender sender) : base(sender) { }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserList(
        CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetUserListQuery(), cancellationToken);
        return ApiOk(result);
    }
}