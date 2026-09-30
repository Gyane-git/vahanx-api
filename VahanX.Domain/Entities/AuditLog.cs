using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Audit log entity for tracking all significant actions in the system.
/// Append-only: never update or delete audit records.
/// </summary>
public class AuditLog : BaseEntity
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

    public DateTime Timestamp { get; set; }

    public AuditResult Result { get; set; } = AuditResult.Success;

    public string? FailureReason { get; set; }

    // Legacy property for backward compatibility
    public string? UserId { get; set; }
}
