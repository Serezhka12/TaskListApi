namespace TaskListApi.DataContracts;

/// <summary>
/// Represents a unified API response containing status code, data and error (if any).
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// HTTP status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Data payload (if available).
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error message (if exists).
    /// </summary>
    public string? Error { get; set; }
}

