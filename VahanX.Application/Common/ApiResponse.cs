namespace VahanX.Application.Common;

/// <summary>
/// Standard API response wrapper for all endpoints.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<ApiError> Errors { get; set; } = [];

    public string TraceId { get; set; } = string.Empty;

    public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            TraceId = Guid.NewGuid().ToString()
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, List<ApiError>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors ?? [],
            TraceId = Guid.NewGuid().ToString()
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, string errorCode, string field, string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = [new ApiError { Code = errorCode, Field = field, Message = errorMessage }],
            TraceId = Guid.NewGuid().ToString()
        };
    }
}
