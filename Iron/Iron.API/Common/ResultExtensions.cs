using Iron.Domain.Common;

namespace Iron.Api.Common;

public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, string successMessage = "")
    {
        return result.IsSuccess
            ? ApiResponse<T>.Ok(result.Value, successMessage)
            : ApiResponse<T>.Fail(result.Errors);
    }
}
