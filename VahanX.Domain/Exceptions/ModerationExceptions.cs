namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when a moderation case is not found.
/// </summary>
public class ModerationCaseNotFoundException : DomainException
{
    public ModerationCaseNotFoundException(Guid caseId)
        : base($"Moderation case '{caseId}' not found.", "MODERATION_CASE_NOT_FOUND")
    {
    }
}

/// <summary>
/// Exception thrown when a report is not found.
/// </summary>
public class ReportNotFoundException : DomainException
{
    public ReportNotFoundException(Guid reportId)
        : base($"Report '{reportId}' not found.", "REPORT_NOT_FOUND")
    {
    }
}

/// <summary>
/// Exception thrown when a user attempts an unauthorized moderation action.
/// </summary>
public class UnauthorizedModerationActionException : DomainException
{
    public UnauthorizedModerationActionException(string message)
        : base(message, "UNAUTHORIZED_MODERATION_ACTION")
    {
    }
}

/// <summary>
/// Exception thrown when an invalid status transition is attempted.
/// </summary>
public class InvalidModerationTransitionException : DomainException
{
    public InvalidModerationTransitionException(string message)
        : base(message, "INVALID_MODERATION_TRANSITION")
    {
    }
}

/// <summary>
/// Exception thrown when a user restriction is not found.
/// </summary>
public class UserRestrictionNotFoundException : DomainException
{
    public UserRestrictionNotFoundException(Guid restrictionId)
        : base($"User restriction '{restrictionId}' not found.", "USER_RESTRICTION_NOT_FOUND")
    {
    }
}
