using Microsoft.Extensions.Configuration;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of system info service.
/// </summary>
public class SystemInfoService : ISystemInfoService
{
    private readonly IConfiguration _configuration;

    public SystemInfoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<SystemInfoDto> GetSystemInfoAsync(CancellationToken cancellationToken = default)
    {
        var info = new SystemInfoDto
        {
            ApplicationName = _configuration["ApplicationName"] ?? _configuration["applicationName"] ?? "VahanX API",
            ApiVersion = _configuration["ApiVersion"] ?? _configuration["apiVersion"] ?? "v1",
            Environment = _configuration["Environment"] ?? _configuration["environment"] ?? "Production",
            CurrentUtcTime = DateTime.UtcNow,
            DotNetVersion = Environment.Version.ToString()
        };

        return Task.FromResult(info);
    }
}
