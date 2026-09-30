using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository interface for audit log operations.
/// </summary>
public interface IAuditLogRepository
{
    Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
