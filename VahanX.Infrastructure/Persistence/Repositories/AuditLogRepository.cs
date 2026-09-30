using Microsoft.EntityFrameworkCore;
using VahanX.Domain.Entities;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for audit log operations.
/// </summary>
public class AuditLogRepository : IAuditLogRepository
{
    private readonly VahanXDbContext _context;

    public AuditLogRepository(VahanXDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        await _context.AuditLogs.AddAsync(auditLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>> GetByEntityAsync(string entityType, string entityId, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLog>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .Where(a => a.ActorUserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync(cancellationToken);
    }
}
