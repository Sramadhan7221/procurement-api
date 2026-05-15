using MediatR;
using Microsoft.AspNetCore.Mvc;
using Procurement.API.Common;
using Procurement.Application.Common;

namespace Procurement.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected BaseApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult ApiOk<T>(Result<T> result)
    {
        if (!result.IsSuccess)
            return ApiError(result);

        return base.Ok(new ApiResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status200OK,
            Message = result.Message,
            Data = result.Data
        });
    }

    protected IActionResult ApiCreated<T>(Result<T> result)
    {
        if (!result.IsSuccess)
            return ApiError(result);

        return StatusCode(StatusCodes.Status201Created, new ApiResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status201Created,
            Message = result.Message,
            Data = result.Data
        });
    }

    protected IActionResult ApiCreatedAt<T>(Result<T> result, string actionName, object? routeValues = null)
    {
        if (!result.IsSuccess)
            return ApiError(result);

        return CreatedAtAction(actionName, routeValues, new ApiResponse<T>
        {
            Success = true,
            StatusCode = StatusCodes.Status201Created,
            Message = result.Message,
            Data = result.Data
        });
    }

    private IActionResult ApiError<T>(Result<T> result)
    {
        return base.BadRequest(new ApiResponse<T>
        {
            Success = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = result.Errors
        });
    }
}
