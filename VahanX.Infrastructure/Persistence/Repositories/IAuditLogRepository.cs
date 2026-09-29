using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository interface for audit log operations.
/// </summary>
public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(string entityName, string entityId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
}
