using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Moderation;

/// <summary>
/// Response DTO for a report.
/// </summary>
public class ReportResponse
{
    public Guid Id { get; set; }
    public Guid ReporterUserId { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid ReportReasonId { get; set; }
    public string? ReasonName { get; set; }
    public string? Description { get; set; }
    public ModerationReportStatus Status { get; set; }
    public ReportPriority Priority { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNote { get; set; }
}

/// <summary>
/// Request DTO for creating a report.
/// </summary>
public class CreateReportRequest
{
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid ReportReasonId { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Request DTO for assigning a report.
/// </summary>
public class AssignReportRequest
{
    public Guid AssignedToUserId { get; set; }
}

/// <summary>
/// Request DTO for resolving a report.
/// </summary>
public class ResolveReportRequest
{
    public string ResolutionNote { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for a moderation case.
/// </summary>
public class ModerationCaseResponse
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public ModerationCaseStatus Status { get; set; }
    public ReportPriority Priority { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public ModerationResolutionType? ResolutionType { get; set; }
    public string? ResolutionNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating a moderation case.
/// </summary>
public class CreateModerationCaseRequest
{
    public Guid? ReportId { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public ReportPriority Priority { get; set; } = ReportPriority.Normal;
}

/// <summary>
/// Request DTO for performing a moderation action.
/// </summary>
public class ModerationActionRequest
{
    public ModerationActionType ActionType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

/// <summary>
/// Request DTO for resolving a moderation case.
/// </summary>
public class ResolveModerationCaseRequest
{
    public ModerationResolutionType ResolutionType { get; set; }
    public string? ResolutionNote { get; set; }
}

/// <summary>
/// Response DTO for a user restriction.
/// </summary>
public class UserRestrictionResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UserRestrictionType RestrictionType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Request DTO for creating a user restriction.
/// </summary>
public class CreateUserRestrictionRequest
{
    public Guid UserId { get; set; }
    public UserRestrictionType RestrictionType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime? EndsAt { get; set; }
}

/// <summary>
/// Request DTO for warning a user.
/// </summary>
public class WarnUserRequest
{
    public string Reason { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

/// <summary>
/// Request DTO for suspending a user.
/// </summary>
public class SuspendUserRequest
{
    public string Reason { get; set; } = string.Empty;
    public DateTime? EndsAt { get; set; }
}

/// <summary>
/// Response DTO for a report reason.
/// </summary>
public class ReportReasonResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TargetType TargetType { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}
