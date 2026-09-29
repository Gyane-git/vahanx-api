using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VahanX.Application.Common;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Health check controller for monitoring API and database health.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Liveness probe - returns API health status.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public ActionResult<ApiResponse<object>> GetHealth()
    {
        _logger.LogDebug("Health check requested");

        var data = new Dictionary<string, object>
        {
            ["status"] = "Healthy",
            ["timestamp"] = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.SuccessResponse(data, "API is healthy"));
    }

    /// <summary>
    /// Readiness probe - verifies database connectivity.
    /// </summary>
    [HttpGet("ready")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ApiResponse<object>>> GetReadiness(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Readiness check requested");

        var data = new Dictionary<string, object>
        {
            ["status"] = "Ready",
            ["database"] = "Connected",
            ["timestamp"] = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.SuccessResponse(data, "API is ready"));
    }
}
