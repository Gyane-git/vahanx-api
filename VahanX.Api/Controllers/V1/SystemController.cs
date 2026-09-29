using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// System controller for foundation verification endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SystemController : ControllerBase
{
    private readonly ISystemInfoService _systemInfoService;
    private readonly ILogger<SystemController> _logger;

    public SystemController(ISystemInfoService systemInfoService, ILogger<SystemController> logger)
    {
        _systemInfoService = systemInfoService;
        _logger = logger;
    }

    /// <summary>
    /// Get system information.
    /// </summary>
    [HttpGet("info")]
    [ProducesResponseType(typeof(ApiResponse<SystemInfoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<SystemInfoDto>>> GetSystemInfo(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting system information");

        var info = await _systemInfoService.GetSystemInfoAsync(cancellationToken);

        return Ok(ApiResponse<SystemInfoDto>.SuccessResponse(info));
    }
}
