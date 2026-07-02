using System.Text.Json.Serialization;

namespace Iron.Api.Common;

public class ApiResponse<T>
{
    public bool Success { get; private init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; private init; }

    public string Message { get; private init; } = string.Empty;

    public IReadOnlyList<string> Errors { get; private init; } = [];

    public static ApiResponse<T> Ok(T data, string message = "") =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string error, string message = "") =>
        new() { Success = false, Message = message, Errors = [error] };

    public static ApiResponse<T> Fail(IReadOnlyList<string> errors, string message = "") =>
        new() { Success = false, Message = message, Errors = errors };
}
