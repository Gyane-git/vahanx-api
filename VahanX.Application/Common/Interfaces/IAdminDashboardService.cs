using VahanX.Application.DTOs.Admin;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for admin dashboard operations.
/// </summary>
public interface IAdminDashboardService
{
    Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
}
