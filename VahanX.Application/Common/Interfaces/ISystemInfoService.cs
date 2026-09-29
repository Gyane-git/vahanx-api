using VahanX.Application.DTOs;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for retrieving system information.
/// </summary>
public interface ISystemInfoService
{
    Task<SystemInfoDto> GetSystemInfoAsync(CancellationToken cancellationToken = default);
}
