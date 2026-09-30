using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Audit;

/// <summary>
/// Response DTO for an audit log entry.
/// </summary>
public class AuditLogResponse
{
    public Guid Id { get; set; }
    public Guid? ActorUserId { get; set; }
    public AuditAction Action { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? Changes { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public AuditResult Result { get; set; }
    public string? FailureReason { get; set; }
}

/// <summary>
/// Request DTO for creating an audit log entry.
/// </summary>
public class CreateAuditLogRequest
{
    public Guid? ActorUserId { get; set; }
    public AuditAction Action { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? Changes { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public AuditResult Result { get; set; } = AuditResult.Success;
    public string? FailureReason { get; set; }
}

/// <summary>
/// Audit log query filters.
/// </summary>
public class AuditLogQueryParams
{
    public Guid? ActorUserId { get; set; }
    public string? EntityType { get; set; }
    public string? EntityId { get; set; }
    public AuditAction? Action { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public AuditResult? Result { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
