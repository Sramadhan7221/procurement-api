namespace Procurement.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? Message { get; private set; }
    public IReadOnlyList<string> Errors { get; private set; } = Array.Empty<string>();

    private Result() { }

    public static Result<T> Success(T data, string? message = null) => new()
    {
        IsSuccess = true,
        Data = data,
        Message = message
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Errors = new[] { error }
    };

    public static Result<T> Failure(IEnumerable<string> errors) => new()
    {
        IsSuccess = false,
        Errors = errors.ToList().AsReadOnly()
    };
}
