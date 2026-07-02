namespace Iron.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<string> Errors { get; }

    public string Error => Errors.Count > 0 ? Errors[0] : string.Empty;

    protected Result(bool isSuccess, IReadOnlyList<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Ok() => new(true, []);

    public static Result Fail(string error) => new(false, [error]);

    public static Result Fail(IReadOnlyList<string> errors) => new(false, errors);

    public static Result<T> Ok<T>(T value) => new(value, true, []);

    public static Result<T> Fail<T>(string error) => new(default!, false, [error]);

    public static Result<T> Fail<T>(IReadOnlyList<string> errors) => new(default!, false, errors);

    public static Result<T> Try<T>(Func<T> factory)
    {
        try
        {
            return Ok(factory());
        }
        catch (ArgumentException ex)
        {
            return Fail<T>(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Fail<T>(ex.Message);
        }
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    protected internal Result(T value, bool isSuccess, IReadOnlyList<string> errors) : base(isSuccess, errors)
    {
        Value = value;
    }
}
