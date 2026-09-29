namespace VahanX.Application.DTOs;

/// <summary>
/// System information DTO returned by the system info endpoint.
/// </summary>
public class SystemInfoDto
{
    public string ApplicationName { get; set; } = string.Empty;

    public string ApiVersion { get; set; } = string.Empty;

    public string Environment { get; set; } = string.Empty;

    public DateTime CurrentUtcTime { get; set; }

    public string DotNetVersion { get; set; } = string.Empty;
}
