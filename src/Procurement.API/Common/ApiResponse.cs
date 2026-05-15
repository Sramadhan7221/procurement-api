namespace Procurement.API.Common;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
}
