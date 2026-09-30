using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Audit;
using VahanX.Domain.Entities;

namespace VahanX.Application.Services;

/// <summary>
/// Implementation of audit service.
/// </summary>
public class AuditService : IAuditService
{
    private readonly IRepository<AuditLog> _auditLogs;

    public AuditService(IRepository<AuditLog> auditLogs)
    {
        _auditLogs = auditLogs;
    }

    public async Task<AuditLogResponse> AuditAsync(CreateAuditLogRequest request, CancellationToken cancellationToken = default)
    {
        var auditLog = new AuditLog
        {
            ActorUserId = request.ActorUserId,
            Action = request.Action,
            EntityType = request.EntityType,
            EntityId = request.EntityId,
            OldValues = request.OldValues,
            NewValues = request.NewValues,
            Changes = request.Changes,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
            CorrelationId = request.CorrelationId,
            Timestamp = DateTime.UtcNow,
            Result = request.Result,
            FailureReason = request.FailureReason
        };

        await _auditLogs.AddAsync(auditLog, cancellationToken);

        return MapToResponse(auditLog);
    }

    public async Task<PagedResult<AuditLogResponse>> GetAuditLogsAsync(AuditLogQueryParams? filters, CancellationToken cancellationToken = default)
    {
        var query = await _auditLogs.QueryAsync(true, cancellationToken);

        if (filters != null)
        {
            if (filters.ActorUserId.HasValue)
                query = query.Where(a => a.ActorUserId == filters.ActorUserId.Value);

            if (!string.IsNullOrWhiteSpace(filters.EntityType))
                query = query.Where(a => a.EntityType == filters.EntityType);

            if (!string.IsNullOrWhiteSpace(filters.EntityId))
                query = query.Where(a => a.EntityId == filters.EntityId);

            if (filters.Action.HasValue)
                query = query.Where(a => a.Action == filters.Action.Value);

            if (filters.From.HasValue)
                query = query.Where(a => a.Timestamp >= filters.From.Value);

            if (filters.To.HasValue)
                query = query.Where(a => a.Timestamp <= filters.To.Value);

            if (filters.Result.HasValue)
                query = query.Where(a => a.Result == filters.Result.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(a => a.Timestamp)
            .Skip(((filters?.Page ?? 1) - 1) * (filters?.PageSize ?? 20))
            .Take(filters?.PageSize ?? 20)
            .Select(a => new AuditLogResponse
            {
                Id = a.Id,
                ActorUserId = a.ActorUserId,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Changes = a.Changes,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                CorrelationId = a.CorrelationId,
                Timestamp = a.Timestamp,
                Result = a.Result,
                FailureReason = a.FailureReason
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AuditLogResponse>.Create(items, totalCount, filters?.Page ?? 1, filters?.PageSize ?? 20);
    }

    public async Task<AuditLogResponse?> GetAuditLogByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var auditLog = await _auditLogs.GetByIdAsync(id, cancellationToken);
        return auditLog == null ? null : MapToResponse(auditLog);
    }

    private static AuditLogResponse MapToResponse(AuditLog auditLog)
    {
        return new AuditLogResponse
        {
            Id = auditLog.Id,
            ActorUserId = auditLog.ActorUserId,
            Action = auditLog.Action,
            EntityType = auditLog.EntityType,
            EntityId = auditLog.EntityId,
            Changes = auditLog.Changes,
            IpAddress = auditLog.IpAddress,
            UserAgent = auditLog.UserAgent,
            CorrelationId = auditLog.CorrelationId,
            Timestamp = auditLog.Timestamp,
            Result = auditLog.Result,
            FailureReason = auditLog.FailureReason
        };
    }
}
