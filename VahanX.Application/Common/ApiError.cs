namespace VahanX.Application.Common;

/// <summary>
/// Represents a single error in the API response.
/// </summary>
public class ApiError
{
    public string Code { get; set; } = string.Empty;

    public string Field { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
