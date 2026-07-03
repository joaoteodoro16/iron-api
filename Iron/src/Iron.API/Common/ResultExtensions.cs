using Iron.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Iron.Api.Common;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        return result.IsSuccess
            ? ApiResponse<T>.Ok(result.Value, result.Message)
            : ApiResponse<T>.Fail(result.Errors, result.Message);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        var response = result.ToApiResponse();

        if (result.IsSuccess)
            return new OkObjectResult(response);

        return result.ErrorType switch
        {
            ErrorType.Validation => new BadRequestObjectResult(response),
            ErrorType.Conflict => new ConflictObjectResult(response),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorType.Forbidden => new ObjectResult(response) { StatusCode = StatusCodes.Status403Forbidden },
            _ => new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }
}
