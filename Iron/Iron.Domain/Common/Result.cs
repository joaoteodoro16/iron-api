namespace Iron.Domain.Common;

public class Result
{
    private const string DefaultSuccessMessage = "Operação realizada com sucesso";
    private const string DefaultFailureMessage = "Não foi possível concluir essa operação";

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<string> Errors { get; }

    public string Error => Errors.Count > 0 ? Errors[0] : string.Empty;

    public ErrorType ErrorType { get; }

    public string Message { get; }

    protected Result(bool isSuccess, IReadOnlyList<string> errors, ErrorType errorType, string message)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        ErrorType = errorType;
        Message = message;
    }

    public static Result Ok(string? message = null) =>
        new(true, [], ErrorType.Failure, message ?? DefaultSuccessMessage);

    public static Result Fail(string error, ErrorType errorType = ErrorType.Failure, string? message = null) =>
        new(false, [error], errorType, message ?? (errorType == ErrorType.Failure ? DefaultFailureMessage : error));

    public static Result Fail(IReadOnlyList<string> errors, ErrorType errorType = ErrorType.Failure, string? message = null) =>
        new(false, errors, errorType, message ?? DefaultMessageFor(errors, errorType));

    public static Result<T> Ok<T>(T value, string? message = null) =>
        new(value, true, [], ErrorType.Failure, message ?? DefaultSuccessMessage);

    public static Result<T> Fail<T>(string error, ErrorType errorType = ErrorType.Failure, string? message = null) =>
        new(default!, false, [error], errorType, message ?? (errorType == ErrorType.Failure ? DefaultFailureMessage : error));

    public static Result<T> Fail<T>(IReadOnlyList<string> errors, ErrorType errorType = ErrorType.Failure, string? message = null) =>
        new(default!, false, errors, errorType, message ?? DefaultMessageFor(errors, errorType));

    public static Result<T> Try<T>(Func<T> factory)
    {
        try
        {
            return Ok(factory());
        }
        catch (ArgumentException ex)
        {
            return Fail<T>(ex.Message, ErrorType.Validation);
        }
        catch (InvalidOperationException ex)
        {
            return Fail<T>(ex.Message, ErrorType.Conflict);
        }
    }

    private static string DefaultMessageFor(IReadOnlyList<string> errors, ErrorType errorType)
    {
        if (errorType == ErrorType.Failure || errors.Count == 0)
            return DefaultFailureMessage;

        return errors[0];
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, IReadOnlyList<string> errors, ErrorType errorType, string message)
        : base(isSuccess, errors, errorType, message)
    {
        Value = value;
    }
}
