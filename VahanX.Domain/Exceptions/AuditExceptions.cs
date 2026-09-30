namespace VahanX.Domain.Exceptions;

/// <summary>
/// Exception thrown when audit access is denied.
/// </summary>
public class AuditAccessDeniedException : DomainException
{
    public AuditAccessDeniedException()
        : base("Access to audit logs is denied.", "AUDIT_ACCESS_DENIED")
    {
    }
}
