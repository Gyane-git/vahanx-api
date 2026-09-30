using VahanX.Application.Common;
using VahanX.Application.DTOs.Audit;

namespace VahanX.Application.Common.Interfaces;

/// <summary>
/// Service for audit logging operations.
/// </summary>
public interface IAuditService
{
    Task<AuditLogResponse> AuditAsync(CreateAuditLogRequest request, CancellationToken cancellationToken = default);
    Task<PagedResult<AuditLogResponse>> GetAuditLogsAsync(AuditLogQueryParams? filters, CancellationToken cancellationToken = default);
    Task<AuditLogResponse?> GetAuditLogByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
